using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Stossion.Helpers.RestHelpers;
using Stossion.ViewModels.User;
using Stossion.Web.Models;
using System.Diagnostics;

namespace Stossion.Web.Controllers
{
    public class LoginController(IConfiguration configuration) : BaseController(configuration)
    {
        
        public IActionResult Index()
        {
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


			return RedirectToAction("Index");
		}
    }
}
