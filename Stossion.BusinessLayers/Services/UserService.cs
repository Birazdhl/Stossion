using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Stossion.BusinessLayers.Interfaces;
using Stossion.DbManagement.StossionDbManagement;
using Stossion.Domain;
using Stossion.Helpers.RestHelpers;
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
    public class UserService(
        UserManager<StossionUser> _userManager, 
        RoleManager<IdentityRole> _roleManager,
        IHttpContextAccessor _httpContextAccessor,
        StossionDbContext _context,
        IConfiguration _config,
        ITokenInterface _tokenRepository) : IUserInterface
    {
        public async Task<GeneralResponse> CreateUser(RegisterViewModel model)
        {
                if (model is null) return new GeneralResponse() { flag = false, message = "Model is empty" };
                var newUser = new StossionUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Birthday = model.Birthday,
                    GenderId = model.Gender,
                    CountryId = model.Country,
                    Email = model.Email,
                    PasswordHash = model.Password,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                };
                var user = await _userManager.FindByEmailAsync(newUser.Email);
                if (user is not null) return new GeneralResponse() { flag = false, message = "User with this Email Id registered already" };

                var getUserByUsername =  _userManager.Users.FirstOrDefault(u => u.UserName == model.UserName);
                if (getUserByUsername is not null) return new GeneralResponse() { flag = false, message = "User with this Username registered already" };

                var getUserByPhoneNumber =  _userManager.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);
                if (getUserByPhoneNumber is not null) return new GeneralResponse() { flag = false, message = "User with this Phone No. registered already" };

                var createUser = await _userManager.CreateAsync(newUser!, model.Password);
                if (!createUser.Succeeded) return new GeneralResponse() { flag = false, message = createUser.Errors.First().Description };

                //Assign Default Role : Admin to first registrar; rest is user
                var checkAdmin = await _roleManager.FindByNameAsync("Admin");
                if (checkAdmin is null)
                {
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                    await _userManager.AddToRoleAsync(newUser, "Admin");
                    return new GeneralResponse() { flag = true, message = "Account Created" };
                }
                else
                {
                    var checkUser = await _roleManager.FindByNameAsync("User");
                    if (checkUser is null)
                        await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                    await _userManager.AddToRoleAsync(newUser, "User");
                    return new GeneralResponse() { flag = true, message = "Account Created" };
                }
        }

        public async Task<LoginResponse> LoginUser(LoginViewModel model)
        {
            if (model == null)
                return new LoginResponse() { flag = false, token = null!, message = "Login container is empty" };

            var getUser =  _userManager.Users.FirstOrDefault(u => u.UserName == model.Username);
            if (getUser is null)
                return new LoginResponse() { flag = false, token = null!, message = "User not found" };

            bool checkUserPasswords = await _userManager.CheckPasswordAsync(getUser, model.Password);
            if (!checkUserPasswords)
                return new LoginResponse() { flag = false, token = null!, message = "Invalid username/password" };

            return await _tokenRepository.GenerateAndReturnToken(model.Username);

        }
		
        public async Task<LoginResponse> Refresh(RefreshTokenViewModel requestToken)
        {
            bool isValidateToken = _tokenRepository.ValidateToken(requestToken.RefreshToken);
            if (!isValidateToken)
            {
				return new LoginResponse() { flag = true, token = null, refreshToken = null, message = "Invalid Token" };
			}
            var refreshToken = await _tokenRepository.GetByToken(requestToken.RefreshToken);
            if (refreshToken == null)
            {
				return new LoginResponse() { flag = true, token = null, refreshToken = null, message = "Token Not Found" };
			}

            await _tokenRepository.Delete(refreshToken.Id);

           var getUser = _userManager.Users.FirstOrDefault(u => u.Id == refreshToken.UserId);
            if (getUser == null)
            {
				return new LoginResponse() { flag = true, token = null, refreshToken = null, message = "Token Not Found" };
			}

			return await _tokenRepository.GenerateAndReturnToken(getUser.UserName ?? string.Empty);

		}

		public StossionUser? GetUserDetails()
		{
            var result = new StossionUser();
			ClaimsPrincipal? currentUser = _httpContextAccessor.HttpContext?.User;
			if (currentUser?.Identity?.IsAuthenticated == true)
			{
                var username = currentUser?.Identity.Name;
				var getUser = _userManager.Users.FirstOrDefault(u => u.UserName == username);
			}
			return result;
		}
		public Task Logout(Guid id)
		{
			_tokenRepository.Delete(id);
            return Task.CompletedTask;
		}
	}
}
