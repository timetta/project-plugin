using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Red.WorkPoint.ApiClient.Helpers;

namespace Red.WorkPoint.ApiClient.Data
{
    /// <summary>
    /// Action OData functionality.
    /// </summary>
    public class Action
    {
        private readonly string _baseUrl;

        public Action(string url)
        {
            _baseUrl = url;
        }

        /// <summary>
        /// Execute OData action.
        /// </summary>
        /// <param name="o">Action OData parameters.</param>
        /// <exception cref="Exception">Throws on connection troubles.</exception>
        public async Task Execute(object o = null)
        {
            var url = $"{_baseUrl}";

            using (var client = DataService.GetHttpClient())
            {
                var data = JsonConvert.SerializeObject(o, new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>
                    {
                        new ODataDateTimeConverter()
                    }
                });
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                await DataService.CheckResponse(response);

                try
                {
                   await response.Content.ReadAsStringAsync();
                }
                catch (TaskCanceledException)
                {
                    throw new Exception("Connection trouble.");
                }
                catch (OperationCanceledException)
                {
                    throw new Exception("Connection trouble.");
                }
            }
        }
    }
}

