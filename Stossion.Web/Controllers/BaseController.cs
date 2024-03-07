using Microsoft.AspNetCore.Mvc;
using Stossion.Helpers.RestHelpers;
using Stossion.ViewModels.User;

namespace Stossion.Web.Controllers
{
	public class BaseController(IConfiguration configuration) : Controller
	{
		public async Task<T> StossionPost<T,X>(string controller, string action, X model)
		{
			var hostUri = configuration.GetValue<string>("StossionAPI");
			var result = await RestAPI.Post<T, X>("User", "Login", model, hostUri);
			return (T)result;
		}

		public async Task<IActionResult> StossionGet(LoginViewModel model)
		{
			var result = await RestAPI.Post<IActionResult, LoginViewModel>("User", "Login", model);
			return View(result);
		}
	}
}
