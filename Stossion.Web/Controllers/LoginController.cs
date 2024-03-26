using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stossion.ViewModels.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Stossion.Helpers.Enum;
using System.Reflection;
using Stossion.Helpers.RestHelpers;
using Stossion.Web.Authorization;
using System.Web;

namespace Stossion.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController(IConfiguration configuration, IHttpContextAccessor contextAccessor) : BaseController(configuration, contextAccessor)
    {
        
        public async Task<IActionResult> Index()
       {
			if (!String.IsNullOrEmpty(TempData["EmailNotregistered"]?.ToString()) && TempData["EmailNotregistered"]?.ToString() == "Invalid")
			{
				ViewBag.EmailNotregistered = "Invalid";
				return View();
			}
			
			//var authenticationResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		
			if (User.Identity != null && User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
        }

	
		[HttpGet]
		public IActionResult ForgetPassword()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
			// Call external API to get token, flag, and claims based on username and password
			var response = await StossionPost("User", "Login", model);

            var result = JsonConvert.DeserializeObject<LoginResponse>(response.result);

            if (result?.message == StossionConstants.unverifiedEmail)
            {
				ViewBag.UserName = model.Username;
                return RedirectToAction("ErrorMessage", "Common", new { message = "Please Verify Email first to continue" });
            }
            if (result.flag)
            {
                // Store the token securely (e.g., in a secure cookie or session)
                HttpContext.Response.Cookies.Append("AuthorizationToken", result.token);

                return RedirectToAction("Index","Home");
			}


			return NoContent();
		}

        public async Task<IActionResult> Logout()
        {
            var response = await StossionPost("User", "Logout",string.Empty);

            if (response.result?.ToLower() == "logged out")
            {
				foreach (var cookie in Request.Cookies.Keys)
				{
					Response.Cookies.Delete(cookie);
				}
				return RedirectToAction("Index", "Login");
			}
			return NoContent();

		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			var response = await StossionPost("User", "Register", model);
			var result = JsonConvert.DeserializeObject<LoginResponse>(response.result);
			return Ok(result);
		}

		public IActionResult GoogleSignIn()
		{
			var header = configuration.GetValue<string>("Jwt:Audience");
			var services = Url.Action("GoogleSignInCallback");
			var url = header + services;
			var properties = new AuthenticationProperties
			{
				RedirectUri = url
			};

			return Challenge(properties, "Google");
		}

		public async Task<IActionResult> GoogleSignInCallback()
		{
			var authenticationResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			if (!authenticationResult.Succeeded)
			{
				// Handle failed authentication
				return RedirectToAction("Index");
			}

			// Retrieve user's Gmail name
			var gmailName = authenticationResult.Principal.FindFirst(ClaimTypes.Email)?.Value;

			var birthday = authenticationResult.Principal.FindFirst(ClaimTypes.DateOfBirth)?.Value;

			// Call external API to get token, flag, and claims based on username and password
			var response = await StossionPost("User", "SingInEmail", gmailName);

			var result = JsonConvert.DeserializeObject<LoginResponse>(response.result);

			if (result != null && result.flag)
			{
				if (!String.IsNullOrEmpty(result.token))
				{
					HttpContext.Response.Cookies.Append("AuthorizationToken", result.token);
				}
				// Store the token securely (e.g., in a secure cookie or session)

			}
			else
			{
				foreach (var cookie in Request.Cookies.Keys)
				{
                    Response.Cookies.Delete(cookie);
				}
				TempData["EmailNotregistered"] = "Invalid";
			}

			return RedirectToAction("Index");
		}


		public async Task<IActionResult> VerifyEmail(string token)
		{
			var response = await StossionPost("User", "VerifyEmail", token);
			if (response.IsSuccess)
			{
				
                var result = JsonConvert.DeserializeObject<LoginResponse>(response.result);
				if (result != null && (!String.IsNullOrEmpty(result.message) && !String.IsNullOrEmpty(result.token)))
                {
                    if (result.message == StossionConstants.invalidParameter)
                    {
                        return RedirectToAction("ErrorMessage", "Common", new { message = "Invalid Email Verification Token" });
                    }
                    HttpContext.Response.Cookies.Append("AuthorizationToken", result.token);
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangeEmail(string token)
        {
            var response = await StossionPost("User", "ChangeEmail", token);
            if (response.IsSuccess)
            {

                var result = JsonConvert.DeserializeObject<LoginResponse>(response.result);
                if (result != null && (!String.IsNullOrEmpty(result.message) && !String.IsNullOrEmpty(result.token)))
                {
                    if (result.message == StossionConstants.invalidParameter)
                    {
                        return RedirectToAction("ErrorMessage", "Common", new { message = "Invalid Email Verification Token" });
                    }
                    HttpContext.Response.Cookies.Append("AuthorizationToken", result.token);
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
		public async Task<IActionResult> EmailVerificationLink(string username)
		{
			var response = await StossionPost("User", "ForgetPasswordVerificationLink", username);
			var message = response.result.ToString();
			return Ok(message);
        }

		[HttpGet]
		public IActionResult ResetPassword(string token, string username)
		{
			ForgetPasswordViewModel model = new ForgetPasswordViewModel()
			{
				Username = username,
				Token = token
			};

            return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ForgetPasswordViewModel model)
		{
            var response = await StossionPost("User", "ResetPassword", model);
			return Ok("Success");
        }

        [Authorize]
        [HttpGet]
		public async Task<string> GetProfilePicture()
		{
			ApiGetRequest request = new ApiGetRequest()
			{
				Controller = "User",
				MethodName = "GetProfilePicture"
			};
            var response = await StossionGet(request);
			return response.result.ToString();
        }

		[Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var response = await StossionPost("User", "ChangePassword", model);
            return Ok(response.result);
        }
    }
}
