
using Newtonsoft.Json;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// OData error.
    /// </summary>
    public class ODataError
    {
        /// <summary>
        /// Error message.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Error code.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
