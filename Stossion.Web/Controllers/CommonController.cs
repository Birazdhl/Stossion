using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stossion.Domain;
using Stossion.Helpers.RestHelpers;
using Stossion.ViewModels.Country;

namespace Stossion.Web.Controllers
{
	public class CommonController(IConfiguration configuration, IHttpContextAccessor contextAccessor) : BaseController(configuration, contextAccessor)
	{
		[AllowAnonymous]
		public async Task<List<CountryViewModel>> GetCountryList()
		{
			ApiGetRequest request = new ApiGetRequest()
			{
				Controller = "Country",
				MethodName = "GetCountriesList"
			};
			var response = await StossionGet(request);

			if (response.IsSuccess)
			{
				var result =  JsonConvert.DeserializeObject<List<CountryViewModel>>(response.result) ?? new List<CountryViewModel>();
				result = result.OrderBy(x => x.Name).ToList();
				return result;
			}
			return new List<CountryViewModel>();
		}

        [AllowAnonymous]
        public IActionResult ErrorMessage(string message,string userName)
        {
			if (String.IsNullOrEmpty(userName))
			{
				ViewBag.UserName = userName;
			}
            return View("~/Views/Home/ErrorMessage.cshtml", message);
        }
    }
}
