namespace Red.WorkPoint.ProjectPlugin
{
    partial class PluginRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public PluginRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Office.Tools.Ribbon.RibbonDialogLauncher ribbonDialogLauncherImpl1 = this.Factory.CreateRibbonDialogLauncher();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginRibbon));
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.WpPublish = this.Factory.CreateRibbonButton();
            this.WpSync = this.Factory.CreateRibbonButton();
            this.WpOpen = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.ControlId.OfficeId = "TabProject";
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "TabProject";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.DialogLauncher = ribbonDialogLauncherImpl1;
            this.group1.Items.Add(this.WpPublish);
            this.group1.Items.Add(this.WpSync);
            this.group1.Items.Add(this.WpOpen);
            this.group1.Label = "Timetta";
            this.group1.Name = "group1";
            this.group1.DialogLauncherClick += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.group1_DialogLauncherClick);
            // 
            // WpPublish
            // 
            this.WpPublish.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.WpPublish.Image = ((System.Drawing.Image)(resources.GetObject("WpPublish.Image")));
            this.WpPublish.Label = "Publish";
            this.WpPublish.Name = "WpPublish";
            this.WpPublish.ShowImage = true;
            this.WpPublish.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.WpPublish_Click);
            // 
            // WpSync
            // 
            this.WpSync.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.WpSync.Image = ((System.Drawing.Image)(resources.GetObject("WpSync.Image")));
            this.WpSync.Label = "Update";
            this.WpSync.Name = "WpSync";
            this.WpSync.ShowImage = true;
            this.WpSync.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SyncWP_Click);
            // 
            // WpOpen
            // 
            this.WpOpen.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.WpOpen.Image = ((System.Drawing.Image)(resources.GetObject("WpOpen.Image")));
            this.WpOpen.Label = "Open";
            this.WpOpen.Name = "WpOpen";
            this.WpOpen.ShowImage = true;
            this.WpOpen.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenWP_Click);
            // 
            // PluginRibbon
            // 
            this.Name = "PluginRibbon";
            this.RibbonType = "Microsoft.Project.Project";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.PluginRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton WpSync;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton WpOpen;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton WpPublish;
    }

    partial class ThisRibbonCollection
    {
        internal PluginRibbon PluginRibbon
        {
            get { return this.GetRibbon<PluginRibbon>(); }
        }
    }
}
