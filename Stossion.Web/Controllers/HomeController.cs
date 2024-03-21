using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            return View(message);
        }

    }
}
