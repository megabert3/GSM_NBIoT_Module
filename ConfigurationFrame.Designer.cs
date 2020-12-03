namespace GSM_NBIoT_Module {
    partial class ConfigurationFrame {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationFrame));
            this.configurationListView = new System.Windows.Forms.ListView();
            this.configNameClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.targetIdClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.protocol_idClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.indexClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MCLmodeClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.portClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.domenNameClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fwForMK = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fwForQuectel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.target_IDtxtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.protocol_idTxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.editConfigurationBtn = new System.Windows.Forms.Button();
            this.pathToFW_QuectelBtn = new System.Windows.Forms.Button();
            this.pathToFW_MKBtn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.pathToFW_QuectelTxtBx = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pathToFW_MKtxtBx = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ConfigNameTxtBx = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.setPassBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.repeatNewPassTxtBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.newPassTxtBox = new System.Windows.Forms.TextBox();
            this.oldPassTxtBox = new System.Windows.Forms.TextBox();
            this.deleteConfigurationBtn = new System.Windows.Forms.Button();
            this.addConfigurationBtn = new System.Windows.Forms.Button();
            this.MCL_chkBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.portTxtBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.domenNameTxtBox = new System.Windows.Forms.TextBox();
            this.IPv4rdBtn = new System.Windows.Forms.RadioButton();
            this.domenNameRdBtn = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.indexTxtBox = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // configurationListView
            // 
            this.configurationListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configurationListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.configNameClm,
            this.targetIdClm,
            this.protocol_idClm,
            this.indexClm,
            this.MCLmodeClm,
            this.portClm,
            this.domenNameClm,
            this.fwForMK,
            this.fwForQuectel});
            this.configurationListView.FullRowSelect = true;
            this.configurationListView.GridLines = true;
            this.configurationListView.HideSelection = false;
            this.configurationListView.Location = new System.Drawing.Point(12, 23);
            this.configurationListView.Name = "configurationListView";
            this.configurationListView.Size = new System.Drawing.Size(1099, 471);
            this.configurationListView.TabIndex = 0;
            this.configurationListView.UseCompatibleStateImageBehavior = false;
            this.configurationListView.View = System.Windows.Forms.View.Details;
            // 
            // configNameClm
            // 
            this.configNameClm.Text = "Имя конфигурации";
            this.configNameClm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.configNameClm.Width = 192;
            // 
            // targetIdClm
            // 
            this.targetIdClm.Text = "Target_ID";
            this.targetIdClm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.targetIdClm.Width = 70;
            // 
            // protocol_idClm
            // 
            this.protocol_idClm.Text = "Protocol_ID";
            this.protocol_idClm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.protocol_idClm.Width = 80;
            // 
            // indexClm
            // 
            this.indexClm.Text = "Index";
            this.indexClm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.indexClm.Width = 70;
            // 
            // MCLmodeClm
            // 
            this.MCLmodeClm.Text = "MCL Mode";
            this.MCLmodeClm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MCLmodeClm.Width = 70;
            // 
            // portClm
            // 
            this.portClm.Text = "Порт";
            this.portClm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.portClm.Width = 80;
            // 
            // domenNameClm
            // 
            this.domenNameClm.Text = "Доменное имя или IP адрес";
            this.domenNameClm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.domenNameClm.Width = 166;
            // 
            // fwForMK
            // 
            this.fwForMK.Text = "Прошивка для МК";
            this.fwForMK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.fwForMK.Width = 125;
            // 
            // fwForQuectel
            // 
            this.fwForQuectel.Text = "Прошивка для Quectel";
            this.fwForQuectel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.fwForQuectel.Width = 156;
            // 
            // target_IDtxtBox
            // 
            this.target_IDtxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.target_IDtxtBox.Location = new System.Drawing.Point(6, 85);
            this.target_IDtxtBox.Name = "target_IDtxtBox";
            this.target_IDtxtBox.Size = new System.Drawing.Size(115, 20);
            this.target_IDtxtBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Target_ID";
            // 
            // protocol_idTxtBox
            // 
            this.protocol_idTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.protocol_idTxtBox.Location = new System.Drawing.Point(6, 124);
            this.protocol_idTxtBox.Name = "protocol_idTxtBox";
            this.protocol_idTxtBox.Size = new System.Drawing.Size(115, 20);
            this.protocol_idTxtBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Protocol_ID";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.editConfigurationBtn);
            this.groupBox1.Controls.Add(this.pathToFW_QuectelBtn);
            this.groupBox1.Controls.Add(this.pathToFW_MKBtn);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.pathToFW_QuectelTxtBx);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.pathToFW_MKtxtBx);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.ConfigNameTxtBx);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.deleteConfigurationBtn);
            this.groupBox1.Controls.Add(this.addConfigurationBtn);
            this.groupBox1.Controls.Add(this.MCL_chkBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.portTxtBox);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.indexTxtBox);
            this.groupBox1.Controls.Add(this.target_IDtxtBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.protocol_idTxtBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 500);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1099, 192);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры конфигурации";
            // 
            // editConfigurationBtn
            // 
            this.editConfigurationBtn.Location = new System.Drawing.Point(620, 41);
            this.editConfigurationBtn.Name = "editConfigurationBtn";
            this.editConfigurationBtn.Size = new System.Drawing.Size(115, 25);
            this.editConfigurationBtn.TabIndex = 23;
            this.editConfigurationBtn.Text = "Редактировать";
            this.editConfigurationBtn.UseVisualStyleBackColor = true;
            this.editConfigurationBtn.Click += new System.EventHandler(this.editConfigurationBtn_Click);
            // 
            // pathToFW_QuectelBtn
            // 
            this.pathToFW_QuectelBtn.Location = new System.Drawing.Point(705, 164);
            this.pathToFW_QuectelBtn.Name = "pathToFW_QuectelBtn";
            this.pathToFW_QuectelBtn.Size = new System.Drawing.Size(30, 20);
            this.pathToFW_QuectelBtn.TabIndex = 22;
            this.pathToFW_QuectelBtn.Text = "...";
            this.pathToFW_QuectelBtn.UseVisualStyleBackColor = true;
            this.pathToFW_QuectelBtn.Click += new System.EventHandler(this.pathToFW_QuectelBtn_Click);
            // 
            // pathToFW_MKBtn
            // 
            this.pathToFW_MKBtn.Location = new System.Drawing.Point(705, 126);
            this.pathToFW_MKBtn.Name = "pathToFW_MKBtn";
            this.pathToFW_MKBtn.Size = new System.Drawing.Size(30, 20);
            this.pathToFW_MKBtn.TabIndex = 21;
            this.pathToFW_MKBtn.Text = "...";
            this.pathToFW_MKBtn.UseVisualStyleBackColor = true;
            this.pathToFW_MKBtn.Click += new System.EventHandler(this.pathToFW_MKBtn_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(485, 151);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(160, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Имя прошивки подуля Quectel";
            // 
            // pathToFW_QuectelTxtBx
            // 
            this.pathToFW_QuectelTxtBx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pathToFW_QuectelTxtBx.Location = new System.Drawing.Point(488, 164);
            this.pathToFW_QuectelTxtBx.Name = "pathToFW_QuectelTxtBx";
            this.pathToFW_QuectelTxtBx.Size = new System.Drawing.Size(211, 20);
            this.pathToFW_QuectelTxtBx.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(485, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(182, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Имя прошивки микроконтроллера";
            // 
            // pathToFW_MKtxtBx
            // 
            this.pathToFW_MKtxtBx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pathToFW_MKtxtBx.Location = new System.Drawing.Point(488, 126);
            this.pathToFW_MKtxtBx.Name = "pathToFW_MKtxtBx";
            this.pathToFW_MKtxtBx.Size = new System.Drawing.Size(211, 20);
            this.pathToFW_MKtxtBx.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Название конфигурации";
            // 
            // ConfigNameTxtBx
            // 
            this.ConfigNameTxtBx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ConfigNameTxtBx.Location = new System.Drawing.Point(6, 41);
            this.ConfigNameTxtBx.Name = "ConfigNameTxtBx";
            this.ConfigNameTxtBx.Size = new System.Drawing.Size(474, 20);
            this.ConfigNameTxtBx.TabIndex = 15;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.setPassBtn);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.repeatNewPassTxtBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.newPassTxtBox);
            this.groupBox3.Controls.Add(this.oldPassTxtBox);
            this.groupBox3.Location = new System.Drawing.Point(779, 44);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(320, 148);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Пароль";
            // 
            // setPassBtn
            // 
            this.setPassBtn.Location = new System.Drawing.Point(213, 112);
            this.setPassBtn.Name = "setPassBtn";
            this.setPassBtn.Size = new System.Drawing.Size(101, 23);
            this.setPassBtn.TabIndex = 15;
            this.setPassBtn.Text = "Установить";
            this.setPassBtn.UseVisualStyleBackColor = true;
            this.setPassBtn.Click += new System.EventHandler(this.setPassBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Повторите новый пароль";
            // 
            // repeatNewPassTxtBox
            // 
            this.repeatNewPassTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.repeatNewPassTxtBox.Location = new System.Drawing.Point(20, 115);
            this.repeatNewPassTxtBox.Name = "repeatNewPassTxtBox";
            this.repeatNewPassTxtBox.PasswordChar = '*';
            this.repeatNewPassTxtBox.Size = new System.Drawing.Size(176, 20);
            this.repeatNewPassTxtBox.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Новый пароль";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Старый пароль";
            // 
            // newPassTxtBox
            // 
            this.newPassTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.newPassTxtBox.Location = new System.Drawing.Point(20, 71);
            this.newPassTxtBox.Name = "newPassTxtBox";
            this.newPassTxtBox.PasswordChar = '*';
            this.newPassTxtBox.Size = new System.Drawing.Size(176, 20);
            this.newPassTxtBox.TabIndex = 14;
            // 
            // oldPassTxtBox
            // 
            this.oldPassTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.oldPassTxtBox.Location = new System.Drawing.Point(20, 30);
            this.oldPassTxtBox.Name = "oldPassTxtBox";
            this.oldPassTxtBox.PasswordChar = '*';
            this.oldPassTxtBox.Size = new System.Drawing.Size(176, 20);
            this.oldPassTxtBox.TabIndex = 13;
            // 
            // deleteConfigurationBtn
            // 
            this.deleteConfigurationBtn.Location = new System.Drawing.Point(620, 77);
            this.deleteConfigurationBtn.Name = "deleteConfigurationBtn";
            this.deleteConfigurationBtn.Size = new System.Drawing.Size(115, 25);
            this.deleteConfigurationBtn.TabIndex = 12;
            this.deleteConfigurationBtn.Text = "Удалить";
            this.deleteConfigurationBtn.UseVisualStyleBackColor = true;
            this.deleteConfigurationBtn.Click += new System.EventHandler(this.deleteConfigurationBtn_Click);
            // 
            // addConfigurationBtn
            // 
            this.addConfigurationBtn.Location = new System.Drawing.Point(486, 40);
            this.addConfigurationBtn.Name = "addConfigurationBtn";
            this.addConfigurationBtn.Size = new System.Drawing.Size(115, 25);
            this.addConfigurationBtn.TabIndex = 11;
            this.addConfigurationBtn.Text = "Добавить";
            this.addConfigurationBtn.UseVisualStyleBackColor = true;
            this.addConfigurationBtn.Click += new System.EventHandler(this.addConfigurationBtn_Click);
            // 
            // MCL_chkBox
            // 
            this.MCL_chkBox.AutoSize = true;
            this.MCL_chkBox.Location = new System.Drawing.Point(275, 88);
            this.MCL_chkBox.Name = "MCL_chkBox";
            this.MCL_chkBox.Size = new System.Drawing.Size(78, 17);
            this.MCL_chkBox.TabIndex = 10;
            this.MCL_chkBox.Text = "MCL Mode";
            this.MCL_chkBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(140, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Порт";
            // 
            // portTxtBox
            // 
            this.portTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.portTxtBox.Location = new System.Drawing.Point(143, 86);
            this.portTxtBox.Name = "portTxtBox";
            this.portTxtBox.Size = new System.Drawing.Size(115, 20);
            this.portTxtBox.TabIndex = 8;
            this.portTxtBox.Text = "8103";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.domenNameTxtBox);
            this.groupBox2.Controls.Add(this.IPv4rdBtn);
            this.groupBox2.Controls.Add(this.domenNameRdBtn);
            this.groupBox2.Location = new System.Drawing.Point(143, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(328, 71);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
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
            // IPv4rdBtn
            // 
            this.IPv4rdBtn.AutoSize = true;
            this.IPv4rdBtn.Location = new System.Drawing.Point(219, 15);
            this.IPv4rdBtn.Name = "IPv4rdBtn";
            this.IPv4rdBtn.Size = new System.Drawing.Size(80, 17);
            this.IPv4rdBtn.TabIndex = 1;
            this.IPv4rdBtn.Text = "IPv4 адрес";
            this.IPv4rdBtn.UseVisualStyleBackColor = true;
            this.IPv4rdBtn.CheckedChanged += new System.EventHandler(this.IPv4rdBtn_CheckedChanged);
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
            this.domenNameRdBtn.CheckedChanged += new System.EventHandler(this.domenNameRdBtn_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Index";
            // 
            // indexTxtBox
            // 
            this.indexTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.indexTxtBox.Location = new System.Drawing.Point(6, 163);
            this.indexTxtBox.Name = "indexTxtBox";
            this.indexTxtBox.Size = new System.Drawing.Size(115, 20);
            this.indexTxtBox.TabIndex = 5;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // ConfigurationFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1123, 695);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.configurationListView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ConfigurationFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Конфигурация";
            this.Load += new System.EventHandler(this.ConfigurationFrame_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView configurationListView;
        private System.Windows.Forms.TextBox target_IDtxtBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox protocol_idTxtBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox indexTxtBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox portTxtBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton IPv4rdBtn;
        private System.Windows.Forms.RadioButton domenNameRdBtn;
        private System.Windows.Forms.TextBox domenNameTxtBox;
        private System.Windows.Forms.CheckBox MCL_chkBox;
        private System.Windows.Forms.Button addConfigurationBtn;
        private System.Windows.Forms.Button deleteConfigurationBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox oldPassTxtBox;
        private System.Windows.Forms.TextBox newPassTxtBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox repeatNewPassTxtBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button setPassBtn;
        private System.Windows.Forms.ColumnHeader targetIdClm;
        private System.Windows.Forms.TextBox ConfigNameTxtBx;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ColumnHeader configNameClm;
        private System.Windows.Forms.ColumnHeader protocol_idClm;
        private System.Windows.Forms.ColumnHeader indexClm;
        private System.Windows.Forms.ColumnHeader portClm;
        private System.Windows.Forms.ColumnHeader MCLmodeClm;
        private System.Windows.Forms.ColumnHeader domenNameClm;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox pathToFW_MKtxtBx;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox pathToFW_QuectelTxtBx;
        private System.Windows.Forms.Button pathToFW_MKBtn;
        private System.Windows.Forms.Button pathToFW_QuectelBtn;
        private System.Windows.Forms.ColumnHeader fwForMK;
        private System.Windows.Forms.ColumnHeader fwForQuectel;
        private System.Windows.Forms.Button editConfigurationBtn;
    }
}