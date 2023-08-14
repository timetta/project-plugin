using System;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Project state.
    /// </summary>
    public class ProjectState : EnumEntity
    {
        /// <summary>
        /// Cancelled.
        /// </summary>
        public static ProjectState Cancelled => new ProjectState
        {
            Id = new Guid("403af070-8d9f-426c-aba2-70bd6a37a01c"),
            Code = "Cancelled",
            Name = "Cancelled"
        };
        /// <summary>
        /// In progress.
        /// </summary>
        public static ProjectState InProgress => new ProjectState
        {
            Id = new Guid("cd2f2bf7-9388-43b8-9039-dded9700afd2"),
            Code = "InProgress",
            Name = "InProgress"
        };
        /// <summary>
        /// Deferred.
        /// </summary>
        public static ProjectState Deferred => new ProjectState
        {
            Id = new Guid("b086664d-ba47-4083-ba2a-15957a414104"),
            Code = "Deferred",
            Name = "Deferred"
        };
        /// <summary>
        /// Completed.
        /// </summary>
        public static ProjectState Completed => new ProjectState
        {
            Id = new Guid("cfec6d6f-d1dd-4dc7-a922-398973ba5fdc"),
            Code = "Completed",
            Name = "Completed"
        };
        /// <summary>
        /// Archived.
        /// </summary>
        public static ProjectState Archived => new ProjectState
        {
            Id = new Guid("584c3678-ff09-44ff-9116-2d0433d3675e"),
            Code = "Archived",
            Name = "Archived"
        };
        /// <summary>
        /// Tentative.
        /// </summary>
        public static ProjectState Tentative => new ProjectState
        {
            Id = new Guid("9752fc91-714a-414f-9c03-8a3a1d6cce06"),
            Code = "Tentative",
            Name = "Tentative"
        };
    }
}
