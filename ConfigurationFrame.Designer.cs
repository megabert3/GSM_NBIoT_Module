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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.pathToFW_MKtxtBx = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pathToFW_QuectelBtn = new System.Windows.Forms.Button();
            this.pathToFW_QuectelTxtBx = new System.Windows.Forms.TextBox();
            this.pathToFW_MKBtn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.portTxtBox = new System.Windows.Forms.TextBox();
            this.indexTxtBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.domenNameTxtBox = new System.Windows.Forms.TextBox();
            this.IPv4rdBtn = new System.Windows.Forms.RadioButton();
            this.domenNameRdBtn = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.MCL_chkBox = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.addQuectelCommandBtn = new System.Windows.Forms.Button();
            this.quectelCommandTxtBox = new System.Windows.Forms.TextBox();
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
            this.addConfigurationBtn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editConfigurationBtn = new System.Windows.Forms.Button();
            this.deleteConfigurationBtn = new System.Windows.Forms.Button();
            this.quectelCommnadsdtGrdView = new System.Windows.Forms.DataGridView();
            this.qurctelCommandColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quectelCommnadsdtGrdView)).BeginInit();
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
            this.configurationListView.Location = new System.Drawing.Point(12, 12);
            this.configurationListView.Name = "configurationListView";
            this.configurationListView.Size = new System.Drawing.Size(1145, 491);
            this.configurationListView.TabIndex = 0;
            this.configurationListView.UseCompatibleStateImageBehavior = false;
            this.configurationListView.View = System.Windows.Forms.View.Details;
            this.configurationListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.configurationListView_ItemSelectionChanged);
            this.configurationListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.configurationListView_MouseDoubleClick);
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
            this.target_IDtxtBox.Location = new System.Drawing.Point(9, 49);
            this.target_IDtxtBox.Name = "target_IDtxtBox";
            this.target_IDtxtBox.Size = new System.Drawing.Size(115, 20);
            this.target_IDtxtBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Target_ID";
            // 
            // protocol_idTxtBox
            // 
            this.protocol_idTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.protocol_idTxtBox.Location = new System.Drawing.Point(9, 88);
            this.protocol_idTxtBox.Name = "protocol_idTxtBox";
            this.protocol_idTxtBox.Size = new System.Drawing.Size(115, 20);
            this.protocol_idTxtBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Protocol_ID";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.ConfigNameTxtBx);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.addConfigurationBtn);
            this.groupBox1.Location = new System.Drawing.Point(12, 540);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1145, 308);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры конфигурации";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.pathToFW_MKtxtBx);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.pathToFW_QuectelBtn);
            this.groupBox5.Controls.Add(this.pathToFW_QuectelTxtBx);
            this.groupBox5.Controls.Add(this.pathToFW_MKBtn);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Location = new System.Drawing.Point(541, 104);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(269, 110);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Путь к прошивкам";
            // 
            // pathToFW_MKtxtBx
            // 
            this.pathToFW_MKtxtBx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pathToFW_MKtxtBx.ForeColor = System.Drawing.SystemColors.WindowText;
            this.pathToFW_MKtxtBx.Location = new System.Drawing.Point(6, 34);
            this.pathToFW_MKtxtBx.Name = "pathToFW_MKtxtBx";
            this.pathToFW_MKtxtBx.Size = new System.Drawing.Size(211, 20);
            this.pathToFW_MKtxtBx.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(182, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Имя прошивки микроконтроллера";
            // 
            // pathToFW_QuectelBtn
            // 
            this.pathToFW_QuectelBtn.Location = new System.Drawing.Point(223, 72);
            this.pathToFW_QuectelBtn.Name = "pathToFW_QuectelBtn";
            this.pathToFW_QuectelBtn.Size = new System.Drawing.Size(30, 20);
            this.pathToFW_QuectelBtn.TabIndex = 22;
            this.pathToFW_QuectelBtn.Text = "...";
            this.pathToFW_QuectelBtn.UseVisualStyleBackColor = true;
            this.pathToFW_QuectelBtn.Click += new System.EventHandler(this.pathToFW_QuectelBtn_Click);
            // 
            // pathToFW_QuectelTxtBx
            // 
            this.pathToFW_QuectelTxtBx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pathToFW_QuectelTxtBx.Location = new System.Drawing.Point(6, 72);
            this.pathToFW_QuectelTxtBx.Name = "pathToFW_QuectelTxtBx";
            this.pathToFW_QuectelTxtBx.Size = new System.Drawing.Size(211, 20);
            this.pathToFW_QuectelTxtBx.TabIndex = 19;
            // 
            // pathToFW_MKBtn
            // 
            this.pathToFW_MKBtn.Location = new System.Drawing.Point(223, 34);
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
            this.label10.Location = new System.Drawing.Point(3, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(162, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Имя прошивки модуля Quectel";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 72);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(529, 230);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(521, 204);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Конфигурация микроконтроллера";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.portTxtBox);
            this.groupBox4.Controls.Add(this.protocol_idTxtBox);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.target_IDtxtBox);
            this.groupBox4.Controls.Add(this.indexTxtBox);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.groupBox2);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.MCL_chkBox);
            this.groupBox4.Location = new System.Drawing.Point(6, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(509, 186);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Конфигурация микроконтроллера";
            // 
            // portTxtBox
            // 
            this.portTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.portTxtBox.Location = new System.Drawing.Point(139, 48);
            this.portTxtBox.Name = "portTxtBox";
            this.portTxtBox.Size = new System.Drawing.Size(115, 20);
            this.portTxtBox.TabIndex = 8;
            this.portTxtBox.Text = "8103";
            // 
            // indexTxtBox
            // 
            this.indexTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.indexTxtBox.Location = new System.Drawing.Point(9, 127);
            this.indexTxtBox.Name = "indexTxtBox";
            this.indexTxtBox.Size = new System.Drawing.Size(115, 20);
            this.indexTxtBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Index";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.domenNameTxtBox);
            this.groupBox2.Controls.Add(this.IPv4rdBtn);
            this.groupBox2.Controls.Add(this.domenNameRdBtn);
            this.groupBox2.Location = new System.Drawing.Point(139, 78);
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
            this.domenNameTxtBox.Text = "\"devices.226.taipit.ru\"";
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(136, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Порт";
            // 
            // MCL_chkBox
            // 
            this.MCL_chkBox.AutoSize = true;
            this.MCL_chkBox.Location = new System.Drawing.Point(271, 50);
            this.MCL_chkBox.Name = "MCL_chkBox";
            this.MCL_chkBox.Size = new System.Drawing.Size(78, 17);
            this.MCL_chkBox.TabIndex = 10;
            this.MCL_chkBox.Text = "MCL Mode";
            this.MCL_chkBox.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.quectelCommnadsdtGrdView);
            this.tabPage2.Controls.Add(this.addQuectelCommandBtn);
            this.tabPage2.Controls.Add(this.quectelCommandTxtBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(521, 204);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Конфигурация модуля Quectel";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // addQuectelCommandBtn
            // 
            this.addQuectelCommandBtn.Location = new System.Drawing.Point(280, 180);
            this.addQuectelCommandBtn.Name = "addQuectelCommandBtn";
            this.addQuectelCommandBtn.Size = new System.Drawing.Size(97, 22);
            this.addQuectelCommandBtn.TabIndex = 2;
            this.addQuectelCommandBtn.Text = "Добавить";
            this.addQuectelCommandBtn.UseVisualStyleBackColor = true;
            this.addQuectelCommandBtn.Click += new System.EventHandler(this.addQuectelCommandBtn_Click);
            // 
            // quectelCommandTxtBox
            // 
            this.quectelCommandTxtBox.Location = new System.Drawing.Point(6, 180);
            this.quectelCommandTxtBox.Name = "quectelCommandTxtBox";
            this.quectelCommandTxtBox.Size = new System.Drawing.Size(268, 20);
            this.quectelCommandTxtBox.TabIndex = 1;
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
            this.ConfigNameTxtBx.Size = new System.Drawing.Size(525, 20);
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
            this.groupBox3.Location = new System.Drawing.Point(819, 99);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(320, 174);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Пароль для конфигурационного файла";
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
            // addConfigurationBtn
            // 
            this.addConfigurationBtn.Location = new System.Drawing.Point(541, 41);
            this.addConfigurationBtn.Name = "addConfigurationBtn";
            this.addConfigurationBtn.Size = new System.Drawing.Size(115, 22);
            this.addConfigurationBtn.TabIndex = 11;
            this.addConfigurationBtn.Text = "Добавить";
            this.addConfigurationBtn.UseVisualStyleBackColor = true;
            this.addConfigurationBtn.Click += new System.EventHandler(this.addConfigurationBtn_Click);
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
            // editConfigurationBtn
            // 
            this.editConfigurationBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editConfigurationBtn.Enabled = false;
            this.editConfigurationBtn.Location = new System.Drawing.Point(910, 509);
            this.editConfigurationBtn.Name = "editConfigurationBtn";
            this.editConfigurationBtn.Size = new System.Drawing.Size(115, 25);
            this.editConfigurationBtn.TabIndex = 27;
            this.editConfigurationBtn.Text = "Редактировать";
            this.editConfigurationBtn.UseVisualStyleBackColor = true;
            this.editConfigurationBtn.Click += new System.EventHandler(this.editConfigurationBtn_Click);
            // 
            // deleteConfigurationBtn
            // 
            this.deleteConfigurationBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteConfigurationBtn.Enabled = false;
            this.deleteConfigurationBtn.Location = new System.Drawing.Point(1042, 509);
            this.deleteConfigurationBtn.Name = "deleteConfigurationBtn";
            this.deleteConfigurationBtn.Size = new System.Drawing.Size(115, 25);
            this.deleteConfigurationBtn.TabIndex = 26;
            this.deleteConfigurationBtn.Text = "Удалить";
            this.deleteConfigurationBtn.UseVisualStyleBackColor = true;
            this.deleteConfigurationBtn.Click += new System.EventHandler(this.deleteConfigurationBtn_Click);
            // 
            // quectelCommnadsdtGrdView
            // 
            this.quectelCommnadsdtGrdView.AllowUserToAddRows = false;
            this.quectelCommnadsdtGrdView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.quectelCommnadsdtGrdView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.quectelCommnadsdtGrdView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.qurctelCommandColumn});
            this.quectelCommnadsdtGrdView.Location = new System.Drawing.Point(6, 11);
            this.quectelCommnadsdtGrdView.MultiSelect = false;
            this.quectelCommnadsdtGrdView.Name = "quectelCommnadsdtGrdView";
            this.quectelCommnadsdtGrdView.RowHeadersVisible = false;
            this.quectelCommnadsdtGrdView.RowHeadersWidth = 20;
            this.quectelCommnadsdtGrdView.RowTemplate.Height = 17;
            this.quectelCommnadsdtGrdView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.quectelCommnadsdtGrdView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.quectelCommnadsdtGrdView.Size = new System.Drawing.Size(371, 163);
            this.quectelCommnadsdtGrdView.TabIndex = 27;
            this.quectelCommnadsdtGrdView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.quectelCommnadsdtGrdView_CellEndEdit);
            // 
            // qurctelCommandColumn
            // 
            this.qurctelCommandColumn.HeaderText = "Конфигурационная команда";
            this.qurctelCommandColumn.Name = "qurctelCommandColumn";
            this.qurctelCommandColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.qurctelCommandColumn.Width = 370;
            // 
            // ConfigurationFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1172, 850);
            this.Controls.Add(this.editConfigurationBtn);
            this.Controls.Add(this.deleteConfigurationBtn);
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
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quectelCommnadsdtGrdView)).EndInit();
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
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button editConfigurationBtn;
        private System.Windows.Forms.Button deleteConfigurationBtn;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button addQuectelCommandBtn;
        private System.Windows.Forms.TextBox quectelCommandTxtBox;
        private System.Windows.Forms.DataGridView quectelCommnadsdtGrdView;
        private System.Windows.Forms.DataGridViewTextBoxColumn qurctelCommandColumn;
    }
}