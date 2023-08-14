namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Enumeration entity.
    /// </summary>
    public abstract class EnumEntity : Entity, IEnumEntity
    {
        /// <inheritdoc />
        public string Code { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }
    }
}