using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Stossion.Helpers.RestHelpers
{
    public static class RestAPI
    {
        public static async Task<ApiResponse> Get(string Controller, string? MethodName = null, string? host = null, string? paramName = null, string? param = null)
        {
            try
            {
                var header = string.Empty;
				var result = new ApiResponse();

				if (String.IsNullOrEmpty(MethodName))
                {
                    header = Controller;
                }
                else
                {
                    header = host + Controller + (String.IsNullOrEmpty(MethodName) ? string.Empty : "/" + MethodName);
                    if (!String.IsNullOrEmpty(param))
                    {
                        header = header + "?" + paramName + "=" + param;
                    }
                }
                var url = new Uri(header);

                using var handler = new HttpClientHandler();
                {
                    using (var client = new HttpClient(handler))
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
							result.IsSuccess = response.IsSuccessStatusCode;
							result.StatusCode = response.StatusCode;
							result.result = await response.Content.ReadAsStringAsync();							
                        }
                        else
                        {
							result.IsSuccess = response.IsSuccessStatusCode;
							result.StatusCode = response.StatusCode;
						}
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
				return new ApiResponse()
				{
					IsSuccess = false,
					StatusCode = HttpStatusCode.ExpectationFailed
				};
			}
        }

        public static async Task<ApiResponse> Post<X>(string Controller, string MethodName, X data, string? host = null)
        {
            try
            {
                var url = string.Empty;
                var result = new ApiResponse();

                if (String.IsNullOrWhiteSpace(MethodName))
                {
                    url = Controller;
                }
                else
                {
                    url = host + Controller + (String.IsNullOrEmpty(MethodName) ? string.Empty : "/" + MethodName);
                }

                using var handler = new HttpClientHandler();
                {
                    using (var client = new HttpClient(handler))
                    {
                        HttpResponseMessage response = await client.PostAsJsonAsync(url, data);
                        if (response.IsSuccessStatusCode)
                        {
                            result.IsSuccess = response.IsSuccessStatusCode;
                            result.StatusCode = response.StatusCode;
                            result.result = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            result.IsSuccess = response.IsSuccessStatusCode;
                            result.StatusCode = response.StatusCode;
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.ExpectationFailed
                };
            }
        }
    }
}
