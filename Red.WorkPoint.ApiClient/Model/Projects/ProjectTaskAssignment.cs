using System;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Project task assignment.
    /// </summary>
    public class ProjectTaskAssignment : Entity
    {
        /// <summary>
        /// Project task.
        /// </summary>
        public virtual ProjectTask ProjectTask { get; set; }

        /// <summary>
        /// Project task Id.
        /// </summary>
        public Guid ProjectTaskId { get; set; }

        /// <summary>
        /// Project team member.
        /// </summary>
        public ProjectTeamMember ProjectTeamMember { get; set; }

        /// <summary>
        /// Project team member id.
        /// </summary>
        public Guid? ProjectTeamMemberId { get; set; }

        /// <summary>
        /// Specifies assignment to the all project team.
        /// </summary>
        public bool IsAllTeamRole { get; set; }
    }
}