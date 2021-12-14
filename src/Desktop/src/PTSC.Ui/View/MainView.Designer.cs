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
            this.splitContainerModule = new System.Windows.Forms.SplitContainer();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.SettingsTab = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.ModuleTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerModule)).BeginInit();
            this.splitContainerModule.Panel1.SuspendLayout();
            this.splitContainerModule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
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
            this.ModuleTab.Controls.Add(this.splitContainerModule);
            this.ModuleTab.Location = new System.Drawing.Point(4, 24);
            this.ModuleTab.Name = "ModuleTab";
            this.ModuleTab.Padding = new System.Windows.Forms.Padding(3);
            this.ModuleTab.Size = new System.Drawing.Size(1186, 778);
            this.ModuleTab.TabIndex = 0;
            this.ModuleTab.Text = "Module";
            this.ModuleTab.UseVisualStyleBackColor = true;
            // 
            // splitContainerModule
            // 
            this.splitContainerModule.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerModule.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainerModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerModule.Location = new System.Drawing.Point(3, 3);
            this.splitContainerModule.Name = "splitContainerModule";
            // 
            // splitContainerModule.Panel1
            // 
            this.splitContainerModule.Panel1.Controls.Add(this.pictureBoxImage);
            this.splitContainerModule.Size = new System.Drawing.Size(1180, 772);
            this.splitContainerModule.SplitterDistance = 817;
            this.splitContainerModule.TabIndex = 0;
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxImage.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(813, 768);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 0;
            this.pictureBoxImage.TabStop = false;
            // 
            // SettingsTab
            // 
            this.SettingsTab.Location = new System.Drawing.Point(4, 24);
            this.SettingsTab.Name = "SettingsTab";
            this.SettingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.SettingsTab.Size = new System.Drawing.Size(1186, 778);
            this.SettingsTab.TabIndex = 1;
            this.SettingsTab.Text = "Settings";
            this.SettingsTab.UseVisualStyleBackColor = true;
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
            this.splitContainerModule.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerModule)).EndInit();
            this.splitContainerModule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public TabControl tabControl;
        public TabPage ModuleTab;
        public TabPage SettingsTab;
        public SplitContainer splitContainerModule;
        public PictureBox pictureBoxImage;
    }
}