using System;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Base entity interface.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Id.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Creation date and time.
        /// </summary>
        DateTimeOffset? Created { get; set; }

        /// <summary>
        /// Is active?
        /// </summary>
        bool IsActive { get; set; }
    }
}