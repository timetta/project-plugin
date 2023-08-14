using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Red.WorkPoint.ApiClient.Helpers;

namespace Red.WorkPoint.ApiClient.Data
{
    /// <summary>
    /// Entity OData functionality.
    /// </summary>
    public class Entity
    {
        private readonly string _baseUrl;

        public Entity(Guid id, string baseUrl)
        {
            _baseUrl = $"{baseUrl}({id})";
        }

        /// <summary>
        /// Returns entities collection.
        /// </summary>
        /// <param name="name">Collection name.</param>
        /// <returns>Collection.</returns>
        public Collection Collection(string name)
        {
            return new Collection(_baseUrl, name);
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
                    var data = JsonConvert.DeserializeObject<T>(content);

                    return data;
                }
            }
            catch (TaskCanceledException)
            {
                throw new Exception("Connection trouble.");
            }
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <returns>Updated entity.</returns>
        /// <exception cref="Exception">Throws on connection troubles.</exception>
        public async Task<T> Update<T>(T entity)
        {
            try
            {
                var url = $"{_baseUrl}";

                using (var client = DataService.GetHttpClient())
                {
                    client.DefaultRequestHeaders.Add("Prefer", "return=representation");

                    var data = JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,

                        Converters = new List<JsonConverter>
                        {
                            new ODataDateTimeConverter(),
                            new StringEnumConverter()
                        }
                    });

                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(url, content);
                    await DataService.CheckResponse(response);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<T>(responseContent);

                    return responseData;
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
        /// Patches the entity.
        /// </summary>
        /// <param name="data">Patch data.</param>
        /// <exception cref="Exception">Throws on connection troubles.</exception>
        public async Task Patch(object data)
        {
            try
            {
                var url = $"{_baseUrl}";

                using (var client = DataService.GetHttpClient())
                {
                    client.DefaultRequestHeaders.Add("Prefer", "return=representation");

                    var contentData = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,

                        Converters = new List<JsonConverter>
                        {
                            new ODataDateTimeConverter()
                        }
                    });

                    var content = new StringContent(contentData, Encoding.UTF8, "application/json");
                    var message = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };
                    var response = await client.SendAsync(message);
                    await DataService.CheckResponse(response);
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
        /// Function to the entity.
        /// </summary>
        /// <param name="name">Function name.</param>
        /// <param name="parameters">Function parameters.</param>
        /// <returns>Function.</returns>
        public Function Function(string name, Dictionary<string, string> parameters = null)
        {
            var url = $"{_baseUrl}/{name}";
            if (parameters != null)
            {
                url += "(";
                url = parameters.Aggregate(url, (current, parameter) => current + $"{parameter.Key}={parameter.Value},")
                    .TrimEnd(',');
                url += ")";
            }

            return new Function(url);
        }

        /// <summary>
        /// Action to the entity.
        /// </summary>
        /// <param name="name">Action name.</param>
        /// <returns>Action.</returns>
        public Action Action(string name)
        {
            var url = $"{_baseUrl}/{name}";
            return new Action(url);
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        public async Task Delete()
        {
            var url = $"{_baseUrl}";

            using (var client = DataService.GetHttpClient())
            {
                client.DefaultRequestHeaders.Add("Prefer", "return=representation");
                var message = new HttpRequestMessage(new HttpMethod("DELETE"), url);
                var response = await client.SendAsync(message);
                await DataService.CheckResponse(response);
            }
        }
    }
}