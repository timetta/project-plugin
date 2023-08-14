using System;
using Microsoft.Office.Interop.MSProject;

namespace Red.WorkPoint.ProjectPlugin
{
    public static class Context
    {
        /// <summary>
        /// Project Id.
        /// </summary>
        public static Guid ProjectId { get; set; }

        /// <summary>
        /// Application.
        /// </summary>
        public static Application Application { get; set; }

        /// <summary>
        /// Project.
        /// </summary>
        public static Project Project { get; set; }

        /// <summary>
        /// Access token.
        /// </summary>
        public static string AccessToken { get; set; }

        /// <summary>
        /// Access token expiry time.
        /// </summary>
        public static DateTime ExpiryTime { get; set; }
    }
}