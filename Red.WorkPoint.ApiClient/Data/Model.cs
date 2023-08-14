
namespace Red.WorkPoint.ApiClient.Data
{
    /// <summary>
    /// Unbound OData functionality
    /// </summary>
    public class Model
    {
        public Model()
        {

        }

        /// <summary>
        /// Returns function by name.
        /// </summary>
        /// <param name="name">Function name.</param>
        /// <returns>Function.</returns>
        public Function Function(string name)
        {
            var baseUrl = DataService.ApiUrl + "/" + name;
            return new Function(baseUrl);
        }
    }
}