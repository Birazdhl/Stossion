using Microsoft.AspNetCore.Mvc;
using Stossion.Helpers.RestHelpers;
using Stossion.ViewModels.User;
using System;

namespace Stossion.Web.Controllers
{
	public class BaseController(IConfiguration configuration) : Controller
	{
		public async Task<ApiResponse> StossionPost<X>(string controller, string action, X model)
		{
			var hostUri = configuration.GetValue<string>("StossionAPI");
			var result = await RestAPI.Post<X>(controller, action, model, hostUri);
			return result;
		}

		public async Task<ApiResponse> StossionGet(string controller, string action)
		{
			var hostUri = configuration.GetValue<string>("StossionAPI");
			var result = await RestAPI.Get(controller, action, hostUri);
			return result;
		}
	}
}
