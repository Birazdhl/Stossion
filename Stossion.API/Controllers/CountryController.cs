using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stossion.BusinessLayers.Interfaces;
using Stossion.Domain;

namespace Stossion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController(ICountryInterface _countryInterface) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("UpdateCountryList")]
        public async Task<string>  UpdateCountryList()
        {
           return await _countryInterface.UpdateCountriesList();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetCountriesList")]
        public async Task<List<Country>> GetCountriesLst()
        {
			return await _countryInterface.GetCountryList();
        }
    }
}
