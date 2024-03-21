using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Stossion.BusinessLayers.Interfaces;
using Stossion.Domain;
using Stossion.Helpers.Enum;
using Stossion.ViewModels.User;
using System.Security.Claims;

namespace Stossion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserInterface userInterface) : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser(RegisterViewModel model)
        {
                var response = await userInterface.CreateUser(model);
                return Ok(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser(LoginViewModel model)
        {
            var response = await userInterface.LoginUser(model);
            return Ok(response);
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenViewModel request)
        {
            var response = await userInterface.Refresh(request);
            return Ok(response);
        }

		[Authorize]
		[HttpPost]
		[Route("Logout")]
		public async Task<IActionResult> Logout()
		{
            var user = userInterface.GetUserDetails();
            if (user == null)
            {
				return Unauthorized();
			}
            Guid.TryParse(user.Id, out Guid userId);

            await userInterface.Logout(userId);
            return Ok("Logged Out");
		}

		[HttpPost]
		[Route("SingInEmail")]
		public async Task<IActionResult> SingInEmail([FromBody] string email)
		{
			var response = await userInterface.SingInEmail(email);
			return Ok(response);
		}

        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] string token)
        {
            try
            {
               var result =  await userInterface.VerifyEmail(token);
                return Ok(result);
			}
			catch (Exception)
            {
				return Ok(StossionConstants.internalServerError);
			}

		}

        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var result = await userInterface.ChangePassword(model, User?.Identity?.Name);
            return Ok(result);

        }

        [HttpPost]
        [Route("ForgetPasswordVerificationLink")]
        public async Task<IActionResult> ForgetPasswordVerificationLink([FromBody] string username)
        {
            var result = await userInterface.ForgetPasswordVerificationLink(username);
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ForgetPasswordViewModel model)
        {
            var result = await userInterface.ResetPassword(model);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetProfilePicture")]
        public async Task<string> GetProfilePicture()
        {
            var result = await userInterface.GetProfileImage(User?.Identity?.Name ?? string.Empty);
            return result;
        }

        [Authorize]
        [HttpGet("GetUserDetails")]
        public async Task<UserDetailsViewModel> GetUserDetails()
        {
            try
            {
                var result = await userInterface.GetUserDetails(User?.Identity?.Name ?? string.Empty);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}
