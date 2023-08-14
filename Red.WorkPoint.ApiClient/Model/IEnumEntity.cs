namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Enumeration entity interface.
    /// </summary>
    public interface IEnumEntity : ICodedEntity
    {
        /// <summary>
        /// Name of enumeration element.
        /// </summary>
        string Name { get; set; }
    }
}