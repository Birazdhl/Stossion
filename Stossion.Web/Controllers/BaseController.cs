using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Stossion.Helpers.RestHelpers;
using Stossion.ViewModels.User;
using System;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace Stossion.Web.Controllers
{
	[Authorize]
	public class BaseController(IConfiguration configuration,IHttpContextAccessor _httpContextAccessor) : Controller
	{
		private readonly string hostUri = configuration.GetValue<string>("StossionAPI");
		public async Task<ApiResponse> StossionPost<X>(string controller, string action, X? model, List<Dictionary<string,string>>? headers = null)
		{
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

		public async Task<ApiResponse> StossionGet(ApiGetRequest request)
		{
			string? auth = _httpContextAccessor?.HttpContext?.Request.Cookies["AuthorizationToken"];
			request.host = hostUri;

			

			if (request.headers?.Count() > 0)
			{
				foreach (var item in request.headers)
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
			else
			{
                if (!string.IsNullOrEmpty(auth))
                {
                    Dictionary<string, string> bearer = new Dictionary<string, string>
					{
					    { "Bearer", auth }
					};
                   
                        request.headers = [bearer];
                }
            }

			var result = await RestAPI.Get(request);
			return result;
		}
	}
}
