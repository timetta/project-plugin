using System;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Data transfer object for ResourcePlan.
    /// </summary>
    public class ResourcePlanDto
    {
        /// <summary>
        /// Task Id.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Team member Id.
        /// </summary>
        public Guid TeamMemberId { get; set; }

        /// <summary>
        /// Role Id.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Hours.
        /// </summary>
        public decimal Hours { get; set; }
    }
}