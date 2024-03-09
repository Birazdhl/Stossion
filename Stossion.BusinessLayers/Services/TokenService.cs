using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stossion.BusinessLayers.Interfaces;
using Stossion.DbManagement.StossionDbManagement;
using Stossion.Domain;
using Stossion.ViewModels.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.BusinessLayers.Services
{
	public class TokenService(StossionDbContext _context,
		IConfiguration _config,
		UserManager<StossionUser> _userManager,
		RoleManager<IdentityRole> _roleManager) : ITokenInterface
	{
		public string GenerateToken(UserSession user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			var userClaims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.UserId),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.Role),
				new Claim(ClaimTypes.GivenName, user.FirstName),
				new Claim(ClaimTypes.Surname, user.LastName)
			};
			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Audience"],
				claims: userClaims,
				expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:AccessTokenExpirationMinutes"])),
			signingCredentials: credentials
				);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public string GenerateRefreshToken()
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:RefreshTokenSecret"]!));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
			   issuer: _config["Jwt:Issuer"],
			   audience: _config["Jwt:Audience"],
			   expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:RefreshTokenExpirationMinutes"])),
			   signingCredentials: credentials
			   );

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public bool ValidateToken(string refreshToken)
		{
			TokenValidationParameters validationParameters = new TokenValidationParameters()
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = true,
				ValidIssuer = _config["Jwt:Issuer"],
				ValidAudience = _config["Jwt:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:RefreshTokenSecret"]!)),
				ClockSkew = TimeSpan.Zero
			};
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public Task Create(RefreshToken refreshToken)
		{
			refreshToken.Id = Guid.NewGuid();
			_context.RefreshTokens.Add(refreshToken);
			_context.SaveChanges();
			return Task.CompletedTask;
		}

		public Task<RefreshToken> GetByToken(string token)
		{
			var result =  _context.RefreshTokens.Where(x => x.Token == token).FirstOrDefault();
			return Task.FromResult(result);
		}

		public async Task<LoginResponse> GenerateAndReturnToken(string userName)
		{
			var getUser = _userManager.Users.FirstOrDefault(u => u.UserName == userName);
			var getUserRole = await _userManager.GetRolesAsync(getUser);

			var userSession = new UserSession()
			{
				UserId = getUser.Id,
				FirstName = getUser.FirstName ?? string.Empty,
				LastName = getUser.LastName ?? string.Empty,
				Email = getUser.Email ?? string.Empty,
				Role = getUserRole.First() ?? string.Empty,
				UserName = getUser.UserName ?? string.Empty
			};

			string token = GenerateToken(userSession);
			string refreshToken = GenerateRefreshToken();

			RefreshToken refreshTokenDto = new RefreshToken()
			{
				Token = refreshToken,
				UserId = getUser.Id,
			};

			await Create(refreshTokenDto);

			return new LoginResponse() { flag = true, token = token!, refreshToken = refreshToken, message = "Login completed" };
		}

		public Task Delete(Guid id)
		{
			var refreshToken = _context.RefreshTokens.Where(x => x.Id == id).ToList();
			_context.RefreshTokens.RemoveRange(refreshToken);
			_context.SaveChanges();
			return Task.CompletedTask;
		}
	}
}
