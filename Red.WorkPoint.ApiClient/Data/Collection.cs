using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Red.WorkPoint.ApiClient.Helpers;

namespace Red.WorkPoint.ApiClient.Data
{
    /// <summary>
    /// Collection OData functionality.
    /// </summary>
    public class Collection
    {
        private readonly string _baseUrl;

        public Collection(string rootUrl, string collectionName)
        {
            _baseUrl = $"{rootUrl}/{collectionName}";
        }

        /// <summary>
        /// Returns entity by Id.
        /// </summary>
        /// <param name="id">Entity Id.</param>
        public Entity Entity(Guid id)
        {
            return new Entity(id, _baseUrl);
        }

        /// <summary>
        /// Inserts entity to current collection.
        /// </summary>
        /// <param name="entity">Entity for insert.</param>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <returns>Inserted entity.</returns>
        /// <exception cref="Exception">Throws on connection troubles.</exception>
        public async Task<T> Insert<T>(T entity)
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
                    var response = await client.PostAsync(url, content);
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
        /// OData query for current collection.
        /// </summary>
        /// <param name="oDataParams">OData parameters.</param>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <returns>Current collection with query OData parameters.</returns>
        /// <exception cref="Exception">Throws on connection troubles.</exception>
        public async Task<List<T>> Query<T>(ODataParams oDataParams = null)
        {
            var url = $"{_baseUrl}{DataService.GetODataQueryString(oDataParams)}";

            using (var client = DataService.GetHttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    await DataService.CheckResponse(response);
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(content);
                    IList<JToken> results = jsonObject["value"].Children().ToList();

                    return results.Select(result => result.ToObject<T>()).ToList();
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

        /// <summary>
        /// Builds function for current collection.
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
    }
}