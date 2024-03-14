using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Stossion.Helpers.RestHelpers;
using Stossion.ViewModels.User;
using Stossion.Web.Models;
using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

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
			var authenticationResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			//if (authenticationResult.Succeeded)
			//{
			//	return RedirectToAction("Index", "Home");
			//}
			if (User.Identity != null && User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
			// Call external API to get token, flag, and claims based on username and password
			var response = await StossionPost("User", "Login", model);

            var result = JsonConvert.DeserializeObject<LoginResponse>(response.result);

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
			if (result != null) {
				if (result.flag)
				{
					// Store the token securely (e.g., in a secure cookie or session)
					HttpContext.Response.Cookies.Append("AuthorizationToken", result.token ?? string.Empty);
				}
			}
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
	}
}
