using Stossion.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.BusinessLayers.Interfaces
{
    public interface ICountryInterface 
    {
        Task<string> UpdateCountriesList();
        Task<List<Country>> GetCountryList();
        Country GetCountryById(int id);
		Country GetCountryBySymbol(string id);
	}
}
