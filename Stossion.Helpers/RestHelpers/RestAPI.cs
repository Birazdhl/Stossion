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
    public class RestAPI()
    {
        //string Controller, string? MethodName = null, string? host = null, string? paramName = null, string? param = null

        public static async Task<ApiResponse> Get(ApiGetRequest request)
        {
            try
            {
                var header = string.Empty;
                var result = new ApiResponse();

                if (String.IsNullOrEmpty(request.MethodName))
                {
                    header = request.Controller;
                }
                else
                {
                    header = request.host + request.Controller + (String.IsNullOrEmpty(request.MethodName) ? string.Empty : "/" + request.MethodName);
                    if (!String.IsNullOrEmpty(request.param))
                    {
                        header = header + "?" + request.param + "=" + request.value;
                    }
                }
                var url = new Uri(header);


                using var handler = new HttpClientHandler();
                {
                    using (var client = new HttpClient(handler))
                    {
                        if (request.headers != null && request?.headers.Count() > 0)
                        {
                            foreach (var item in request.headers)
                            {
                                foreach (var pair in item)
                                {
                                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(pair.Key, pair.Value);

                                }
                            }
                        }


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

                        StringContent? content = null;
                        
                        if (request.data != null) { 
						    content = new StringContent(JsonConvert.SerializeObject(request.data), Encoding.UTF8, "application/json");
						}
						HttpResponseMessage response = await client.PostAsync(url, content);
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
