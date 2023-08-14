using System;
using System.Collections.Generic;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Project team member.
    /// </summary>
    public class ProjectTeamMember : Entity
    {
        /// <summary>
        /// Project.
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// Project Id.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Resource.
        /// </summary>
        public virtual Resource Resource { get; set; }

        /// <summary>
        /// Resource Id.
        /// </summary>
        public Guid ResourceId { get; set; }

        /// <summary>
        /// Role description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Role assignments.
        /// </summary>
        public virtual ICollection<ProjectTeamMemberRole> RoleAssignments { get; set; }
    }
}