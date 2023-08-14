using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Client session.
    /// </summary>
    public class ClientSession
    {
        /// <summary>
        /// User info.
        /// </summary>
        public NamedEntity User { get; set; }

        /// <summary>
        /// Specifies the user is in substitution mode.
        /// </summary>
        public bool IsSubstituteMode { get; set; }

        /// <summary>
        /// Current user roles.
        /// </summary>
        public ICollection<string> Roles { get; set; }

        /// <summary>
        /// Time zone.
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Culture code.
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Time input type.
        /// </summary>
        public string TimeInputType { get; set; }

        /// <summary>
        /// Use stopwatch
        /// </summary>
        public bool UseStopwatch { get; set; }
    }
}