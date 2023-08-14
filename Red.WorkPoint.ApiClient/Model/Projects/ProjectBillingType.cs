using System;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Billing type.
    /// </summary>
    public class ProjectBillingType : EnumEntity
    {
        /// <summary>
        /// Non billable.
        /// </summary>
        public static ProjectBillingType NonBillable => new ProjectBillingType
        {
            Id = new Guid("4d1a525f-3abc-4871-a64a-349c1dd3cabf"),
            Code = "NonBillable",
            Name = "Non Billable"
        };

        /// <summary>
        /// Time and money.
        /// </summary>
        // ReSharper disable once InconsistentNaming так сложилось исторически.
        public static ProjectBillingType TM => new ProjectBillingType
        {
            Id = new Guid("584dddc1-94df-43b2-b3f3-372c02fcb016"),
            Code = "TM",
            Name = "T&M"
        };

        /// <summary>
        /// Fixed bid.
        /// </summary>
        public static ProjectBillingType FixedBid => new ProjectBillingType
        {
            Id = new Guid("e87e0e6b-c034-45ac-8b74-bd0256f3f535"),
            Code = "FixedBid",
            Name = "Fixed Bid"
        };
    }
}
