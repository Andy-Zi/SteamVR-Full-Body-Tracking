namespace PTSC.Ui.View
{
    partial class MainView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.ModuleTab = new System.Windows.Forms.TabPage();
            this.groupBoxVisualization = new System.Windows.Forms.GroupBox();
            this.tabControlModuleView = new System.Windows.Forms.TabControl();
            this.tabPageModuleView = new System.Windows.Forms.TabPage();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.tabPageSkeleton = new System.Windows.Forms.TabPage();
            this.groupBoxModule = new System.Windows.Forms.GroupBox();
            this.labelModuleLog = new System.Windows.Forms.Label();
            this.richTextBoxModule = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonStopModule = new System.Windows.Forms.Button();
            this.buttonStartModule = new System.Windows.Forms.Button();
            this.comboBoxModule = new System.Windows.Forms.ComboBox();
            this.groupBoxDriver = new System.Windows.Forms.GroupBox();
            this.SettingsTab = new System.Windows.Forms.TabPage();
            this.labelFpsLimit = new System.Windows.Forms.Label();
            this.numericUpDownFps = new System.Windows.Forms.NumericUpDown();
            this.tabControl.SuspendLayout();
            this.ModuleTab.SuspendLayout();
            this.groupBoxVisualization.SuspendLayout();
            this.tabControlModuleView.SuspendLayout();
            this.tabPageModuleView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.groupBoxModule.SuspendLayout();
            this.SettingsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFps)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.ModuleTab);
            this.tabControl.Controls.Add(this.SettingsTab);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1194, 806);
            this.tabControl.TabIndex = 0;
            // 
            // ModuleTab
            // 
            this.ModuleTab.Controls.Add(this.groupBoxVisualization);
            this.ModuleTab.Controls.Add(this.groupBoxModule);
            this.ModuleTab.Controls.Add(this.groupBoxDriver);
            this.ModuleTab.Location = new System.Drawing.Point(4, 24);
            this.ModuleTab.Name = "ModuleTab";
            this.ModuleTab.Padding = new System.Windows.Forms.Padding(3);
            this.ModuleTab.Size = new System.Drawing.Size(1186, 778);
            this.ModuleTab.TabIndex = 0;
            this.ModuleTab.Text = "Module";
            this.ModuleTab.UseVisualStyleBackColor = true;
            // 
            // groupBoxVisualization
            // 
            this.groupBoxVisualization.Controls.Add(this.tabControlModuleView);
            this.groupBoxVisualization.Location = new System.Drawing.Point(8, 6);
            this.groupBoxVisualization.Name = "groupBoxVisualization";
            this.groupBoxVisualization.Size = new System.Drawing.Size(820, 764);
            this.groupBoxVisualization.TabIndex = 2;
            this.groupBoxVisualization.TabStop = false;
            this.groupBoxVisualization.Text = "Visualization";
            // 
            // tabControlModuleView
            // 
            this.tabControlModuleView.Controls.Add(this.tabPageModuleView);
            this.tabControlModuleView.Controls.Add(this.tabPageSkeleton);
            this.tabControlModuleView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlModuleView.Location = new System.Drawing.Point(3, 19);
            this.tabControlModuleView.Name = "tabControlModuleView";
            this.tabControlModuleView.SelectedIndex = 0;
            this.tabControlModuleView.Size = new System.Drawing.Size(814, 742);
            this.tabControlModuleView.TabIndex = 1;
            // 
            // tabPageModuleView
            // 
            this.tabPageModuleView.Controls.Add(this.pictureBoxImage);
            this.tabPageModuleView.Location = new System.Drawing.Point(4, 24);
            this.tabPageModuleView.Name = "tabPageModuleView";
            this.tabPageModuleView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageModuleView.Size = new System.Drawing.Size(806, 714);
            this.tabPageModuleView.TabIndex = 0;
            this.tabPageModuleView.Text = "Module Output";
            this.tabPageModuleView.UseVisualStyleBackColor = true;
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxImage.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(800, 708);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 0;
            this.pictureBoxImage.TabStop = false;
            // 
            // tabPageSkeleton
            // 
            this.tabPageSkeleton.Location = new System.Drawing.Point(4, 24);
            this.tabPageSkeleton.Name = "tabPageSkeleton";
            this.tabPageSkeleton.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSkeleton.Size = new System.Drawing.Size(806, 714);
            this.tabPageSkeleton.TabIndex = 1;
            this.tabPageSkeleton.Text = "Skeleton View";
            this.tabPageSkeleton.UseVisualStyleBackColor = true;
            // 
            // groupBoxModule
            // 
            this.groupBoxModule.Controls.Add(this.labelModuleLog);
            this.groupBoxModule.Controls.Add(this.richTextBoxModule);
            this.groupBoxModule.Controls.Add(this.button1);
            this.groupBoxModule.Controls.Add(this.buttonStopModule);
            this.groupBoxModule.Controls.Add(this.buttonStartModule);
            this.groupBoxModule.Controls.Add(this.comboBoxModule);
            this.groupBoxModule.Location = new System.Drawing.Point(834, 6);
            this.groupBoxModule.Name = "groupBoxModule";
            this.groupBoxModule.Size = new System.Drawing.Size(349, 371);
            this.groupBoxModule.TabIndex = 0;
            this.groupBoxModule.TabStop = false;
            this.groupBoxModule.Text = "Module";
            // 
            // labelModuleLog
            // 
            this.labelModuleLog.AutoSize = true;
            this.labelModuleLog.Location = new System.Drawing.Point(6, 77);
            this.labelModuleLog.Name = "labelModuleLog";
            this.labelModuleLog.Size = new System.Drawing.Size(33, 15);
            this.labelModuleLog.TabIndex = 4;
            this.labelModuleLog.Text = "Log :";
            // 
            // richTextBoxModule
            // 
            this.richTextBoxModule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxModule.Location = new System.Drawing.Point(6, 95);
            this.richTextBoxModule.Name = "richTextBoxModule";
            this.richTextBoxModule.ReadOnly = true;
            this.richTextBoxModule.Size = new System.Drawing.Size(337, 270);
            this.richTextBoxModule.TabIndex = 1;
            this.richTextBoxModule.Text = "";
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::PTSC.Ui.Properties.Resources.gear;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Location = new System.Drawing.Point(318, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 25);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // buttonStopModule
            // 
            this.buttonStopModule.Location = new System.Drawing.Point(192, 51);
            this.buttonStopModule.Name = "buttonStopModule";
            this.buttonStopModule.Size = new System.Drawing.Size(119, 23);
            this.buttonStopModule.TabIndex = 2;
            this.buttonStopModule.Text = "Stop";
            this.buttonStopModule.UseVisualStyleBackColor = true;
            this.buttonStopModule.Click += new System.EventHandler(this.buttonStopModule_Click);
            // 
            // buttonStartModule
            // 
            this.buttonStartModule.Location = new System.Drawing.Point(6, 51);
            this.buttonStartModule.Name = "buttonStartModule";
            this.buttonStartModule.Size = new System.Drawing.Size(113, 23);
            this.buttonStartModule.TabIndex = 1;
            this.buttonStartModule.Text = "Start";
            this.buttonStartModule.UseVisualStyleBackColor = true;
            this.buttonStartModule.Click += new System.EventHandler(this.buttonStartModule_Click);
            // 
            // comboBoxModule
            // 
            this.comboBoxModule.FormattingEnabled = true;
            this.comboBoxModule.Location = new System.Drawing.Point(6, 22);
            this.comboBoxModule.Name = "comboBoxModule";
            this.comboBoxModule.Size = new System.Drawing.Size(305, 23);
            this.comboBoxModule.TabIndex = 0;
            // 
            // groupBoxDriver
            // 
            this.groupBoxDriver.Location = new System.Drawing.Point(834, 377);
            this.groupBoxDriver.Name = "groupBoxDriver";
            this.groupBoxDriver.Size = new System.Drawing.Size(351, 393);
            this.groupBoxDriver.TabIndex = 1;
            this.groupBoxDriver.TabStop = false;
            this.groupBoxDriver.Text = "VR Driver";
            // 
            // SettingsTab
            // 
            this.SettingsTab.Controls.Add(this.numericUpDownFps);
            this.SettingsTab.Controls.Add(this.labelFpsLimit);
            this.SettingsTab.Location = new System.Drawing.Point(4, 24);
            this.SettingsTab.Name = "SettingsTab";
            this.SettingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.SettingsTab.Size = new System.Drawing.Size(1186, 778);
            this.SettingsTab.TabIndex = 1;
            this.SettingsTab.Text = "Settings";
            this.SettingsTab.UseVisualStyleBackColor = true;
            // 
            // labelFpsLimit
            // 
            this.labelFpsLimit.AutoSize = true;
            this.labelFpsLimit.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelFpsLimit.Location = new System.Drawing.Point(8, 12);
            this.labelFpsLimit.Name = "labelFpsLimit";
            this.labelFpsLimit.Size = new System.Drawing.Size(72, 20);
            this.labelFpsLimit.TabIndex = 0;
            this.labelFpsLimit.Text = "FPS Limit:";
            // 
            // numericUpDownFps
            // 
            this.numericUpDownFps.Location = new System.Drawing.Point(86, 12);
            this.numericUpDownFps.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.numericUpDownFps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDownFps.Name = "numericUpDownFps";
            this.numericUpDownFps.Size = new System.Drawing.Size(120, 23);
            this.numericUpDownFps.TabIndex = 1;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 806);
            this.Controls.Add(this.tabControl);
            this.Name = "MainView";
            this.Text = "PTSC";
            this.tabControl.ResumeLayout(false);
            this.ModuleTab.ResumeLayout(false);
            this.groupBoxVisualization.ResumeLayout(false);
            this.tabControlModuleView.ResumeLayout(false);
            this.tabPageModuleView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.groupBoxModule.ResumeLayout(false);
            this.groupBoxModule.PerformLayout();
            this.SettingsTab.ResumeLayout(false);
            this.SettingsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFps)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public TabControl tabControl;
        public TabPage ModuleTab;
        public TabPage SettingsTab;
        public PictureBox pictureBoxImage;
        private GroupBox groupBoxModule;
        public ComboBox comboBoxModule;
        private Button buttonStopModule;
        private Button buttonStartModule;
        public RichTextBox richTextBoxModule;
        private TabPage tabPageModuleView;
        private TabPage tabPageSkeleton;
        public TabControl tabControlModuleView;
        private Button button1;
        private Label labelModuleLog;
        private GroupBox groupBoxDriver;
        private GroupBox groupBoxVisualization;
        private NumericUpDown numericUpDownFps;
        private Label labelFpsLimit;
    }
}