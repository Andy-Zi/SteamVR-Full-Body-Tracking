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
            this.groupBoxVisualization = new System.Windows.Forms.GroupBox();
            this.tabControlModuleView = new System.Windows.Forms.TabControl();
            this.tabPageModuleView = new System.Windows.Forms.TabPage();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.tabPageSkeleton = new System.Windows.Forms.TabPage();
            this.groupBoxModule = new System.Windows.Forms.GroupBox();
            this.labelModuleLog = new System.Windows.Forms.Label();
            this.richTextBoxModule = new System.Windows.Forms.RichTextBox();
            this.button_moduleSettings = new System.Windows.Forms.Button();
            this.buttonStopModule = new System.Windows.Forms.Button();
            this.buttonStartModule = new System.Windows.Forms.Button();
            this.comboBoxModule = new System.Windows.Forms.ComboBox();
            this.groupBoxDriver = new System.Windows.Forms.GroupBox();
            this.panelModuleContainer = new System.Windows.Forms.Panel();
            this.panelPipelineContainer = new System.Windows.Forms.Panel();
            this.labelModuleStateValue = new System.Windows.Forms.Label();
            this.labelDriverStateValue = new System.Windows.Forms.Label();
            this.labelModuleLatency = new System.Windows.Forms.Label();
            this.labelPipelineLatency = new System.Windows.Forms.Label();
            this.labelModuleState = new System.Windows.Forms.Label();
            this.labelDriverState = new System.Windows.Forms.Label();
            this.labelApplicationSettings = new System.Windows.Forms.Label();
            this.buttonApplicationSettings = new System.Windows.Forms.Button();
            this.groupBoxVisualization.SuspendLayout();
            this.tabControlModuleView.SuspendLayout();
            this.tabPageModuleView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.groupBoxModule.SuspendLayout();
            this.groupBoxDriver.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxVisualization
            // 
            this.groupBoxVisualization.Controls.Add(this.tabControlModuleView);
            this.groupBoxVisualization.Location = new System.Drawing.Point(2, 0);
            this.groupBoxVisualization.Name = "groupBoxVisualization";
            this.groupBoxVisualization.Size = new System.Drawing.Size(857, 806);
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
            this.tabControlModuleView.Size = new System.Drawing.Size(851, 784);
            this.tabControlModuleView.TabIndex = 1;
            // 
            // tabPageModuleView
            // 
            this.tabPageModuleView.Controls.Add(this.pictureBoxImage);
            this.tabPageModuleView.Location = new System.Drawing.Point(4, 24);
            this.tabPageModuleView.Name = "tabPageModuleView";
            this.tabPageModuleView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageModuleView.Size = new System.Drawing.Size(843, 756);
            this.tabPageModuleView.TabIndex = 0;
            this.tabPageModuleView.Text = "Module Output";
            this.tabPageModuleView.UseVisualStyleBackColor = true;
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxImage.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(837, 750);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 0;
            this.pictureBoxImage.TabStop = false;
            // 
            // tabPageSkeleton
            // 
            this.tabPageSkeleton.Location = new System.Drawing.Point(4, 24);
            this.tabPageSkeleton.Name = "tabPageSkeleton";
            this.tabPageSkeleton.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSkeleton.Size = new System.Drawing.Size(843, 756);
            this.tabPageSkeleton.TabIndex = 1;
            this.tabPageSkeleton.Text = "Skeleton View";
            this.tabPageSkeleton.UseVisualStyleBackColor = true;
            // 
            // groupBoxModule
            // 
            this.groupBoxModule.Controls.Add(this.labelModuleLog);
            this.groupBoxModule.Controls.Add(this.richTextBoxModule);
            this.groupBoxModule.Controls.Add(this.button_moduleSettings);
            this.groupBoxModule.Controls.Add(this.buttonStopModule);
            this.groupBoxModule.Controls.Add(this.buttonStartModule);
            this.groupBoxModule.Controls.Add(this.comboBoxModule);
            this.groupBoxModule.Location = new System.Drawing.Point(865, 0);
            this.groupBoxModule.Name = "groupBoxModule";
            this.groupBoxModule.Size = new System.Drawing.Size(349, 288);
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
            this.richTextBoxModule.Size = new System.Drawing.Size(337, 187);
            this.richTextBoxModule.TabIndex = 1;
            this.richTextBoxModule.Text = "";
            // 
            // button_moduleSettings
            // 
            this.button_moduleSettings.BackgroundImage = global::PTSC.Ui.Properties.Resources.gear;
            this.button_moduleSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_moduleSettings.Location = new System.Drawing.Point(318, 21);
            this.button_moduleSettings.Name = "button_moduleSettings";
            this.button_moduleSettings.Size = new System.Drawing.Size(25, 25);
            this.button_moduleSettings.TabIndex = 3;
            this.button_moduleSettings.UseVisualStyleBackColor = true;
            this.button_moduleSettings.Click += new System.EventHandler(this.button_moduleSettings_Click);
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
            this.groupBoxDriver.Controls.Add(this.panelModuleContainer);
            this.groupBoxDriver.Controls.Add(this.panelPipelineContainer);
            this.groupBoxDriver.Controls.Add(this.labelModuleStateValue);
            this.groupBoxDriver.Controls.Add(this.labelDriverStateValue);
            this.groupBoxDriver.Controls.Add(this.labelModuleLatency);
            this.groupBoxDriver.Controls.Add(this.labelPipelineLatency);
            this.groupBoxDriver.Controls.Add(this.labelModuleState);
            this.groupBoxDriver.Controls.Add(this.labelDriverState);
            this.groupBoxDriver.Location = new System.Drawing.Point(865, 294);
            this.groupBoxDriver.Name = "groupBoxDriver";
            this.groupBoxDriver.Size = new System.Drawing.Size(349, 463);
            this.groupBoxDriver.TabIndex = 1;
            this.groupBoxDriver.TabStop = false;
            this.groupBoxDriver.Text = "VR Driver";
            // 
            // panelModuleContainer
            // 
            this.panelModuleContainer.Location = new System.Drawing.Point(6, 305);
            this.panelModuleContainer.Name = "panelModuleContainer";
            this.panelModuleContainer.Size = new System.Drawing.Size(337, 152);
            this.panelModuleContainer.TabIndex = 7;
            // 
            // panelPipelineContainer
            // 
            this.panelPipelineContainer.Location = new System.Drawing.Point(6, 119);
            this.panelPipelineContainer.Name = "panelPipelineContainer";
            this.panelPipelineContainer.Size = new System.Drawing.Size(337, 159);
            this.panelPipelineContainer.TabIndex = 6;
            // 
            // labelModuleStateValue
            // 
            this.labelModuleStateValue.AutoSize = true;
            this.labelModuleStateValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelModuleStateValue.Location = new System.Drawing.Point(120, 53);
            this.labelModuleStateValue.Name = "labelModuleStateValue";
            this.labelModuleStateValue.Size = new System.Drawing.Size(103, 21);
            this.labelModuleStateValue.TabIndex = 5;
            this.labelModuleStateValue.Text = "Disconnected";
            // 
            // labelDriverStateValue
            // 
            this.labelDriverStateValue.AutoSize = true;
            this.labelDriverStateValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDriverStateValue.Location = new System.Drawing.Point(120, 19);
            this.labelDriverStateValue.Name = "labelDriverStateValue";
            this.labelDriverStateValue.Size = new System.Drawing.Size(103, 21);
            this.labelDriverStateValue.TabIndex = 4;
            this.labelDriverStateValue.Text = "Disconnected";
            // 
            // labelModuleLatency
            // 
            this.labelModuleLatency.AutoSize = true;
            this.labelModuleLatency.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelModuleLatency.Location = new System.Drawing.Point(6, 281);
            this.labelModuleLatency.Name = "labelModuleLatency";
            this.labelModuleLatency.Size = new System.Drawing.Size(127, 21);
            this.labelModuleLatency.TabIndex = 3;
            this.labelModuleLatency.Text = "Module Latency :";
            // 
            // labelPipelineLatency
            // 
            this.labelPipelineLatency.AutoSize = true;
            this.labelPipelineLatency.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelPipelineLatency.Location = new System.Drawing.Point(6, 93);
            this.labelPipelineLatency.Name = "labelPipelineLatency";
            this.labelPipelineLatency.Size = new System.Drawing.Size(129, 21);
            this.labelPipelineLatency.TabIndex = 2;
            this.labelPipelineLatency.Text = "Pipeline Latency :";
            // 
            // labelModuleState
            // 
            this.labelModuleState.AutoSize = true;
            this.labelModuleState.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelModuleState.Location = new System.Drawing.Point(6, 53);
            this.labelModuleState.Name = "labelModuleState";
            this.labelModuleState.Size = new System.Drawing.Size(108, 21);
            this.labelModuleState.TabIndex = 1;
            this.labelModuleState.Text = "Module State :";
            // 
            // labelDriverState
            // 
            this.labelDriverState.AutoSize = true;
            this.labelDriverState.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDriverState.Location = new System.Drawing.Point(6, 19);
            this.labelDriverState.Name = "labelDriverState";
            this.labelDriverState.Size = new System.Drawing.Size(98, 21);
            this.labelDriverState.TabIndex = 0;
            this.labelDriverState.Text = "Driver State :";
            // 
            // labelApplicationSettings
            // 
            this.labelApplicationSettings.AutoSize = true;
            this.labelApplicationSettings.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelApplicationSettings.Location = new System.Drawing.Point(1010, 768);
            this.labelApplicationSettings.Name = "labelApplicationSettings";
            this.labelApplicationSettings.Size = new System.Drawing.Size(159, 21);
            this.labelApplicationSettings.TabIndex = 3;
            this.labelApplicationSettings.Text = "Application Settings : ";
            // 
            // buttonApplicationSettings
            // 
            this.buttonApplicationSettings.BackgroundImage = global::PTSC.Ui.Properties.Resources.gear;
            this.buttonApplicationSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonApplicationSettings.Location = new System.Drawing.Point(1164, 760);
            this.buttonApplicationSettings.Name = "buttonApplicationSettings";
            this.buttonApplicationSettings.Size = new System.Drawing.Size(44, 40);
            this.buttonApplicationSettings.TabIndex = 5;
            this.buttonApplicationSettings.UseVisualStyleBackColor = true;
            this.buttonApplicationSettings.Click += new System.EventHandler(this.buttonApplicationSettings_Click);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1216, 806);
            this.Controls.Add(this.buttonApplicationSettings);
            this.Controls.Add(this.labelApplicationSettings);
            this.Controls.Add(this.groupBoxVisualization);
            this.Controls.Add(this.groupBoxDriver);
            this.Controls.Add(this.groupBoxModule);
            this.Name = "MainView";
            this.Text = "PTSC";
            this.groupBoxVisualization.ResumeLayout(false);
            this.tabControlModuleView.ResumeLayout(false);
            this.tabPageModuleView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.groupBoxModule.ResumeLayout(false);
            this.groupBoxModule.PerformLayout();
            this.groupBoxDriver.ResumeLayout(false);
            this.groupBoxDriver.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public PictureBox pictureBoxImage;
        private GroupBox groupBoxModule;
        public ComboBox comboBoxModule;
        private Button buttonStopModule;
        private Button buttonStartModule;
        public RichTextBox richTextBoxModule;
        private TabPage tabPageModuleView;
        private TabPage tabPageSkeleton;
        public TabControl tabControlModuleView;
        private Button button_moduleSettings;
        private Label labelModuleLog;
        private GroupBox groupBoxDriver;
        private GroupBox groupBoxVisualization;
        private Label labelApplicationSettings;
        private Button buttonApplicationSettings;
        private Label labelModuleLatency;
        private Label labelPipelineLatency;
        private Label labelModuleState;
        private Label labelDriverState;
        private Panel panelModuleContainer;
        private Panel panelPipelineContainer;
        public Label labelDriverStateValue;
        public Label labelModuleStateValue;
    }
}