using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Stossion.Helpers.RestHelpers
{
    public  class RestAPI()
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

        public static async Task<ApiResponse> Post<X>(ApiPostRequest<X> request)
        {
            try
            {
                var url = string.Empty;
                var result = new ApiResponse();

                if (String.IsNullOrWhiteSpace(request.MethodName))
                {
                    url = request.Controller;
                }
                else
                {
                    url = request.host + request.Controller + (String.IsNullOrEmpty(request.MethodName) ? string.Empty : "/" + request.MethodName);
                }


                using var handler = new HttpClientHandler();
                {
                    using (var client = new HttpClient(handler))
                    {
                        foreach (var item in request.headers)
                        {
                            foreach (var pair in item)
                            {
								client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(pair.Key, pair.Value);

							}
						}

						HttpResponseMessage response = await client.PostAsJsonAsync(url, request.data);
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
