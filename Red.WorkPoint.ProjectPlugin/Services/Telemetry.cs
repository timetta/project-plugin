using System.Deployment.Application;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DependencyCollector;

namespace Red.WorkPoint.ProjectPlugin.Services
{
    /// <summary>
    /// Telemetry service.
    /// </summary>
    public static class Telemetry
    {
        private const string CloudRoleName = "project_plugin";

        /// <summary>
        /// Telemetry client.
        /// </summary>
        public static TelemetryClient Client { get; }

        private static TelemetryConfiguration Configuration { get; set; }


        static Telemetry()
        {
            Configuration = TelemetryConfiguration.Active;
            Configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            Configuration.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());

            Client = new TelemetryClient
            {
                InstrumentationKey = Config.AppInsightInstrumentationKey
            };

            Client.Context.Cloud.RoleName = CloudRoleName;
            Client.Context.Device.OperatingSystem = System.Environment.OSVersion.ToString();

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var applicationDeployment = ApplicationDeployment.CurrentDeployment;
                var version = applicationDeployment.CurrentVersion;
                Client.Context.Component.Version =
                    $@"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }

            Client.Context.User.AccountId = "";
            Client.Context.User.Id = "";
        }

        /// <summary>
        /// Initializes dependency tracking.
        /// </summary>
        /// <returns>DependencyTrackingTelemetryModule.</returns>
        public static DependencyTrackingTelemetryModule InitializeDependencyTracking()
        {
            var module = new DependencyTrackingTelemetryModule();
            module.Initialize(Configuration);
            return module;
        }

        /// <summary>
        /// Sets user context.
        /// </summary>
        public static void SetUserContext()
        {
            Client.Context.User.AccountId = Properties.Settings.Default.Tenant;
            Client.Context.User.Id = Properties.Settings.Default.User;
        }
    }
}