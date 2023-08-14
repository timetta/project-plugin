using System;
using System.Windows.Forms;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Red.WorkPoint.ProjectPlugin.Resources;
using Red.WorkPoint.ProjectPlugin.Services;

namespace Red.WorkPoint.ProjectPlugin
{
    public partial class SyncDialog : Form
    {
        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        public SyncDialog()
        {
            InitializeComponent();
            Text = LocalStrings.TitleSync;
            closeButton.Visible = false;
            closeButton.Text = LocalStrings.ButtonClose;
            ControlBox = false;
            Sync();
        }

        private async void Sync()
        {
            using (Telemetry.InitializeDependencyTracking())
            {
                using (var operation = Telemetry.Client.StartOperation<RequestTelemetry>("Sync Project"))
                {
                    try
                    {
                        Telemetry.Client.TrackEvent("Project Synchronization");
                        var service = new SyncService(log);

                        if (service.CheckProjectFile())
                        {
                            log.Items.Add(LocalStrings.MessageAuth);
                            if (!await AuthService.UpdateAccessToken())
                            {
                                log.Items.Add(LocalStrings.MessageOperationWasCanceled);
                                closeButton.Visible = true;
                                ControlBox = true;
                                return;
                            }

                            progressBar.PerformStep();
                            await service.UpdateProject();
                            progressBar.PerformStep();

                            await service.SyncActualHours();
                            progressBar.PerformStep();

                            await service.SyncTeam();
                            progressBar.PerformStep();

                            await service.SyncTasks();
                            progressBar.PerformStep();

                            await service.SyncResourcePlan();
                            progressBar.PerformStep();

                            log.Items.Add(LocalStrings.MessageSyncDone);
                        }

                        closeButton.Visible = true;
                        ControlBox = true;
                    }
                    catch (Exception ex)
                    {
                        Telemetry.Client.TrackException(ex);
                        closeButton.Visible = true;
                        ControlBox = true;
                        MessageBox.Show(ex.Message, LocalStrings.ErrorMessageTitle, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    finally
                    {
                        Telemetry.Client.StopOperation(operation);
                    }
                }
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}