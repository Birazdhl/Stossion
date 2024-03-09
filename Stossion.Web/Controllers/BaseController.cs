using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Stossion.Helpers.RestHelpers;
using Stossion.ViewModels.User;
using System;

namespace Stossion.Web.Controllers
{
	[Authorize]
	public class BaseController(IConfiguration configuration,IHttpContextAccessor _httpContextAccessor) : Controller
	{
		public async Task<ApiResponse> StossionPost<X>(string controller, string action, X? model, List<Dictionary<string,string>>? headers = null)
		{
			var hostUri = configuration.GetValue<string>("StossionAPI");
			ApiPostRequest<X> request = new ApiPostRequest<X>()
			{
				Controller = controller,
				MethodName = action,
				host = hostUri,
				data = model,
				headers = new List<Dictionary<string, string>>()
			};

			string? auth = _httpContextAccessor?.HttpContext?.Request.Cookies["AuthorizationToken"];

			if (!string.IsNullOrEmpty(auth))
			{
				Dictionary<string, string> bearer = new Dictionary<string, string>
				{
					{ "Bearer", auth }
				};
				request.headers.Add(bearer);
			}

			if (headers?.Count() > 0)
			{
				foreach (var item in headers)
				{
					foreach (var pair in item)
					{
						Dictionary<string, string> externalHeaders = new Dictionary<string, string>
						{
							{ "Bearer", _httpContextAccessor?.HttpContext?.Request.Cookies["AuthorizationToken"] ?? string.Empty }
						};
						request.headers.Add(externalHeaders);
					}
				}

			}

			var result = await RestAPI.Post(request);
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
