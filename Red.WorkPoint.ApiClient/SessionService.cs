using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Service for handling client session.
    /// </summary>
    public class SessionService
    {
        /// <summary>
        /// Create client session.
        /// </summary>
        /// <returns>Client session.</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<ClientSession> GetSession()
        {
            try
            {
                using (var client = DataService.GetHttpClient())
                {
                    var response = await client.GetAsync($"{DataService.ApiUrl}/GetSession");
                    await DataService.CheckResponse(response);
                    var content = await response.Content.ReadAsStringAsync();
                    var session = JsonConvert.DeserializeObject<ClientSession>(content);
                    return session;
                }
            }
            catch (TaskCanceledException)
            {
                throw new Exception("Connection trouble.");
            }
            catch (Exception)
            {
                throw new Exception("Connection trouble.");
            }
        }
    }
}