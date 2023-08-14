using System;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Lookup value for custom field.
    /// </summary>
    public class LookupValue : NamedEntity
    {
        /// <summary>
        /// Custom field Id.
        /// </summary>
        public Guid CustomFieldId { get; set; }
    }
}