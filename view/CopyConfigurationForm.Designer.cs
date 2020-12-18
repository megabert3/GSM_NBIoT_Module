namespace GSM_NBIoT_Module.view {
    partial class CoppyConfForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoppyConfForm));
            this.nameTxtBx = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.acceptNameBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nameTxtBx
            // 
            this.nameTxtBx.Location = new System.Drawing.Point(15, 42);
            this.nameTxtBx.Name = "nameTxtBx";
            this.nameTxtBx.Size = new System.Drawing.Size(222, 20);
            this.nameTxtBx.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Название новой конфигурации";
            // 
            // acceptNameBtn
            // 
            this.acceptNameBtn.Location = new System.Drawing.Point(15, 82);
            this.acceptNameBtn.Name = "acceptNameBtn";
            this.acceptNameBtn.Size = new System.Drawing.Size(89, 23);
            this.acceptNameBtn.TabIndex = 2;
            this.acceptNameBtn.Text = "Принять";
            this.acceptNameBtn.UseVisualStyleBackColor = true;
            this.acceptNameBtn.Click += new System.EventHandler(this.acceptNameBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(150, 82);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(87, 23);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "Отменить";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // CoppyConfForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 122);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.acceptNameBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameTxtBx);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "CoppyConfForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Копирование конфигурации";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CoppyConfForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameTxtBx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button acceptNameBtn;
        private System.Windows.Forms.Button cancelBtn;
    }
}