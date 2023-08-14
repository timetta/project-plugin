using System;
using System.Collections.Generic;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Project.
    /// </summary>
    public class Project : NamedEntity, ICustomizableEntity, ICodedEntity
    {
        /// <summary>
        /// Project code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Project description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// External url.
        /// </summary>
        public string ExternalUrl { get; set; }

        /// <summary>
        /// Project start date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Project end date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Skip manager approve.
        /// </summary>
        public bool SkipManagerApprove { get; set; }

        /// <summary>
        /// Allow time entry for main task.
        /// </summary>
        public bool AllowTimeEntry { get; set; }

        /// <summary>
        /// State Id.
        /// </summary>
        public Guid StateId { get; set; }

        /// <summary>
        /// Project state.
        /// </summary>
        public virtual ProjectState State { get; set; }

        /// <summary>
        /// Billing type Id.
        /// </summary>

        public Guid BillingTypeId { get; set; }

        /// <summary>
        /// Project manager.
        /// </summary>
        public virtual NamedEntity Manager { get; set; }

        /// <summary>
        /// Project manager Id.
        /// </summary>
        public Guid ManagerId { get; set; }

        /// <summary>
        /// Organization(customer).
        /// </summary>
        public virtual NamedEntity Organization { get; set; }

        /// <summary>
        /// Organization Id.
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// Project program.
        /// </summary>
        public NamedEntity Program { get; set; }

        /// <summary>
        /// Project program Id.
        /// </summary>

        public Guid? ProgramId { get; set; }

        /// <summary>
        /// Source project Id.
        /// </summary>

        public Guid? SourceProjectId { get; set; }

        /// <summary>
        /// Source project.
        /// </summary>
        public virtual Project SourceProject { get; set; }

        /// <summary>
        /// Project tasks.
        /// </summary>
        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }

        /// <summary>
        /// Project team members.
        /// </summary>
        public virtual ICollection<ProjectTeamMember> ProjectTeamMembers { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        #region Custom Fields

        public string StringValue1 { get; set; }
        public string StringValue2 { get; set; }
        public string StringValue3 { get; set; }
        public string StringValue4 { get; set; }
        public string StringValue5 { get; set; }

        public decimal? DecimalValue1 { get; set; }
        public decimal? DecimalValue2 { get; set; }
        public decimal? DecimalValue3 { get; set; }
        public decimal? DecimalValue4 { get; set; }
        public decimal? DecimalValue5 { get; set; }

        public DateTime? DateValue1 { get; set; }
        public DateTime? DateValue2 { get; set; }
        public DateTime? DateValue3 { get; set; }
        public DateTime? DateValue4 { get; set; }
        public DateTime? DateValue5 { get; set; }

        public Guid? LookupValue1Id { get; set; }
        public virtual LookupValue LookupValue1 { get; set; }
        public Guid? LookupValue2Id { get; set; }
        public virtual LookupValue LookupValue2 { get; set; }
        public Guid? LookupValue3Id { get; set; }
        public virtual LookupValue LookupValue3 { get; set; }
        public Guid? LookupValue4Id { get; set; }
        public virtual LookupValue LookupValue4 { get; set; }
        public Guid? LookupValue5Id { get; set; }
        public virtual LookupValue LookupValue5 { get; set; }

        public int? IntegerValue1 { get; set; }
        public int? IntegerValue2 { get; set; }
        public int? IntegerValue3 { get; set; }
        public int? IntegerValue4 { get; set; }
        public int? IntegerValue5 { get; set; }

        #endregion
    }
}