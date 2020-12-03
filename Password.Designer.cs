namespace GSM_NBIoT_Module {
    partial class Password {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Password));
            this.label1 = new System.Windows.Forms.Label();
            this.PasswordtxtBx = new System.Windows.Forms.TextBox();
            this.enterPassBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Пароль";
            // 
            // PasswordtxtBx
            // 
            this.PasswordtxtBx.Location = new System.Drawing.Point(72, 43);
            this.PasswordtxtBx.Name = "PasswordtxtBx";
            this.PasswordtxtBx.PasswordChar = '*';
            this.PasswordtxtBx.Size = new System.Drawing.Size(205, 20);
            this.PasswordtxtBx.TabIndex = 1;
            // 
            // enterPassBtn
            // 
            this.enterPassBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.enterPassBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.enterPassBtn.Location = new System.Drawing.Point(111, 88);
            this.enterPassBtn.Name = "enterPassBtn";
            this.enterPassBtn.Size = new System.Drawing.Size(75, 23);
            this.enterPassBtn.TabIndex = 2;
            this.enterPassBtn.Text = "Ввести";
            this.enterPassBtn.UseVisualStyleBackColor = true;
            this.enterPassBtn.Click += new System.EventHandler(this.enterPassBtn_Click);
            // 
            // Password
            // 
            this.AcceptButton = this.enterPassBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 134);
            this.Controls.Add(this.enterPassBtn);
            this.Controls.Add(this.PasswordtxtBx);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Password";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Пароль";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PasswordtxtBx;
        private System.Windows.Forms.Button enterPassBtn;
    }
}