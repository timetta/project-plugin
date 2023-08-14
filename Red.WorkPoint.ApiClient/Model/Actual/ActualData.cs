namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Allocation data.
    /// </summary>
    public class AllocationData : ReportDate
    {
        /// <summary>
        /// User.
        /// </summary>
        public UserInfo User { get; set; }

        /// <summary>
        /// Project.
        /// </summary>
        public NamedEntity Project { get; set; }

        /// <summary>
        /// Project task.
        /// </summary>
        public NamedEntity ProjectTask { get; set; }

        /// <summary>
        /// Billing rate.
        /// </summary>
        public NamedEntity BillingRate { get; set; }

        /// <summary>
        /// Activity.
        /// </summary>
        public NamedEntity Activity { get; set; }

        /// <summary>
        /// Hours.
        /// </summary>
        public double Hours { get; set; }

    }
}
