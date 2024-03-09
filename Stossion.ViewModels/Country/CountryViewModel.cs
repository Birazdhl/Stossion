using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.ViewModels.Country
{
    public class CountryModel
    {
        public string cca3 { get; set; }
        public string region { get; set; }
        public Flags flags { get; set; }
        public SubCountryViewModel name { get; set; }
    }

    public class SubCountryViewModel
    {
        public string common { get; set; }
    }

    public class Flags
    {
        public string svg { get; set; }
    }

    public class CountryViewModel
    {
		public string? Name { get; set; }
		public string? Symbol { get; set; }
		public string? Logo { get; set; }
	}
}
