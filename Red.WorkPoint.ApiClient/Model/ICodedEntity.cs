namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Coded entity.
    /// </summary>
    public interface ICodedEntity : IEntity
    {
        /// <summary>
        /// Entity code.
        /// </summary>
        string Code { get; set; }
    }
}