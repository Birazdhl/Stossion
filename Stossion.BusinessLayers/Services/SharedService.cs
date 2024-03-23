using Stossion.BusinessLayers.Interfaces;
using Stossion.DbManagement.StossionDbManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stossion.BusinessLayers.Services
{
    public class SharedService(StossionDbContext _context) : ISharedService
    {
        public string GetTemplates(string name)
        {
            var value = _context.Templates.Where(x => x.Name == name).FirstOrDefault();
            return value?.Value ??  string.Empty;
        }
        public bool CheckValidEmail(string email)
        {
            string pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";

            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern);

            // Use the Match method to check if the email matches the pattern
            return regex.IsMatch(email);
        }
    }
}
