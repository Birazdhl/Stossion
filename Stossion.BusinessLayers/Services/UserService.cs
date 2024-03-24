﻿using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Stossion.BusinessLayers.Interfaces;
using Stossion.DbManagement.StossionDbManagement;
using Stossion.Domain;
using Stossion.Helpers.Enum;
using Stossion.Helpers.RestHelpers;
using Stossion.ViewModels.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
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
        ICountryInterface _countryInterface,
        IEmailSenderService emailSenderService,
        IDapperInterface dapperInterface,
        ISharedService sharedService) : IUserInterface
    {
        public async Task<LoginResponse> CreateUser(RegisterViewModel model)
        {
            try
            {
                if (model is null) return new LoginResponse() { flag = false, message = StossionConstants.emptyModel };
                int countryId = 0;
                if (!string.IsNullOrEmpty(model.Country))
                {
                    countryId = _countryInterface.GetCountryBySymbol(model.Country).Id;
                }
                var verificatonToken = CreateRandomToken();
             
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
                    EmailVerificationToken = verificatonToken
                };

                if (string.IsNullOrEmpty(model.FirstName) ||
                    string.IsNullOrEmpty(model.Birthday.ToString()) ||
                    (model.Gender == 0) ||
                    string.IsNullOrEmpty(model.Country) ||
                    string.IsNullOrEmpty(model.Email) ||
                    string.IsNullOrEmpty(model.Password) ||
                    string.IsNullOrEmpty(model.UserName))
                {
                    return new LoginResponse() { flag = false, token = null!, message = "Inavalid! Model State" };
                }

                var user = await _userManager.FindByEmailAsync(newUser.Email);
                if (user is not null && user.EmailConfirmed) return new LoginResponse() { flag = false, token = null!, message = "Email Id already registered" };

                var getUserByUsername = _userManager.Users.FirstOrDefault(u => u.UserName == model.UserName);
                if (getUserByUsername is not null) return new LoginResponse() { flag = false, message = "User with this Username registered already" };

                if (!string.IsNullOrEmpty(model.PhoneNumber))
                {
                    var getUserByPhoneNumber = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);
                    if (getUserByPhoneNumber is not null) return new LoginResponse() { flag = false, message = "User with this Phone No. registered already" };
                }

                var createUser = await _userManager.CreateAsync(newUser!, model.Password);
                if (!createUser.Succeeded) return new LoginResponse() { flag = false, message = createUser.Errors.First().Description };

                //Assign Default Role : Admin to first registrar; rest is user
                var checkAdmin = await _roleManager.FindByNameAsync("Admin");
                await SendVerifiationEmail(model.Email, verificatonToken);
                if (checkAdmin is null)
                {
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                    await _userManager.AddToRoleAsync(newUser, "Admin");
                }
                else
                {
                    var checkUser = await _roleManager.FindByNameAsync("User");
                    if (checkUser is null)
                        await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                    await _userManager.AddToRoleAsync(newUser, "User");
                }
                //return await _tokenRepository.GenerateAndReturnToken(model.UserName);
                return new LoginResponse() { flag = false, message = StossionConstants.success };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<LoginResponse> LoginUser(LoginViewModel model)
        {

            if (model == null)
                return new LoginResponse() { flag = false, token = null!, message = StossionConstants.emptyModel };

            var getUser =  _userManager.Users.FirstOrDefault(u => u.UserName == model.Username);
            if (getUser is null)
                return new LoginResponse() { flag = false, token = null!, message = "User not found" };

			bool checkUserPasswords = await _userManager.CheckPasswordAsync(getUser, model.Password);
            if (!checkUserPasswords)
                return new LoginResponse() { flag = false, token = null!, message = "Invalid username/password" };

            if (getUser.EmailConfirmed == false)
            {
				return new LoginResponse() { flag = false, token = null!, message = StossionConstants.unverifiedEmail };
			}

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

            if (!getUser.EmailConfirmed)
            {
                return new LoginResponse() { flag = false, token = null!, message = "Please Verify Email first to continue!" };
            }

			return await _tokenRepository.GenerateAndReturnToken(email,true);

		}

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        private async Task SendVerifiationEmail(string email,string token)
        {
            string emailMessage = _context.Templates.Where(x => x.Name == StossionConstants.VerifyEmail).FirstOrDefault()?.Value ?? string.Empty;
            var link = _config["JWT:Audience"] + "/Login/VerifyEmail?token=" + token;
            emailMessage = emailMessage.Replace("@verificationLink", link);

		    await emailSenderService.SendEmailAsync(email, StossionConstants.emailVerificationLink, emailMessage);

		}

        private async Task SendEmailChangeConfirmation(string email, string token)
        {
            string emailMessage = _context.Templates.Where(x => x.Name == StossionConstants.ChangeEmail).FirstOrDefault()?.Value ?? string.Empty;
            var link = _config["JWT:Audience"] + "/Login/ChangeEmail?token=" + token;
            emailMessage = emailMessage.Replace("@verificationLink", link);

            await emailSenderService.SendEmailAsync(email, StossionConstants.emailVerificationLink, emailMessage);

        }

        public async Task<LoginResponse> VerifyEmail(string token)
        {
            try
            {
				var getUser = _userManager.Users.FirstOrDefault(u => u.EmailVerificationToken == token);
				if (getUser is null)
				{
                    return new LoginResponse() { flag = false, token = null!, message = StossionConstants.invalidParameter };
				}

                getUser.EmailVerificationToken = string.Empty;
				getUser.VerifyAt = DateTime.Now;
				getUser.EmailConfirmed = true;
                
                _context.Users.Update(getUser);
				await _context.SaveChangesAsync();

                var login = await _tokenRepository.GenerateAndReturnToken(getUser.Email ?? string.Empty, true);
                login.message = StossionConstants.success;
                return login;
			}
            catch (Exception)
            {
                return new LoginResponse() { flag = false, token = null!, message = StossionConstants.internalServerError };
            }
			
		}

        public async Task<LoginResponse> ChangeEmail(string token)
        {
            try
            {
                var getUser = _userManager.Users.FirstOrDefault(u => u.EmailChangeConfirmationToken == token);
                if (getUser is null)
                {
                    return new LoginResponse() { flag = false, token = null!, message = StossionConstants.invalidParameter };
                }

                getUser.EmailChangeConfirmationToken = string.Empty;
                getUser.VerifyAt = DateTime.Now;
                getUser.EmailConfirmed = true;
                getUser.Email = getUser.ChangingEmail;
                getUser.ChangingEmail = string.Empty;

                _context.Users.Update(getUser);
                await _context.SaveChangesAsync();

                var login = await _tokenRepository.GenerateAndReturnToken(getUser.Email ?? string.Empty, true);
                login.message = StossionConstants.success;
                return login;
            }
            catch (Exception)
            {
                return new LoginResponse() { flag = false, token = null!, message = StossionConstants.internalServerError };
            }

        }

        public async Task<string> ChangePassword(ChangePasswordViewModel model, string? userName)
        {
            try
            {
                if (String.IsNullOrEmpty(model.NewPassword)
                || String.IsNullOrEmpty(model.OldPassword) ||
                    String.IsNullOrEmpty(model.ConfirmNewPassowrd) ||
                    String.IsNullOrEmpty(userName))
                {
                    return "Invalid Model";
                }

                if (model.NewPassword != model.ConfirmNewPassowrd)
                {
                    return ("Password Doesnt Match");
                }

                if (model.NewPassword == model.OldPassword)
                {
                    return ("Same Password");
                }

                var getUser = _userManager.Users.FirstOrDefault(u => u.UserName == userName);

                if (getUser is null)
                    return ("User Not Found");

                var checkPassword = await _userManager.CheckPasswordAsync(getUser, model.OldPassword);
                if (!checkPassword)
                {
                    return "Invalid Password";
                }

                var changeUserPassword = await _userManager.ChangePasswordAsync(getUser, model.OldPassword, model.NewPassword);
                if (changeUserPassword.Succeeded)
                {

                    getUser.CreatedAt = DateTime.UtcNow;
                    getUser.ModifiedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return StossionConstants.success;
                }
                else
                {
                    foreach (var item in changeUserPassword.Errors)
                    {
                        _context.ErrorLogs.Add(new ErrorLog
                        {
                            Message = item.Description,
                            StackTrace = item.Code,
                            DateTime = DateTime.Now,
                            Username = getUser.UserName ?? string.Empty,
                        });
                    }
                    return "Failed to change Password";
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<string> ForgetPasswordVerificationLink(string username)
        {
            try
            {
                if (String.IsNullOrEmpty(username))
                {
                    return "Username Empty";
                }
                var getUser = _userManager.Users.FirstOrDefault(u => u.UserName == username);
                if (getUser == null || !(await _userManager.IsEmailConfirmedAsync(getUser)))
                {
                    return "Username/Email not registered";
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(getUser);

                string emailMessage = _context.Templates.Where(x => x.Name == StossionConstants.ResetPassword).FirstOrDefault()?.Value ?? string.Empty;
                var link = _config["JWT:Audience"] + "/Login/ResetPassword?token=" + token + "&username=" + username;
                emailMessage = emailMessage.Replace("@forgetPasswordLink", link);

                if (string.IsNullOrEmpty(getUser.Email)) { return "No Email Found"; };

                await emailSenderService.SendEmailAsync(getUser.Email, StossionConstants.forgetPasswordLink, emailMessage);

                return StossionConstants.success;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> ResetPassword(ForgetPasswordViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return "Password Dosen't Match";
            }
            var getUser = _userManager.Users.FirstOrDefault(u => u.UserName == model.Username);
            if (getUser == null)
            {
                // Don't reveal that the user does not exist
                return StossionConstants.noContent;
            }

            if (getUser.EmailConfirmed == false)
            {
                return "Email Not Verified";
            }

            var result = await _userManager.ResetPasswordAsync(getUser, model.Token??string.Empty, model.Password ?? string.Empty);
            if (result.Succeeded)
            {
                return StossionConstants.success;
            }
            else
            {
                // Handle password reset failure
                return "Reset Password Failed!!! The reset link may be invalid or expired";
            }
        }

        public async Task<string> GetProfileImage(string username)
        {
            // Specify the path where the image will be saved
            string imagePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", "user",username + ".png");

            if (!File.Exists(imagePath))
            {
                imagePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", "user", "noimage.png");
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imageBytes);

            // Return the Base64 string as JSON or whatever format you prefer
            return base64String;

        }

        public async Task<UserDetailsViewModel> GetUserDetails(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                return null;
            }
            StringBuilder query = new StringBuilder();
            query.AppendLine("select FirstName , LastName, c.Name as 'Country',c.Logo as 'Logo',g.Type as 'Gender'," +
                "u.Email, u.PhoneNumber from AspNetUsers u " +

                "Inner Join Country c on u.CountryId = c.Id " +
                "Inner Join Gender g on u.GenderId = g.Id " +
                "where u.UserName = @username");
            var result = await dapperInterface.QuerySingleAsync<UserDetailsViewModel>(query.ToString(), new { username });
            if (result != null)
            {
                result.ProfilePicture = await GetProfileImage(username);
            }
            return result;
        }

        public async Task<string> UpdateUserProfile(string username, UpdateUserProfileViewModel model)
        {
            if (String.IsNullOrEmpty(username))
            {
                return StossionConstants.invalidUsername;
            }

            var getUser = _userManager.Users.FirstOrDefault(u => u.UserName == username);

            if (getUser == null) {
                return StossionConstants.invalidUsername;
            }

            var currentEmail = getUser.Email;

            var verificatonToken = CreateRandomToken();

            if (model.Key.ToLower() == "email")
            {
                if (model.Password != model.ConfirmPassword)
                {
                    return "Password Dosen't Match";
                }

                if (String.IsNullOrEmpty(model.Password))
                {
                    return "Password Is Required";
                }

                bool checkUserPasswords = await _userManager.CheckPasswordAsync(getUser, model.Password);
                if (!checkUserPasswords)
                    return "Incorrect Password";

                if (String.IsNullOrEmpty(model.Value))
                {
                    return "Email is Required";
                }
                if (currentEmail == model.Value)
                {
                    return StossionConstants.success;
                }
                var isValidEmail = sharedService.CheckValidEmail(model.Value);
                if (!isValidEmail)
                {
                    return StossionConstants.invalidEmail;
                }

                var getUserByEmail = _userManager.Users.FirstOrDefault(u => u.Email == model.Value);
                if (getUserByEmail != null && getUserByEmail.EmailConfirmed) {
                    return "Email already registered";
                }


                getUser.ChangingEmail = model.Value;
                getUser.EmailChangeConfirmationToken = verificatonToken;


            }
            else if (model.Key.ToLower() == "name")
            {
                string[] values = model.Value.Split(',');

                string firstValue = values[0].Trim() ?? string.Empty;
                string secondValue = values[1].Trim() ?? string.Empty;

                if (String.IsNullOrEmpty(firstValue) || String.IsNullOrEmpty(secondValue))
                {
                    return "Both FirstName and LastName are required";
                }

                getUser.FirstName = firstValue;
                getUser.LastName = secondValue;
            }
            else if (model.Key.ToLower() == "country")
            {
                var country = await _context.Country.Where(x => x.Symbol == model.Value).FirstOrDefaultAsync();
                if (country == null)
                {
                    return "Country Not Found";
                }
                getUser.CountryId = country.Id;
            }
            else if (model.Key.ToLower() == "gender")
            {
                var id = Convert.ToInt32(model.Value);
                if (id < 0 && id > 3)
                {
                    return "Invalid Gender Value";
                }
                getUser.GenderId = id;
            }
            else if (model.Key.ToLower() == "phonenumber")
            {
                getUser.PhoneNumber = model.Value;
            }
            else if (model.Key.ToLower() == "profilepicture")
            {
                getUser.ProfilePicture = model.Value;
            }

            _context.Users.Update(getUser);
            await _context.SaveChangesAsync();

            if (model.Key == "email")
            {
                await SendEmailChangeConfirmation(model.Value, verificatonToken);
            }

            return StossionConstants.success;
        }
    

    }

}
