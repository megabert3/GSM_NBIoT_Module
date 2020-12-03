namespace GSM_NBIoT_Module
{
    partial class EditConfigurationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditConfigurationForm));
            this.pathToFW_QuectelBtn = new System.Windows.Forms.Button();
            this.pathToFW_MKBtn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.pathToFW_QuectelTxtBx = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pathToFW_MKtxtBx = new System.Windows.Forms.TextBox();
            this.domenNameRdBtn = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.domenNameTxtBox = new System.Windows.Forms.TextBox();
            this.ConfigNameTxtBx = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.portTxtBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.IPv4rdBtn = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.indexTxtBox = new System.Windows.Forms.TextBox();
            this.target_IDtxtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.protocol_idTxtBox = new System.Windows.Forms.TextBox();
            this.MCL_chkBox = new System.Windows.Forms.CheckBox();
            this.saveEditsBtn = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pathToFW_QuectelBtn
            // 
            this.pathToFW_QuectelBtn.Location = new System.Drawing.Point(723, 67);
            this.pathToFW_QuectelBtn.Name = "pathToFW_QuectelBtn";
            this.pathToFW_QuectelBtn.Size = new System.Drawing.Size(30, 20);
            this.pathToFW_QuectelBtn.TabIndex = 40;
            this.pathToFW_QuectelBtn.Text = "...";
            this.pathToFW_QuectelBtn.UseVisualStyleBackColor = true;
            this.pathToFW_QuectelBtn.Click += new System.EventHandler(this.pathToFW_QuectelBtn_Click);
            // 
            // pathToFW_MKBtn
            // 
            this.pathToFW_MKBtn.Location = new System.Drawing.Point(723, 29);
            this.pathToFW_MKBtn.Name = "pathToFW_MKBtn";
            this.pathToFW_MKBtn.Size = new System.Drawing.Size(30, 20);
            this.pathToFW_MKBtn.TabIndex = 39;
            this.pathToFW_MKBtn.Text = "...";
            this.pathToFW_MKBtn.UseVisualStyleBackColor = true;
            this.pathToFW_MKBtn.Click += new System.EventHandler(this.pathToFW_MKBtn_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(503, 54);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(160, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Имя прошивки подуля Quectel";
            // 
            // pathToFW_QuectelTxtBx
            // 
            this.pathToFW_QuectelTxtBx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pathToFW_QuectelTxtBx.Location = new System.Drawing.Point(506, 67);
            this.pathToFW_QuectelTxtBx.Name = "pathToFW_QuectelTxtBx";
            this.pathToFW_QuectelTxtBx.Size = new System.Drawing.Size(211, 20);
            this.pathToFW_QuectelTxtBx.TabIndex = 37;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(503, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(182, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Имя прошивки микроконтроллера";
            // 
            // pathToFW_MKtxtBx
            // 
            this.pathToFW_MKtxtBx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pathToFW_MKtxtBx.Location = new System.Drawing.Point(506, 29);
            this.pathToFW_MKtxtBx.Name = "pathToFW_MKtxtBx";
            this.pathToFW_MKtxtBx.Size = new System.Drawing.Size(211, 20);
            this.pathToFW_MKtxtBx.TabIndex = 35;
            // 
            // domenNameRdBtn
            // 
            this.domenNameRdBtn.AutoSize = true;
            this.domenNameRdBtn.Checked = true;
            this.domenNameRdBtn.Location = new System.Drawing.Point(17, 15);
            this.domenNameRdBtn.Name = "domenNameRdBtn";
            this.domenNameRdBtn.Size = new System.Drawing.Size(101, 17);
            this.domenNameRdBtn.TabIndex = 0;
            this.domenNameRdBtn.TabStop = true;
            this.domenNameRdBtn.Text = "Доменное имя";
            this.domenNameRdBtn.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Название конфигурации";
            // 
            // domenNameTxtBox
            // 
            this.domenNameTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.domenNameTxtBox.Location = new System.Drawing.Point(17, 45);
            this.domenNameTxtBox.Name = "domenNameTxtBox";
            this.domenNameTxtBox.Size = new System.Drawing.Size(288, 20);
            this.domenNameTxtBox.TabIndex = 10;
            this.domenNameTxtBox.Text = "devices.226.taipit.ru";
            // 
            // ConfigNameTxtBx
            // 
            this.ConfigNameTxtBx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ConfigNameTxtBx.Location = new System.Drawing.Point(13, 29);
            this.ConfigNameTxtBx.Name = "ConfigNameTxtBx";
            this.ConfigNameTxtBx.Size = new System.Drawing.Size(474, 20);
            this.ConfigNameTxtBx.TabIndex = 33;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(147, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Порт";
            // 
            // portTxtBox
            // 
            this.portTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.portTxtBox.Location = new System.Drawing.Point(150, 74);
            this.portTxtBox.Name = "portTxtBox";
            this.portTxtBox.Size = new System.Drawing.Size(115, 20);
            this.portTxtBox.TabIndex = 30;
            this.portTxtBox.Text = "8103";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.domenNameTxtBox);
            this.groupBox2.Controls.Add(this.IPv4rdBtn);
            this.groupBox2.Controls.Add(this.domenNameRdBtn);
            this.groupBox2.Location = new System.Drawing.Point(150, 103);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(328, 71);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            // 
            // IPv4rdBtn
            // 
            this.IPv4rdBtn.AutoSize = true;
            this.IPv4rdBtn.Location = new System.Drawing.Point(219, 15);
            this.IPv4rdBtn.Name = "IPv4rdBtn";
            this.IPv4rdBtn.Size = new System.Drawing.Size(80, 17);
            this.IPv4rdBtn.TabIndex = 1;
            this.IPv4rdBtn.Text = "IPv4 адрес";
            this.IPv4rdBtn.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Index";
            // 
            // indexTxtBox
            // 
            this.indexTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.indexTxtBox.Location = new System.Drawing.Point(12, 151);
            this.indexTxtBox.Name = "indexTxtBox";
            this.indexTxtBox.Size = new System.Drawing.Size(115, 20);
            this.indexTxtBox.TabIndex = 27;
            // 
            // target_IDtxtBox
            // 
            this.target_IDtxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.target_IDtxtBox.Location = new System.Drawing.Point(13, 73);
            this.target_IDtxtBox.Name = "target_IDtxtBox";
            this.target_IDtxtBox.Size = new System.Drawing.Size(115, 20);
            this.target_IDtxtBox.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Target_ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Protocol_ID";
            // 
            // protocol_idTxtBox
            // 
            this.protocol_idTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.protocol_idTxtBox.Location = new System.Drawing.Point(13, 112);
            this.protocol_idTxtBox.Name = "protocol_idTxtBox";
            this.protocol_idTxtBox.Size = new System.Drawing.Size(115, 20);
            this.protocol_idTxtBox.TabIndex = 25;
            // 
            // MCL_chkBox
            // 
            this.MCL_chkBox.AutoSize = true;
            this.MCL_chkBox.Location = new System.Drawing.Point(282, 76);
            this.MCL_chkBox.Name = "MCL_chkBox";
            this.MCL_chkBox.Size = new System.Drawing.Size(78, 17);
            this.MCL_chkBox.TabIndex = 32;
            this.MCL_chkBox.Text = "MCL Mode";
            this.MCL_chkBox.UseVisualStyleBackColor = true;
            // 
            // saveEditsBtn
            // 
            this.saveEditsBtn.Location = new System.Drawing.Point(638, 151);
            this.saveEditsBtn.Name = "saveEditsBtn";
            this.saveEditsBtn.Size = new System.Drawing.Size(115, 25);
            this.saveEditsBtn.TabIndex = 41;
            this.saveEditsBtn.Text = "Сохранить";
            this.saveEditsBtn.UseVisualStyleBackColor = true;
            this.saveEditsBtn.Click += new System.EventHandler(this.saveEditsBtn_Click);
            // 
            // EditConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 188);
            this.Controls.Add(this.saveEditsBtn);
            this.Controls.Add(this.pathToFW_QuectelBtn);
            this.Controls.Add(this.pathToFW_MKBtn);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.pathToFW_QuectelTxtBx);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.pathToFW_MKtxtBx);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ConfigNameTxtBx);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.portTxtBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.indexTxtBox);
            this.Controls.Add(this.target_IDtxtBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.protocol_idTxtBox);
            this.Controls.Add(this.MCL_chkBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditConfigurationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование конфигурации";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button pathToFW_QuectelBtn;
        private System.Windows.Forms.Button pathToFW_MKBtn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox pathToFW_QuectelTxtBx;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox pathToFW_MKtxtBx;
        private System.Windows.Forms.RadioButton domenNameRdBtn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox domenNameTxtBox;
        private System.Windows.Forms.TextBox ConfigNameTxtBx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox portTxtBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton IPv4rdBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox indexTxtBox;
        private System.Windows.Forms.TextBox target_IDtxtBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox protocol_idTxtBox;
        private System.Windows.Forms.CheckBox MCL_chkBox;
        private System.Windows.Forms.Button saveEditsBtn;
    }
}