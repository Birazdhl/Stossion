using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stossion.BusinessLayers.Interfaces;
using Stossion.ViewModels.User;

namespace Stossion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserInterface userInterface) : ControllerBase
    {
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser(RegisterViewModel model)
        {
                var response = await userInterface.CreateUser(model);
                return Ok(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser(LoginViewModel model)
        {
            var response = await userInterface.LoginUser(model);
            return Ok(response);
        }
    }
}
