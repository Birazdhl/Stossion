using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.ViewModels.User
{
    public class LoginResponse
    {
        public bool flag { get; set; }
        public string token { get; set; }
        public string message { get; set; }
    }
}
