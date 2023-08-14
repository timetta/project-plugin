using System;
using System.Diagnostics;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Base entity.
    /// </summary>
    [DebuggerDisplay("Id = {Id}, Created = {Created}")]
    public abstract class Entity : IEntity
    {
        /// <inheritdoc />
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <inheritdoc />
        public DateTimeOffset? Created { get; set; } = DateTimeOffset.UtcNow;

        /// <inheritdoc />
        public bool IsActive { get; set; } = true;
    }
}
