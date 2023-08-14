using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Red.WorkPoint.ApiClient.Data
{
    /// <summary>
    /// Function OData functionality
    /// </summary>
    public class Function
    {
        private readonly string _baseUrl;

        public Function(string url)
        {
            _baseUrl = url;
        }

        /// <summary>
        /// Returns a single entity corresponding the given OData parameters.
        /// </summary>
        /// <param name="oDataParams">OData parameters.</param>
        /// <param name="urlParams">Query parameters.</param>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <returns>Entity.</returns>
        /// <exception cref="Exception">Throws on connection troubles.</exception>
        public async Task<T> Get<T>(ODataParams oDataParams = null, Dictionary<string, string> urlParams = null)
        {
            try
            {
                var url = $"{_baseUrl}{DataService.GetODataQueryString(oDataParams, urlParams)}";

                using (var client = DataService.GetHttpClient())
                {
                    var response = await client.GetAsync(url);
                    await DataService.CheckResponse(response);
                    var content = await response.Content.ReadAsStringAsync();

                    if (typeof(T).IsPrimitive || typeof(T) == typeof(Guid))
                    {
                        var jsonObject = JObject.Parse(content);
                        var result = jsonObject["value"];


                        var data = typeof(T).IsPrimitive
                            ? result.Value<T>()
                            : (T)Convert.ChangeType(Guid.Parse(result.Value<string>()), typeof(T));
                        return data;
                    }
                    else
                    {
                        var data = JsonConvert.DeserializeObject<T>(content);
                        return data;
                    }
                }
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

        /// <summary>
        /// OData query for current collection.
        /// </summary>
        /// <param name="oDataParams">OData parameters.</param>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <returns>Current collection with query OData parameters.</returns>
        /// <exception cref="Exception">Throws on connection troubles.</exception>
        public async Task<List<T>> Query<T>(ODataParams oDataParams = null, Dictionary<string, string> urlParams = null)
        {
            try
            {
                var url = $"{_baseUrl}{DataService.GetODataQueryString(oDataParams, urlParams)}";

                using (var client = DataService.GetHttpClient())
                {
                    var response = await client.GetAsync(url);
                    await DataService.CheckResponse(response);
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(content);
                    IList<JToken> results = jsonObject["value"].Children().ToList();

                    return results.Select(result => result.ToObject<T>()).ToList();
                }
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