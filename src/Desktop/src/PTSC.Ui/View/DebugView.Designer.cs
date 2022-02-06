namespace PTSC.Ui.View
{
    partial class DebugView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox_Mode = new System.Windows.Forms.ComboBox();
            this.comboBox_Part = new System.Windows.Forms.ComboBox();
            this.textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // comboBox_Mode
            // 
            this.comboBox_Mode.FormattingEnabled = true;
            this.comboBox_Mode.Location = new System.Drawing.Point(12, 12);
            this.comboBox_Mode.Name = "comboBox_Mode";
            this.comboBox_Mode.Size = new System.Drawing.Size(121, 23);
            this.comboBox_Mode.TabIndex = 0;
            // 
            // comboBox_Part
            // 
            this.comboBox_Part.FormattingEnabled = true;
            this.comboBox_Part.Location = new System.Drawing.Point(139, 12);
            this.comboBox_Part.Name = "comboBox_Part";
            this.comboBox_Part.Size = new System.Drawing.Size(437, 23);
            this.comboBox_Part.TabIndex = 1;
            // 
            // textBox
            // 
            this.textBox.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox.Location = new System.Drawing.Point(12, 41);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(564, 419);
            this.textBox.TabIndex = 2;
            this.textBox.Text = "Debug Information will be displayed here";
            // 
            // DebugView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 472);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.comboBox_Part);
            this.Controls.Add(this.comboBox_Mode);
            this.Name = "DebugView";
            this.Text = "DebugView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ComboBox comboBox_Mode;
        public ComboBox comboBox_Part;
        public TextBox textBox;
    }
}