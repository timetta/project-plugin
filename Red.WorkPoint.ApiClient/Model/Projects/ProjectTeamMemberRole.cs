using System;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Supporting entity for the bundle ProjectTeamMember and Role.
    /// </summary>
    public class ProjectTeamMemberRole : Entity
    {
        /// <summary>
        /// Role Id.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Project team member Id.
        /// </summary>
        public Guid ProjectTeamMemberId { get; set; }
    }
}