using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stossion.BusinessLayers.Interfaces;
using Stossion.DbManagement.StossionDbManagement;
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
        IConfiguration _config) : IUserInterface
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
                if (user is not null) return new GeneralResponse() { flag = false, message = "User with this Username registered already" };

                var getUserByPhoneNumber =  _userManager.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);
                if (user is not null) return new GeneralResponse() { flag = false, message = "User with this Phone No. registered already" };

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
                return new LoginResponse() { flag = false, token = null!, message = "Invalid email/password" };

            var getUserRole = await _userManager.GetRolesAsync(getUser);
            
            var userSession = new UserSession() { 
                UserId = getUser.Id, 
                FirstName = getUser.FirstName ?? string.Empty,
                LastName = getUser.LastName ?? string.Empty,
                Email = getUser.Email ?? string.Empty,
                Role = getUserRole.First() ?? string.Empty,
                UserName = getUser.UserName ?? string.Empty
            };
            
            string token = GenerateToken(userSession);
            return new LoginResponse() { flag = false, token = token!, message = "Login completed" };

        }

        private string GenerateToken(UserSession user)
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
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetUserDetails()
        {
            string result = string.Empty;
            ClaimsPrincipal? currentUser = _httpContextAccessor.HttpContext?.User;
            if (currentUser?.Identity?.IsAuthenticated == true)
            {
                Claim? nameClaim = currentUser.FindFirst(ClaimTypes.Name) ?? null;
                result = nameClaim?.Value ?? result;
            }
            return result;
        }
    }
}
