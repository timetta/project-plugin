namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Named entity interface.
    /// </summary>
    public interface INamedEntity : IMainEntity
    {
        /// <summary>
        /// Entity name.
        /// </summary>
        string Name { get; set; }
    }
}
