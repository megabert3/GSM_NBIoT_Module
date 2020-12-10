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
            this.portTxtBox = new System.Windows.Forms.TextBox();
            this.protocol_idTxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.target_IDtxtBox = new System.Windows.Forms.TextBox();
            this.indexTxtBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.domenNameTxtBox = new System.Windows.Forms.TextBox();
            this.IPv4rdBtn = new System.Windows.Forms.RadioButton();
            this.domenNameRdBtn = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.MCL_chkBox = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
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
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quectelCommnadsdtGrdView)).BeginInit();
            this.SuspendLayout();
            // 
            // saveEditsBtn
            // 
            this.saveEditsBtn.Location = new System.Drawing.Point(706, 258);
            this.saveEditsBtn.Name = "saveEditsBtn";
            this.saveEditsBtn.Size = new System.Drawing.Size(115, 25);
            this.saveEditsBtn.TabIndex = 41;
            this.saveEditsBtn.Text = "Сохранить";
            this.saveEditsBtn.UseVisualStyleBackColor = true;
            this.saveEditsBtn.Click += new System.EventHandler(this.saveEditsBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(558, 258);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(115, 25);
            this.cancelBtn.TabIndex = 42;
            this.cancelBtn.Text = "Отмена";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
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
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(17, 57);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(529, 230);
            this.tabControl1.TabIndex = 46;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Target_ID";
            // 
            // target_IDtxtBox
            // 
            this.target_IDtxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.target_IDtxtBox.Location = new System.Drawing.Point(9, 49);
            this.target_IDtxtBox.Name = "target_IDtxtBox";
            this.target_IDtxtBox.Size = new System.Drawing.Size(115, 20);
            this.target_IDtxtBox.TabIndex = 1;
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
            this.tabPage2.Controls.Add(this.deleteConfCommnadQuectel);
            this.tabPage2.Controls.Add(this.deleteAllConfCommnadQuectel);
            this.tabPage2.Controls.Add(this.copyAllConfCommnadQuectel);
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
            // deleteConfCommnadQuectel
            // 
            this.deleteConfCommnadQuectel.Location = new System.Drawing.Point(383, 152);
            this.deleteConfCommnadQuectel.Name = "deleteConfCommnadQuectel";
            this.deleteConfCommnadQuectel.Size = new System.Drawing.Size(97, 22);
            this.deleteConfCommnadQuectel.TabIndex = 30;
            this.deleteConfCommnadQuectel.Text = "Удалить";
            this.deleteConfCommnadQuectel.UseVisualStyleBackColor = true;
            this.deleteConfCommnadQuectel.Click += new System.EventHandler(this.deleteConfCommnadQuectel_Click);
            // 
            // deleteAllConfCommnadQuectel
            // 
            this.deleteAllConfCommnadQuectel.Location = new System.Drawing.Point(383, 42);
            this.deleteAllConfCommnadQuectel.Name = "deleteAllConfCommnadQuectel";
            this.deleteAllConfCommnadQuectel.Size = new System.Drawing.Size(97, 22);
            this.deleteAllConfCommnadQuectel.TabIndex = 29;
            this.deleteAllConfCommnadQuectel.Text = "Удалить все";
            this.deleteAllConfCommnadQuectel.UseVisualStyleBackColor = true;
            this.deleteAllConfCommnadQuectel.Click += new System.EventHandler(this.deleteAllConfCommnadQuectel_Click);
            // 
            // copyAllConfCommnadQuectel
            // 
            this.copyAllConfCommnadQuectel.Location = new System.Drawing.Point(383, 11);
            this.copyAllConfCommnadQuectel.Name = "copyAllConfCommnadQuectel";
            this.copyAllConfCommnadQuectel.Size = new System.Drawing.Size(97, 22);
            this.copyAllConfCommnadQuectel.TabIndex = 28;
            this.copyAllConfCommnadQuectel.Text = "Копировать все";
            this.copyAllConfCommnadQuectel.UseVisualStyleBackColor = true;
            this.copyAllConfCommnadQuectel.Click += new System.EventHandler(this.copyAllConfCommnadQuectel_Click);
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
            // EditConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 299);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ConfigNameTxtBx);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.saveEditsBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "EditConfigurationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование конфигурации";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditConfigurationForm_KeyDown);
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
        private System.Windows.Forms.TextBox domenNameTxtBox;
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
    }
}