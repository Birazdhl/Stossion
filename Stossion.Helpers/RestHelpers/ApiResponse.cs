using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.Helpers.RestHelpers
{
	public class ApiResponse
	{
        public bool IsSuccess { get; set; }
        public HttpResponseMessage Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string result { get; set; }
    }
}
