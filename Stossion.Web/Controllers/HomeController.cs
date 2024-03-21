using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stossion.Helpers.RestHelpers;
using Stossion.ViewModels.User;
using Stossion.Web.Authorization;
using Stossion.Web.Models;
using System.Diagnostics;

namespace Stossion.Web.Controllers
{
    public class HomeController(IConfiguration configuration,IHttpContextAccessor contextAccessor) : BaseController(configuration, contextAccessor)
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Menu(string name)
        {
            return View("Menu",name);
        }

        [Authorize]
        public async Task<IActionResult> ViewProfile()
        {
            ApiGetRequest request = new ApiGetRequest()
            {
                Controller = "User",
                MethodName = "GetUserDetails"
            };
            var response = await StossionGet(request);
            if (response.IsSuccess) 
            {
                var result = JsonConvert.DeserializeObject<UserDetailsViewModel>(response.result);
                if (result != null)
                {
                    return View("ViewProfile", result);
                }
            }
            return NoContent();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            return View(message);
        }

    }
}
