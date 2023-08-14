using System;
using System.Windows.Forms;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Red.WorkPoint.ApiClient;
using Red.WorkPoint.ProjectPlugin.Resources;
using Red.WorkPoint.ProjectPlugin.Services;

namespace Red.WorkPoint.ProjectPlugin
{
    public partial class PublishDialog : Form
    {
        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        public PublishDialog()
        {
            InitializeComponent();
            typeTM.Checked = true;

            typeTM.Text = LocalStrings.BillingTypeTM;
            typeFB.Text = LocalStrings.BillingTypeFix;
            typeNB.Text = LocalStrings.BillingTypeNB;
            publishButton.Text = LocalStrings.ButtonPublish;

            Text = LocalStrings.TitlePublish;
            closeButton.Text = LocalStrings.ButtonClose;
            cancelButton.Text = LocalStrings.ButtonCancel;
            closeButton.Visible = false;
            ControlBox = false;
        }

        private async void PublishButton_Click(object sender, EventArgs e)
        {
            using (Telemetry.InitializeDependencyTracking())
            {
                using (var operation = Telemetry.Client.StartOperation<RequestTelemetry>("Publish Project"))
                {
                    Telemetry.Client.TrackEvent("Project Publication");
                    var service = new SyncService(log);

                    try
                    {
                        if (service.CheckProjectFile())
                        {
                            publishButton.Visible = false;
                            cancelButton.Visible = false;
                            typeTM.Enabled = false;
                            typeNB.Enabled = false;
                            typeFB.Enabled = false;

                            log.Items.Add(LocalStrings.MessageAuth);
                            if (!await AuthService.UpdateAccessToken())
                            {
                                log.Items.Add(LocalStrings.MessageOperationWasCanceled);
                                closeButton.Visible = true;
                                ControlBox = true;
                                return;
                            }

                            progressBar.PerformStep();

                            var billingTypeId = ProjectBillingType.TM.Id;
                            if (typeNB.Checked) billingTypeId = ProjectBillingType.NonBillable.Id;
                            if (typeFB.Checked) billingTypeId = ProjectBillingType.FixedBid.Id;

                            DataService.AccessToken = Context.AccessToken;
                            var session = await SessionService.GetSession();
                            progressBar.PerformStep();

                            var newProject = await service.PublishProject(billingTypeId, session.User.Id);
                            progressBar.PerformStep();

                            var props = (Microsoft.Office.Core.DocumentProperties)Context.Application.ActiveProject
                                .CustomDocumentProperties;

                            props.Add(Config.PropNameWpProjectId,
                                false,
                                Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeString,
                                newProject.Id.ToString());

                            Context.ProjectId = newProject.Id;

                            Context.Application.FileSave();

                            await service.SyncTeam();
                            progressBar.PerformStep();

                            await service.SyncTasks();
                            progressBar.PerformStep();

                            await service.SyncResourcePlan();
                            progressBar.PerformStep();

                            Globals.Ribbons.PluginRibbon.WpOpen.Visible = true;
                            Globals.Ribbons.PluginRibbon.WpSync.Visible = true;
                            Globals.Ribbons.PluginRibbon.WpPublish.Visible = false;
                            Context.Application.FileSave();

                            log.Items.Add(LocalStrings.MessagePublishDone);
                        }

                        closeButton.Visible = true;
                        ControlBox = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, LocalStrings.ErrorMessageTitle, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        Telemetry.Client.TrackException(ex);
                        ControlBox = true;
                        closeButton.Visible = true;
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

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}