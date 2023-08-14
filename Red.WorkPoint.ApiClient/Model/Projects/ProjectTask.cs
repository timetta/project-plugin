using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Project task.
    /// </summary>
    public class ProjectTask : NamedEntity
    {
        /// <summary>
        /// Project Id.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Estimated duration.
        /// </summary>
        public decimal? EstimatedDuration { get; set; }

        /// <summary>
        /// Task number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Specifies whether time can be entered.
        /// </summary>
        public bool AllowTimeEntry { get; set; }

        /// <summary>
        /// Lead task Id.
        /// </summary>
        public Guid? LeadTaskId { get; set; }

        /// <summary>
        /// Task end date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Task start date.
        /// </summary>
        public DateTime? StartDate { get; set; }

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

        #endregion

        /// <summary>
        /// Project tasks assignments.
        /// </summary>
        public List<ProjectTaskAssignment> ProjectTaskAssignments { get; set; }
    }
}