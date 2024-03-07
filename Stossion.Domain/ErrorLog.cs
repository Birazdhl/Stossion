using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.Domain
{
    public class ErrorLog
    {
        public Guid Id { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public DateTime DateTime { get; set; }
    }
}
