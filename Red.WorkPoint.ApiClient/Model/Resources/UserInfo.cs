using System;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// User of the system.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }
    }
}