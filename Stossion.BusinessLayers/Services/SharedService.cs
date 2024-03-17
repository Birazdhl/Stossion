using Stossion.BusinessLayers.Interfaces;
using Stossion.DbManagement.StossionDbManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
