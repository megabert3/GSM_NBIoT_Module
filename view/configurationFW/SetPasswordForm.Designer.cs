namespace GSM_NBIoT_Module.view {
    partial class SetPasswordForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetPasswordForm));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.closeBtn = new System.Windows.Forms.Button();
            this.setPassBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.repeatNewPassTxtBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.newPassTxtBox = new System.Windows.Forms.TextBox();
            this.oldPassTxtBox = new System.Windows.Forms.TextBox();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox3.Controls.Add(this.closeBtn);
            this.groupBox3.Controls.Add(this.setPassBtn);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.repeatNewPassTxtBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.newPassTxtBox);
            this.groupBox3.Controls.Add(this.oldPassTxtBox);
            this.groupBox3.Location = new System.Drawing.Point(15, 22);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(320, 187);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Пароль для конфигурационного файла";
            // 
            // closeBtn
            // 
            this.closeBtn.Location = new System.Drawing.Point(211, 162);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(101, 22);
            this.closeBtn.TabIndex = 19;
            this.closeBtn.Text = "Отмена";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // setPassBtn
            // 
            this.setPassBtn.Location = new System.Drawing.Point(211, 134);
            this.setPassBtn.Name = "setPassBtn";
            this.setPassBtn.Size = new System.Drawing.Size(101, 22);
            this.setPassBtn.TabIndex = 15;
            this.setPassBtn.Text = "Установить";
            this.setPassBtn.UseVisualStyleBackColor = true;
            this.setPassBtn.Click += new System.EventHandler(this.setPassBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 119);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Повторите новый пароль";
            // 
            // repeatNewPassTxtBox
            // 
            this.repeatNewPassTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.repeatNewPassTxtBox.Location = new System.Drawing.Point(18, 134);
            this.repeatNewPassTxtBox.Name = "repeatNewPassTxtBox";
            this.repeatNewPassTxtBox.PasswordChar = '*';
            this.repeatNewPassTxtBox.Size = new System.Drawing.Size(176, 20);
            this.repeatNewPassTxtBox.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Новый пароль";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Старый пароль";
            // 
            // newPassTxtBox
            // 
            this.newPassTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.newPassTxtBox.Location = new System.Drawing.Point(18, 90);
            this.newPassTxtBox.Name = "newPassTxtBox";
            this.newPassTxtBox.PasswordChar = '*';
            this.newPassTxtBox.Size = new System.Drawing.Size(176, 20);
            this.newPassTxtBox.TabIndex = 14;
            // 
            // oldPassTxtBox
            // 
            this.oldPassTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.oldPassTxtBox.Location = new System.Drawing.Point(18, 49);
            this.oldPassTxtBox.Name = "oldPassTxtBox";
            this.oldPassTxtBox.PasswordChar = '*';
            this.oldPassTxtBox.Size = new System.Drawing.Size(176, 20);
            this.oldPassTxtBox.TabIndex = 13;
            // 
            // SetPasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 218);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "SetPasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Установка пароля";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SetPasswordForm_KeyDown);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button setPassBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox repeatNewPassTxtBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox newPassTxtBox;
        private System.Windows.Forms.TextBox oldPassTxtBox;
        private System.Windows.Forms.Button closeBtn;
    }
}