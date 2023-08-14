namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Resource.
    /// Root entity for users, departments and roles.
    /// </summary>
    public class Resource : NamedEntity, ICodedEntity
    {
        /// <inheritdoc/>
        public string Code { get; set; }
    }
}