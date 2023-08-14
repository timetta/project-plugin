using Newtonsoft.Json.Converters;

namespace Red.WorkPoint.ApiClient.Helpers
{
    /// <summary>
    /// OData DateTime converter.
    /// </summary>
    class ODataDateTimeConverter : IsoDateTimeConverter
    {
        public ODataDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}