using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.Helpers.RestHelpers
{
	public class ApiPostRequest<X>
	{
		public string? Controller {  get; set; }
		public string? MethodName {  get; set; } 
		public X? data { get; set; }
		public string? host {  get; set; }
		public List<Dictionary<string, string>>? headers { get; set; }
	}

	public class ApiGetRequest
	{
		public string? Controller { get; set; }
		public string? MethodName { get; set; }
		public string? param { get; set; }
		public string? value { get; set; }
		public string? host { get; set; }
		public List<Dictionary<string, string>>? headers { get; set; }
	}
}
