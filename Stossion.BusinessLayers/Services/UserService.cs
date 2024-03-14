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
        ITokenInterface _tokenRepository,
        ICountryInterface _countryInterface) : IUserInterface
    {
        public async Task<LoginResponse> CreateUser(RegisterViewModel model)
        {
                if (model is null) return new LoginResponse() { flag = false, message = "Value is empty" };
                int countryId = 0 ;
                if (!string.IsNullOrEmpty(model.Country))
                {
                    countryId = _countryInterface.GetCountryBySymbol(model.Country).Id;
                }
                var newUser = new StossionUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Birthday = model.Birthday,
                    GenderId = model.Gender,
                    CountryId = countryId,
                    Email = model.Email,
                    PasswordHash = model.Password,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                };

                if (string.IsNullOrEmpty(model.FirstName) ||
					string.IsNullOrEmpty(model.Birthday.ToString()) ||
					(model.Gender == 0) ||
					string.IsNullOrEmpty(model.Country) ||
					string.IsNullOrEmpty(model.Email) ||
					string.IsNullOrEmpty(model.Password) ||
					string.IsNullOrEmpty(model.UserName)) {
                    return new LoginResponse() { flag = false, token = null!, message = "Inavalid! Model State" };
                }
                
                var user = await _userManager.FindByEmailAsync(newUser.Email);
                if (user is not null) return new LoginResponse() { flag = false, token = null!, message = "Email Id already registered" };

			    var getUserByUsername =  _userManager.Users.FirstOrDefault(u => u.UserName == model.UserName);
                if (getUserByUsername is not null) return new LoginResponse() { flag = false, message = "User with this Username registered already" };

                if (!string.IsNullOrEmpty(model.PhoneNumber))
                {
                     var getUserByPhoneNumber =  _userManager.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);
                    if (getUserByPhoneNumber is not null) return new LoginResponse() { flag = false, message = "User with this Phone No. registered already" };
                }
               
                var createUser = await _userManager.CreateAsync(newUser!, model.Password);
                if (!createUser.Succeeded) return new LoginResponse() { flag = false, message = createUser.Errors.First().Description };

                //Assign Default Role : Admin to first registrar; rest is user
                var checkAdmin = await _roleManager.FindByNameAsync("Admin");
                if (checkAdmin is null)
                {
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                    await _userManager.AddToRoleAsync(newUser, "Admin");
                     return await _tokenRepository.GenerateAndReturnToken(model.UserName);
                }
                else
                {
                    var checkUser = await _roleManager.FindByNameAsync("User");
                    if (checkUser is null)
                        await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                    await _userManager.AddToRoleAsync(newUser, "User");
				    return await _tokenRepository.GenerateAndReturnToken(model.UserName);
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

		public async Task<LoginResponse> SingInEmail(string email)
		{
			if (String.IsNullOrEmpty(email))
				return new LoginResponse() { flag = false, token = null!, message = "Email Id is Empty" };

			var getUser = _userManager.Users.FirstOrDefault(u => u.Email == email);
			if (getUser is null)
				return new LoginResponse() { flag = false, token = null!, message = "Email is not registered!" };


			return await _tokenRepository.GenerateAndReturnToken(email,true);

		}
	}
}
