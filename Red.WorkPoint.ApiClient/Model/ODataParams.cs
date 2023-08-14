
namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// OData parameters.
    /// </summary>
    public class ODataParams
    {
        /// <summary>
        /// Specifies count returned elements from the start of a sequence.
        /// </summary>
        public int? Top { get; set; }

        /// <summary>
        /// Specifies count skipping elements from the start of a sequence.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Selects one or few properties of related entity.
        /// </summary>
        public string Select { get; set; }

        /// <summary>
        /// Specifies related entities to include in the query result.
        /// </summary>
        public string Expand { get; set; }

        /// <summary>
        /// Sorts the elements of a sequence according to a key.
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Filters the elements of a sequence by expression.
        /// </summary>
        public string Filter { get; set; }
    }
}
