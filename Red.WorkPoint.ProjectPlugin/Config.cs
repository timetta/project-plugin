using Red.WorkPoint.ApiClient;

namespace Red.WorkPoint.ProjectPlugin
{
    public static class Config
    {
        /// <summary>
        /// Document property name for keeping project Id.
        /// </summary>
        public static string PropNameWpProjectId => "WpProjectId";

        /// <summary>
        /// Passport url.
        /// </summary>
        public static string PassportUrl { get; set; }

        /// <summary>
        /// Web application url.
        /// </summary>
        public static string AppUrl { get; set; }

        /// <summary>
        /// API url.
        /// </summary>
        private static string ApiUrl { get; set; }

        /// <summary>
        /// App insight instrumentation key.
        /// </summary>
        public static string AppInsightInstrumentationKey { get; set; }

        static Config()
        {
            PassportUrl = Properties.Settings.Default.PassportUrl;
            AppUrl = Properties.Settings.Default.AppUrl;
            DataService.ApiUrl = ApiUrl = Properties.Settings.Default.ApiUrl;
            AppInsightInstrumentationKey = Properties.Settings.Default.AppInsightInstrumentationKey;
        }
    }
}