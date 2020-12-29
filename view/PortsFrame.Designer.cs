namespace GSM_NBIoT_Module.view {
    partial class PortsFrame {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PortsFrame));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.standardCmBx = new System.Windows.Forms.ComboBox();
            this.enhancedPortCmBx = new System.Windows.Forms.ComboBox();
            this.refreshComPortsBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Standard";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Enhanced";
            // 
            // acceptBtn
            // 
            this.acceptBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptBtn.Location = new System.Drawing.Point(24, 88);
            this.acceptBtn.Name = "acceptBtn";
            this.acceptBtn.Size = new System.Drawing.Size(75, 23);
            this.acceptBtn.TabIndex = 4;
            this.acceptBtn.Text = "Принять";
            this.acceptBtn.UseVisualStyleBackColor = true;
            this.acceptBtn.Click += new System.EventHandler(this.acceptBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(163, 88);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 5;
            this.cancelBtn.Text = "Отмена";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // standardCmBx
            // 
            this.standardCmBx.FormattingEnabled = true;
            this.standardCmBx.Location = new System.Drawing.Point(88, 22);
            this.standardCmBx.Name = "standardCmBx";
            this.standardCmBx.Size = new System.Drawing.Size(121, 21);
            this.standardCmBx.TabIndex = 6;
            // 
            // enhancedPortCmBx
            // 
            this.enhancedPortCmBx.FormattingEnabled = true;
            this.enhancedPortCmBx.Location = new System.Drawing.Point(88, 48);
            this.enhancedPortCmBx.Name = "enhancedPortCmBx";
            this.enhancedPortCmBx.Size = new System.Drawing.Size(121, 21);
            this.enhancedPortCmBx.TabIndex = 7;
            // 
            // refreshComPortsBtn
            // 
            this.refreshComPortsBtn.Image = global::GSM_NBIoT_Module.Properties.Resources.restart_15px;
            this.refreshComPortsBtn.Location = new System.Drawing.Point(215, 20);
            this.refreshComPortsBtn.Name = "refreshComPortsBtn";
            this.refreshComPortsBtn.Size = new System.Drawing.Size(23, 23);
            this.refreshComPortsBtn.TabIndex = 8;
            this.refreshComPortsBtn.UseVisualStyleBackColor = true;
            this.refreshComPortsBtn.Click += new System.EventHandler(this.refreshComPortsBtn_Click);
            // 
            // PortsFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 120);
            this.Controls.Add(this.refreshComPortsBtn);
            this.Controls.Add(this.enhancedPortCmBx);
            this.Controls.Add(this.standardCmBx);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.acceptBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PortsFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Установка портов ";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PortsFrame_FormClosing);
            this.Load += new System.EventHandler(this.PortsFrame_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button acceptBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.ComboBox standardCmBx;
        private System.Windows.Forms.ComboBox enhancedPortCmBx;
        private System.Windows.Forms.Button refreshComPortsBtn;
    }
}