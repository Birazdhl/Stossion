using Stossion.BusinessLayers.Interfaces;
using Stossion.DbManagement.StossionDbManagement;
using Stossion.Domain;
using Stossion.Helpers.RestHelpers;
using Stossion.ViewModels.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.BusinessLayers.Services
{
    public class CountryService(StossionDbContext _dbContext) : ICountryInterface
    {
        public async Task<string> UpdateCountriesList()
        {
            var countryList = await RestAPI.Get<List<CountryViewModel>>("https://restcountries.com/v3.1/all");
            if (countryList != null) {
                foreach (var item in countryList)
                {
                    var country = _dbContext.Country.Where(x => x.Name == item.name.common).FirstOrDefault();
                    if (country == null)
                    {
                        Country data = new Country()
                        {
                            Name = item.name.common,
                            Logo = item.flags.svg,
                            Symbol = item.cca3,
                            Region = item.region
                        };

                        _dbContext.Country.Add(data);
                        _dbContext.SaveChanges();
                    }
                }
            }

            return ("Ok");
        }
    }
}
