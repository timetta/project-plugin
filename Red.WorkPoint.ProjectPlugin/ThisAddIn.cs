using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Red.WorkPoint.ProjectPlugin.Resources;
using Red.WorkPoint.ProjectPlugin.Services;
using MSProject = Microsoft.Office.Interop.MSProject;
using Office = Microsoft.Office.Core;

namespace Red.WorkPoint.ProjectPlugin
{
    public partial class ThisAddIn
    {
        private readonly List<int> _newTaskIds = new List<int>();

        readonly DefaultContractResolver _contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy(),
        };

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            // Set defaults settings.
            if (Properties.Settings.Default.Properties["ImportActualHours"]?.DefaultValue == null)
            {
                Properties.Settings.Default.ImportActualHours = true;
                Properties.Settings.Default.Save();
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 |
                                                   SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            Telemetry.SetUserContext();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = _contractResolver,
            };

            this.Application.NewProject += Application_NewProject;
            this.Application.ProjectTaskNew += Application_ProjectTaskNew;
            Application.PaneActivate += Application_PaneActivate;
            Application.SaveCompletedToServer += ApplicationOnSaveCompletedToServer;
            Application.ProjectAfterSave += Application_ProjectAfterSave;
            Context.Application = Application;
        }

        private void Application_ProjectAfterSave()
        {
            InitActiveProject();
        }

        private void ApplicationOnSaveCompletedToServer(string bstrname, string bstrprojguid)
        {
            InitActiveProject();
        }

        private void Application_ProjectTaskNew(MSProject.Project pj, int taskId)
        {
            _newTaskIds.Add(taskId);
        }

        private void Application_PaneActivate()
        {
            InitActiveProject();
        }

        private void InitActiveProject()
        {
            try
            {
                if (Application.ActiveProject == null) return;

                Application.ActiveProject.Change -= ActiveProject_Change;
                Application.ActiveProject.Change += ActiveProject_Change;
                Context.Project = Application.ActiveProject;
                Context.Project.Application.CustomFieldRename(MSProject.PjCustomField.pjCustomTaskText30,
                    "WorkPoint Task ID");
                var props = (Office.DocumentProperties)Context.Project.CustomDocumentProperties;

                Guid? projectId = null;
                if (props.Count > 0)
                {
                    foreach (Office.DocumentProperty documentProperty in props)
                    {
                        if (documentProperty.Name == Config.PropNameWpProjectId)
                        {
                            projectId = Guid.Parse(documentProperty.Value);
                        }
                    }
                }

                Globals.Ribbons.PluginRibbon.WpPublish.Label = LocalStrings.CommandPublish;
                Globals.Ribbons.PluginRibbon.WpOpen.Label = LocalStrings.CommandOpen;
                Globals.Ribbons.PluginRibbon.WpSync.Label = LocalStrings.CommandSync;
                Globals.Ribbons.PluginRibbon.WpOpen.Visible = true;
                Globals.Ribbons.PluginRibbon.WpSync.Visible = true;
                Globals.Ribbons.PluginRibbon.WpPublish.Visible = true;

                if (!projectId.HasValue)
                {
                    Globals.Ribbons.PluginRibbon.WpOpen.Visible = false;
                    Globals.Ribbons.PluginRibbon.WpSync.Visible = false;
                }
                else
                {
                    Globals.Ribbons.PluginRibbon.WpPublish.Visible = false;
                    Context.ProjectId = projectId.Value;
                }
            }
            catch (Exception ex)
            {
                Telemetry.Client.TrackException(ex);
            }
        }

        private void ActiveProject_Change(MSProject.Project pj)
        {
            if (_newTaskIds.Count > 0)
            {
                foreach (var newTaskId in _newTaskIds)
                {
                    // When copying, remove the linking with the Timetta task.
                    try
                    {
                        var task = pj.Tasks.UniqueID[newTaskId];
                        task?.SetField(MSProject.PjField.pjTaskText30, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }
            }

            _newTaskIds.Clear();
        }

        private void Application_NewProject(MSProject.Project pj)
        {
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
            Telemetry.Client.Flush();
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += ThisAddIn_Startup;
            this.Shutdown += ThisAddIn_Shutdown;
        }

        #endregion
    }
}