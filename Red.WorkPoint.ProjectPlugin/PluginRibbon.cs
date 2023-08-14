using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;
using Red.WorkPoint.ProjectPlugin.Resources;

namespace Red.WorkPoint.ProjectPlugin
{
    public partial class PluginRibbon
    {
        private void PluginRibbon_Load(object sender, RibbonUIEventArgs e)
        {
        }

        private void SyncWP_Click(object sender, RibbonControlEventArgs e)
        {
            if (!ProjectPlugin.Context.Application.FileSave()) return;

            var syncDialog = new SyncDialog();
            syncDialog.ShowDialog();
        }


        private void OpenWP_Click(object sender, RibbonControlEventArgs e)
        {
            Process.Start($"{Config.AppUrl}/projects/{ProjectPlugin.Context.ProjectId}");
        }

        private void WpPublish_Click(object sender, RibbonControlEventArgs e)
        {
            if (!ProjectPlugin.Context.Application.FileSave()) return;

            var publishDialog = new PublishDialog();
            publishDialog.ShowDialog();
        }

        private void group1_DialogLauncherClick(object sender, RibbonControlEventArgs e)
        {
            var settingsDialog = new Settings();
            settingsDialog.ShowDialog();
        }
    }
}