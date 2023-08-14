using System.Diagnostics;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Named entity.
    /// </summary>
    public class NamedEntity : Entity, INamedEntity
    {
        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public uint RowVersion { get; set; }
    }
}
