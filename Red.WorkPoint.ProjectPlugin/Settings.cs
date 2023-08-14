using System;
using System.Deployment.Application;
using System.Windows.Forms;
using Red.WorkPoint.ProjectPlugin.Resources;
using Red.WorkPoint.ProjectPlugin.Services;

namespace Red.WorkPoint.ProjectPlugin
{
    public partial class Settings : Form
    {
        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        public Settings()
        {
            InitializeComponent();

            Text = LocalStrings.TitleSettings;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var applicationDeployment = ApplicationDeployment.CurrentDeployment;
                var version = applicationDeployment.CurrentVersion;
                VersionInfo.Text = $@"Version: {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
            else
            {
                VersionInfo.Text = @"WorkPoint Project Plugin";
            }

            signOutButton.Text = LocalStrings.ButtonDeleteAuth;
            buttonSave.Text = LocalStrings.ButtonSave;
            improtActualHours.Text = LocalStrings.SettingsImprotActualHours;

            if (string.IsNullOrEmpty(Properties.Settings.Default.User))
            {
                groupBox1.Visible = false;
            }
            else
            {
                currentUserName.Text = Properties.Settings.Default.User;
            }

            // Applying settings.
            improtActualHours.Checked = Properties.Settings.Default.ImportActualHours;

        }


        private void DeleteAuth_Click(object sender, EventArgs e)
        {
            AuthService.Logout();
            groupBox1.Visible = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ImportActualHours = improtActualHours.Checked;
            Properties.Settings.Default.Save();
            Close();
        }

        private void currentUserName_Click(object sender, EventArgs e)
        {

        }
    }
}
