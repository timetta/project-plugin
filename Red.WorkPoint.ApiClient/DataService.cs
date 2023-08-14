using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Red.WorkPoint.ApiClient.Data;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Service for communicating with OData.
    /// </summary>
    public static class DataService
    {
        /// <summary>
        /// Timetta API url.
        /// </summary>
        public static string ApiUrl { get; set; }

        /// <summary>
        /// Timetta API access token.
        /// </summary>
        public static string AccessToken { get; set; }

        /// <summary>
        /// Create HttpClient.
        /// </summary>
        /// <returns>HttpClient.</returns>
        public static HttpClient GetHttpClient()
        {
            var client = new HttpClient
            {
                MaxResponseContentBufferSize = 8 * 1024 * 1024 * 5 // 5 Mb
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            return client;
        }

        /// <summary>
        /// Get OData query string.
        /// </summary>
        /// <param name="oDataParams">OData parameters.</param>
        /// <param name="urlParams">Query(url) parameters.</param>
        /// <returns>OData query string.</returns>
        public static string GetODataQueryString(ODataParams oDataParams, Dictionary<string, string> urlParams = null)
        {
            if (oDataParams == null)
                return "";

            var query = "";

            if (oDataParams.Top.HasValue)
            {
                query += $"$top={oDataParams.Top}&";
            }

            if (oDataParams.Skip.HasValue)
            {
                query += $"$skip={oDataParams.Skip}&";
            }

            if (!string.IsNullOrEmpty(oDataParams.Select))
            {
                query += $"$select={oDataParams.Select}&";
            }

            if (!string.IsNullOrEmpty(oDataParams.Filter))
            {
                query += $"$filter={oDataParams.Filter}&";
            }

            if (!string.IsNullOrEmpty(oDataParams.Expand))
            {
                query += $"$expand={oDataParams.Expand}&";
            }

            if (!string.IsNullOrEmpty(oDataParams.OrderBy))
            {
                query += $"$orderby={oDataParams.OrderBy}&";
            }

            if (urlParams != null)
            {
                query = urlParams.Aggregate(query,
                    (current, urlParam) => current + $"{urlParam.Key}={urlParam.Value}&");
            }

            if (query.Length > 0)
            {
                query = "?" + query.TrimEnd('&');
            }

            return query;
        }

        /// <summary>
        /// Check response.
        /// </summary>
        /// <param name="response">Http response.</param>
        /// <exception cref="Exception"></exception>
        public static async Task CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.RequestTimeout ||
                    response.StatusCode == HttpStatusCode.BadGateway ||
                    response.StatusCode == HttpStatusCode.GatewayTimeout)
                {
                    throw new Exception("Connection trouble.");
                }

                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    var jObject = JToken.Parse(content);
                    var error = JsonConvert.DeserializeObject<ODataError>(jObject["error"].ToString());
                    if (error != null && error.Code == "WpConcurrencyException")
                    {
                    }

                    if (error != null)
                    {
                        throw new Exception(error.Message);
                    }
                }
                catch
                {
                    throw new Exception(content);
                }
            }
        }

        /// <summary>
        /// Returns model.
        /// </summary>
        public static Model Model()
        {
            var model = new Model();
            return model;
        }

        /// <summary>
        /// Get collection by name.
        /// </summary>
        /// <param name="collectionName">Collection name.</param>
        public static Collection Collection(string collectionName)
        {
            var collection = new Collection(ApiUrl, collectionName);
            return collection;
        }
    }
}