namespace GSM_NBIoT_Module
{
    partial class AddEditConfigurationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddEditConfigurationForm));
            this.saveEditsBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.APN_domenName = new System.Windows.Forms.TextBox();
            this.portListenTxtBx = new System.Windows.Forms.TextBox();
            this.portTxtBox = new System.Windows.Forms.TextBox();
            this.protocol_idTxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.target_IDtxtBox = new System.Windows.Forms.TextBox();
            this.indexTxtBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.IPv6rdBtn = new System.Windows.Forms.RadioButton();
            this.domenNameMskTxtBox = new System.Windows.Forms.MaskedTextBox();
            this.IPv4rdBtn = new System.Windows.Forms.RadioButton();
            this.domenNameRdBtn = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.MCL_chkBox = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.upCommandBtn = new System.Windows.Forms.Button();
            this.downCommandBtn = new System.Windows.Forms.Button();
            this.deleteConfCommnadQuectel = new System.Windows.Forms.Button();
            this.deleteAllConfCommnadQuectel = new System.Windows.Forms.Button();
            this.copyAllConfCommnadQuectel = new System.Windows.Forms.Button();
            this.quectelCommnadsdtGrdView = new System.Windows.Forms.DataGridView();
            this.qurctelCommandColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addQuectelCommandBtn = new System.Windows.Forms.Button();
            this.quectelCommandTxtBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ConfigNameTxtBx = new System.Windows.Forms.TextBox();
            this.groupBox5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quectelCommnadsdtGrdView)).BeginInit();
            this.SuspendLayout();
            // 
            // saveEditsBtn
            // 
            this.saveEditsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveEditsBtn.Location = new System.Drawing.Point(706, 304);
            this.saveEditsBtn.Name = "saveEditsBtn";
            this.saveEditsBtn.Size = new System.Drawing.Size(115, 25);
            this.saveEditsBtn.TabIndex = 41;
            this.saveEditsBtn.Text = "Сохранить";
            this.saveEditsBtn.UseVisualStyleBackColor = true;
            this.saveEditsBtn.Click += new System.EventHandler(this.saveEditsBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.Location = new System.Drawing.Point(558, 304);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(115, 25);
            this.cancelBtn.TabIndex = 42;
            this.cancelBtn.Text = "Отмена";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupBox5.Controls.Add(this.pathToFW_MKtxtBx);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.pathToFW_QuectelBtn);
            this.groupBox5.Controls.Add(this.pathToFW_QuectelTxtBx);
            this.groupBox5.Controls.Add(this.pathToFW_MKBtn);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Location = new System.Drawing.Point(552, 79);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(269, 110);
            this.groupBox5.TabIndex = 47;
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
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(34, 57);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(512, 276);
            this.tabControl1.TabIndex = 46;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(504, 250);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Конфигурация микроконтроллера";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox1);
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
            this.groupBox4.Size = new System.Drawing.Size(509, 232);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Конфигурация микроконтроллера";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.APN_domenName);
            this.groupBox1.Controls.Add(this.portListenTxtBx);
            this.groupBox1.Location = new System.Drawing.Point(6, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(475, 65);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Настройки локальной сети";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Порт";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(161, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Доменное имя APN сети";
            // 
            // APN_domenName
            // 
            this.APN_domenName.Location = new System.Drawing.Point(164, 37);
            this.APN_domenName.Name = "APN_domenName";
            this.APN_domenName.Size = new System.Drawing.Size(288, 20);
            this.APN_domenName.TabIndex = 1;
            // 
            // portListenTxtBx
            // 
            this.portListenTxtBx.Location = new System.Drawing.Point(17, 37);
            this.portListenTxtBx.Name = "portListenTxtBx";
            this.portListenTxtBx.Size = new System.Drawing.Size(115, 20);
            this.portListenTxtBx.TabIndex = 0;
            // 
            // portTxtBox
            // 
            this.portTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.portTxtBox.Location = new System.Drawing.Point(153, 43);
            this.portTxtBox.Name = "portTxtBox";
            this.portTxtBox.Size = new System.Drawing.Size(115, 20);
            this.portTxtBox.TabIndex = 8;
            // 
            // protocol_idTxtBox
            // 
            this.protocol_idTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.protocol_idTxtBox.Location = new System.Drawing.Point(23, 82);
            this.protocol_idTxtBox.Name = "protocol_idTxtBox";
            this.protocol_idTxtBox.Size = new System.Drawing.Size(115, 20);
            this.protocol_idTxtBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Protocol_ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Target_ID";
            // 
            // target_IDtxtBox
            // 
            this.target_IDtxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.target_IDtxtBox.Location = new System.Drawing.Point(23, 43);
            this.target_IDtxtBox.Name = "target_IDtxtBox";
            this.target_IDtxtBox.Size = new System.Drawing.Size(115, 20);
            this.target_IDtxtBox.TabIndex = 1;
            // 
            // indexTxtBox
            // 
            this.indexTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.indexTxtBox.Location = new System.Drawing.Point(23, 121);
            this.indexTxtBox.Name = "indexTxtBox";
            this.indexTxtBox.Size = new System.Drawing.Size(115, 20);
            this.indexTxtBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Index";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.IPv6rdBtn);
            this.groupBox2.Controls.Add(this.domenNameMskTxtBox);
            this.groupBox2.Controls.Add(this.IPv4rdBtn);
            this.groupBox2.Controls.Add(this.domenNameRdBtn);
            this.groupBox2.Location = new System.Drawing.Point(153, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(328, 71);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // IPv6rdBtn
            // 
            this.IPv6rdBtn.AutoSize = true;
            this.IPv6rdBtn.Location = new System.Drawing.Point(225, 22);
            this.IPv6rdBtn.Name = "IPv6rdBtn";
            this.IPv6rdBtn.Size = new System.Drawing.Size(80, 17);
            this.IPv6rdBtn.TabIndex = 12;
            this.IPv6rdBtn.TabStop = true;
            this.IPv6rdBtn.Text = "IPv6 адрес";
            this.IPv6rdBtn.UseVisualStyleBackColor = true;
            // 
            // domenNameMskTxtBox
            // 
            this.domenNameMskTxtBox.Location = new System.Drawing.Point(17, 45);
            this.domenNameMskTxtBox.Name = "domenNameMskTxtBox";
            this.domenNameMskTxtBox.Size = new System.Drawing.Size(288, 20);
            this.domenNameMskTxtBox.TabIndex = 11;
            this.domenNameMskTxtBox.Enter += new System.EventHandler(this.domenNameMskTxtBox_Enter);
            // 
            // IPv4rdBtn
            // 
            this.IPv4rdBtn.AutoSize = true;
            this.IPv4rdBtn.Location = new System.Drawing.Point(131, 22);
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
            this.domenNameRdBtn.Location = new System.Drawing.Point(17, 22);
            this.domenNameRdBtn.Name = "domenNameRdBtn";
            this.domenNameRdBtn.Size = new System.Drawing.Size(101, 17);
            this.domenNameRdBtn.TabIndex = 0;
            this.domenNameRdBtn.TabStop = true;
            this.domenNameRdBtn.Text = "Доменное имя";
            this.domenNameRdBtn.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(150, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Порт";
            // 
            // MCL_chkBox
            // 
            this.MCL_chkBox.AutoSize = true;
            this.MCL_chkBox.Location = new System.Drawing.Point(285, 45);
            this.MCL_chkBox.Name = "MCL_chkBox";
            this.MCL_chkBox.Size = new System.Drawing.Size(78, 17);
            this.MCL_chkBox.TabIndex = 10;
            this.MCL_chkBox.Text = "MCL Mode";
            this.MCL_chkBox.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.upCommandBtn);
            this.tabPage2.Controls.Add(this.downCommandBtn);
            this.tabPage2.Controls.Add(this.deleteConfCommnadQuectel);
            this.tabPage2.Controls.Add(this.deleteAllConfCommnadQuectel);
            this.tabPage2.Controls.Add(this.copyAllConfCommnadQuectel);
            this.tabPage2.Controls.Add(this.quectelCommnadsdtGrdView);
            this.tabPage2.Controls.Add(this.addQuectelCommandBtn);
            this.tabPage2.Controls.Add(this.quectelCommandTxtBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(504, 250);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Конфигурация модуля Quectel";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // upCommandBtn
            // 
            this.upCommandBtn.Image = ((System.Drawing.Image)(resources.GetObject("upCommandBtn.Image")));
            this.upCommandBtn.Location = new System.Drawing.Point(393, 96);
            this.upCommandBtn.Name = "upCommandBtn";
            this.upCommandBtn.Size = new System.Drawing.Size(22, 22);
            this.upCommandBtn.TabIndex = 32;
            this.upCommandBtn.UseVisualStyleBackColor = true;
            this.upCommandBtn.Click += new System.EventHandler(this.upCommandBtn_Click);
            // 
            // downCommandBtn
            // 
            this.downCommandBtn.Image = ((System.Drawing.Image)(resources.GetObject("downCommandBtn.Image")));
            this.downCommandBtn.Location = new System.Drawing.Point(393, 124);
            this.downCommandBtn.Name = "downCommandBtn";
            this.downCommandBtn.Size = new System.Drawing.Size(22, 22);
            this.downCommandBtn.TabIndex = 31;
            this.downCommandBtn.UseVisualStyleBackColor = true;
            this.downCommandBtn.Click += new System.EventHandler(this.downCommandBtn_Click);
            // 
            // deleteConfCommnadQuectel
            // 
            this.deleteConfCommnadQuectel.Location = new System.Drawing.Point(393, 152);
            this.deleteConfCommnadQuectel.Name = "deleteConfCommnadQuectel";
            this.deleteConfCommnadQuectel.Size = new System.Drawing.Size(97, 22);
            this.deleteConfCommnadQuectel.TabIndex = 30;
            this.deleteConfCommnadQuectel.Text = "Удалить";
            this.deleteConfCommnadQuectel.UseVisualStyleBackColor = true;
            this.deleteConfCommnadQuectel.Click += new System.EventHandler(this.deleteConfCommnadQuectel_Click);
            // 
            // deleteAllConfCommnadQuectel
            // 
            this.deleteAllConfCommnadQuectel.Location = new System.Drawing.Point(393, 42);
            this.deleteAllConfCommnadQuectel.Name = "deleteAllConfCommnadQuectel";
            this.deleteAllConfCommnadQuectel.Size = new System.Drawing.Size(97, 22);
            this.deleteAllConfCommnadQuectel.TabIndex = 29;
            this.deleteAllConfCommnadQuectel.Text = "Удалить все";
            this.deleteAllConfCommnadQuectel.UseVisualStyleBackColor = true;
            this.deleteAllConfCommnadQuectel.Click += new System.EventHandler(this.deleteAllConfCommnadQuectel_Click);
            // 
            // copyAllConfCommnadQuectel
            // 
            this.copyAllConfCommnadQuectel.Location = new System.Drawing.Point(393, 11);
            this.copyAllConfCommnadQuectel.Name = "copyAllConfCommnadQuectel";
            this.copyAllConfCommnadQuectel.Size = new System.Drawing.Size(97, 22);
            this.copyAllConfCommnadQuectel.TabIndex = 28;
            this.copyAllConfCommnadQuectel.Text = "Копировать все";
            this.copyAllConfCommnadQuectel.UseVisualStyleBackColor = true;
            this.copyAllConfCommnadQuectel.Click += new System.EventHandler(this.copyAllConfCommnadQuectel_Click);
            // 
            // quectelCommnadsdtGrdView
            // 
            this.quectelCommnadsdtGrdView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.quectelCommnadsdtGrdView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.quectelCommnadsdtGrdView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.qurctelCommandColumn});
            this.quectelCommnadsdtGrdView.Location = new System.Drawing.Point(16, 11);
            this.quectelCommnadsdtGrdView.MultiSelect = false;
            this.quectelCommnadsdtGrdView.Name = "quectelCommnadsdtGrdView";
            this.quectelCommnadsdtGrdView.RowHeadersVisible = false;
            this.quectelCommnadsdtGrdView.RowHeadersWidth = 20;
            this.quectelCommnadsdtGrdView.RowTemplate.Height = 17;
            this.quectelCommnadsdtGrdView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.quectelCommnadsdtGrdView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.quectelCommnadsdtGrdView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.quectelCommnadsdtGrdView.Size = new System.Drawing.Size(371, 163);
            this.quectelCommnadsdtGrdView.TabIndex = 27;
            this.quectelCommnadsdtGrdView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.quectelCommnadsdtGrdView_CellDoubleClick);
            this.quectelCommnadsdtGrdView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.quectelCommnadsdtGrdView_CellEndEdit);
            this.quectelCommnadsdtGrdView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.quectelCommnadsdtGrdView_RowsAdded);
            this.quectelCommnadsdtGrdView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.quectelCommnadsdtGrdView_RowsRemoved);
            // 
            // qurctelCommandColumn
            // 
            this.qurctelCommandColumn.HeaderText = "Конфигурационная команда";
            this.qurctelCommandColumn.Name = "qurctelCommandColumn";
            this.qurctelCommandColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.qurctelCommandColumn.Width = 370;
            // 
            // addQuectelCommandBtn
            // 
            this.addQuectelCommandBtn.Location = new System.Drawing.Point(290, 179);
            this.addQuectelCommandBtn.Name = "addQuectelCommandBtn";
            this.addQuectelCommandBtn.Size = new System.Drawing.Size(97, 22);
            this.addQuectelCommandBtn.TabIndex = 2;
            this.addQuectelCommandBtn.Text = "Добавить";
            this.addQuectelCommandBtn.UseVisualStyleBackColor = true;
            this.addQuectelCommandBtn.Click += new System.EventHandler(this.addQuectelCommandBtn_Click);
            // 
            // quectelCommandTxtBox
            // 
            this.quectelCommandTxtBox.Location = new System.Drawing.Point(16, 180);
            this.quectelCommandTxtBox.Name = "quectelCommandTxtBox";
            this.quectelCommandTxtBox.Size = new System.Drawing.Size(268, 20);
            this.quectelCommandTxtBox.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 13);
            this.label8.TabIndex = 45;
            this.label8.Text = "Название конфигурации";
            // 
            // ConfigNameTxtBx
            // 
            this.ConfigNameTxtBx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ConfigNameTxtBx.Location = new System.Drawing.Point(17, 26);
            this.ConfigNameTxtBx.Name = "ConfigNameTxtBx";
            this.ConfigNameTxtBx.Size = new System.Drawing.Size(525, 20);
            this.ConfigNameTxtBx.TabIndex = 44;
            // 
            // AddEditConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 345);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ConfigNameTxtBx);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.saveEditsBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditConfigurationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование конфигурации";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditConfigurationForm_KeyDown);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quectelCommnadsdtGrdView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button saveEditsBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox pathToFW_MKtxtBx;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button pathToFW_QuectelBtn;
        private System.Windows.Forms.TextBox pathToFW_QuectelTxtBx;
        private System.Windows.Forms.Button pathToFW_MKBtn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox portTxtBox;
        private System.Windows.Forms.TextBox protocol_idTxtBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox target_IDtxtBox;
        private System.Windows.Forms.TextBox indexTxtBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton IPv4rdBtn;
        private System.Windows.Forms.RadioButton domenNameRdBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox MCL_chkBox;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button deleteConfCommnadQuectel;
        private System.Windows.Forms.Button deleteAllConfCommnadQuectel;
        private System.Windows.Forms.Button copyAllConfCommnadQuectel;
        private System.Windows.Forms.DataGridView quectelCommnadsdtGrdView;
        private System.Windows.Forms.DataGridViewTextBoxColumn qurctelCommandColumn;
        private System.Windows.Forms.Button addQuectelCommandBtn;
        private System.Windows.Forms.TextBox quectelCommandTxtBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ConfigNameTxtBx;
        private System.Windows.Forms.Button upCommandBtn;
        private System.Windows.Forms.Button downCommandBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox APN_domenName;
        private System.Windows.Forms.TextBox portListenTxtBx;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MaskedTextBox domenNameMskTxtBox;
        private System.Windows.Forms.RadioButton IPv6rdBtn;
    }
}