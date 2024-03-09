using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Stossion.Helpers.RestHelpers;
using Stossion.ViewModels.User;
using Stossion.Web.Models;
using System.Diagnostics;
using System.Reflection;

namespace Stossion.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController(IConfiguration configuration, IHttpContextAccessor contextAccessor) : BaseController(configuration, contextAccessor)
    {
        
        public IActionResult Index()
        {
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
    }
}
