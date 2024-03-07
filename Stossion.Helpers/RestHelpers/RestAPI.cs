using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Stossion.Helpers.RestHelpers
{
    public static class RestAPI
    {
        public static async Task<T> Get<T>(string Controller, string? MethodName = null, string? host = null, string? paramName = null, string? param = null)
        {
            try
            {
                var header = string.Empty;

                if (String.IsNullOrEmpty(MethodName))
                {
                    header = Controller;
                }
                else
                {
                    if (String.IsNullOrEmpty(host))
                    {
                        host += "/";
                    }
                    header = host + Controller + (String.IsNullOrEmpty(MethodName) ? string.Empty : "/" + MethodName);
                    if (!String.IsNullOrEmpty(param))
                    {
                        header = header + "?" + paramName + "="  + param;
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
                            var result = await response.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<T>(result) ?? default(T);
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static async Task<T> Post<T,X>(string Controller, string MethodName, X data,string? host = null)
        {
            try
            {
                var url = string.Empty;

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
                        HttpResponseMessage response = await client.PostAsJsonAsync(url,data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<T>(result) ?? default(T);
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

    }
}
