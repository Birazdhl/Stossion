using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stossion.BusinessLayers.Interfaces;

namespace Stossion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController(ICountryInterface _countryInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<string>  UpdateCountryList()
        {
           return await _countryInterface.UpdateCountriesList();
        }
    }
}
