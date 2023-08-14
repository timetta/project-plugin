namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Entity with timestamp interface.
    /// </summary>
    public interface IMainEntity : IEntity
    {
        /// <summary>
        /// Timestamp.
        /// </summary>
        uint RowVersion { get; set; }
    }
}