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
            this.addConfigurationBtn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editConfigurationBtn = new System.Windows.Forms.Button();
            this.deleteConfigurationBtn = new System.Windows.Forms.Button();
            this.configurationDataGridView = new System.Windows.Forms.DataGridView();
            this.label11 = new System.Windows.Forms.Label();
            this.setPasswordBtn = new System.Windows.Forms.Button();
            this.copyBtn = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.terget_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.protocol_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MCL_mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.domenNameOrIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.configCommandsQuectel = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.frimwareForMK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.frimwareForQuectel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.APNName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ListenPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.configurationDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // addConfigurationBtn
            // 
            this.addConfigurationBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addConfigurationBtn.Location = new System.Drawing.Point(851, 473);
            this.addConfigurationBtn.Name = "addConfigurationBtn";
            this.addConfigurationBtn.Size = new System.Drawing.Size(115, 25);
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
            this.editConfigurationBtn.Location = new System.Drawing.Point(997, 473);
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
            this.deleteConfigurationBtn.Location = new System.Drawing.Point(1118, 473);
            this.deleteConfigurationBtn.Name = "deleteConfigurationBtn";
            this.deleteConfigurationBtn.Size = new System.Drawing.Size(115, 25);
            this.deleteConfigurationBtn.TabIndex = 26;
            this.deleteConfigurationBtn.Text = "Удалить";
            this.deleteConfigurationBtn.UseVisualStyleBackColor = true;
            this.deleteConfigurationBtn.Click += new System.EventHandler(this.deleteConfigurationBtn_Click);
            // 
            // configurationDataGridView
            // 
            this.configurationDataGridView.AllowUserToDeleteRows = false;
            this.configurationDataGridView.AllowUserToResizeRows = false;
            this.configurationDataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.configurationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.configurationDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.terget_id,
            this.protocol_id,
            this.index,
            this.MCL_mode,
            this.port,
            this.domenNameOrIP,
            this.configCommandsQuectel,
            this.frimwareForMK,
            this.frimwareForQuectel,
            this.APNName,
            this.ListenPort});
            this.configurationDataGridView.Location = new System.Drawing.Point(12, 36);
            this.configurationDataGridView.MultiSelect = false;
            this.configurationDataGridView.Name = "configurationDataGridView";
            this.configurationDataGridView.RowHeadersVisible = false;
            this.configurationDataGridView.RowHeadersWidth = 20;
            this.configurationDataGridView.RowTemplate.Height = 17;
            this.configurationDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.configurationDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.configurationDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.configurationDataGridView.Size = new System.Drawing.Size(1220, 431);
            this.configurationDataGridView.TabIndex = 31;
            this.configurationDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.configurationDataGridView_CellClick);
            this.configurationDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.configurationDataGridView_CellMouseDoubleClick);
            this.configurationDataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.configurationDataGridView_RowsAdded);
            this.configurationDataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.configurationDataGridView_RowsRemoved);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 13);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(177, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "Список созданных конфигураций";
            // 
            // setPasswordBtn
            // 
            this.setPasswordBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setPasswordBtn.Location = new System.Drawing.Point(12, 473);
            this.setPasswordBtn.Name = "setPasswordBtn";
            this.setPasswordBtn.Size = new System.Drawing.Size(115, 25);
            this.setPasswordBtn.TabIndex = 33;
            this.setPasswordBtn.Text = "Установить пароль";
            this.setPasswordBtn.UseVisualStyleBackColor = true;
            this.setPasswordBtn.Click += new System.EventHandler(this.setPasswordBtn_Click);
            // 
            // copyBtn
            // 
            this.copyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.copyBtn.Location = new System.Drawing.Point(715, 473);
            this.copyBtn.Name = "copyBtn";
            this.copyBtn.Size = new System.Drawing.Size(115, 25);
            this.copyBtn.TabIndex = 34;
            this.copyBtn.Text = "Копировать";
            this.copyBtn.UseVisualStyleBackColor = true;
            this.copyBtn.Visible = false;
            this.copyBtn.Click += new System.EventHandler(this.copyBtn_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Имя конфигурации";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 175;
            // 
            // terget_id
            // 
            this.terget_id.HeaderText = "Target_ID";
            this.terget_id.Name = "terget_id";
            this.terget_id.ReadOnly = true;
            this.terget_id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.terget_id.Width = 57;
            // 
            // protocol_id
            // 
            this.protocol_id.HeaderText = "Protocol_ID";
            this.protocol_id.Name = "protocol_id";
            this.protocol_id.ReadOnly = true;
            this.protocol_id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.protocol_id.Width = 65;
            // 
            // index
            // 
            this.index.HeaderText = "Index";
            this.index.Name = "index";
            this.index.ReadOnly = true;
            this.index.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.index.Width = 40;
            // 
            // MCL_mode
            // 
            this.MCL_mode.HeaderText = "MCL Mode";
            this.MCL_mode.Name = "MCL_mode";
            this.MCL_mode.ReadOnly = true;
            this.MCL_mode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MCL_mode.Width = 40;
            // 
            // port
            // 
            this.port.HeaderText = "Порт";
            this.port.Name = "port";
            this.port.ReadOnly = true;
            this.port.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.port.Width = 70;
            // 
            // domenNameOrIP
            // 
            this.domenNameOrIP.HeaderText = "Доменное имя или IP адрес";
            this.domenNameOrIP.Name = "domenNameOrIP";
            this.domenNameOrIP.ReadOnly = true;
            this.domenNameOrIP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.domenNameOrIP.Width = 175;
            // 
            // configCommandsQuectel
            // 
            this.configCommandsQuectel.HeaderText = "Команды модулю Quectel";
            this.configCommandsQuectel.Name = "configCommandsQuectel";
            // 
            // frimwareForMK
            // 
            this.frimwareForMK.HeaderText = "Прошивка для МК";
            this.frimwareForMK.Name = "frimwareForMK";
            this.frimwareForMK.ReadOnly = true;
            this.frimwareForMK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.frimwareForMK.Width = 120;
            // 
            // frimwareForQuectel
            // 
            this.frimwareForQuectel.HeaderText = "Прошивка для Quectel";
            this.frimwareForQuectel.Name = "frimwareForQuectel";
            this.frimwareForQuectel.ReadOnly = true;
            this.frimwareForQuectel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.frimwareForQuectel.Width = 150;
            // 
            // APNName
            // 
            this.APNName.HeaderText = "APN";
            this.APNName.Name = "APNName";
            this.APNName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.APNName.Width = 150;
            // 
            // ListenPort
            // 
            this.ListenPort.HeaderText = "Порт входящего соединения";
            this.ListenPort.Name = "ListenPort";
            this.ListenPort.ReadOnly = true;
            this.ListenPort.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ListenPort.Width = 75;
            // 
            // ConfigurationFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1244, 507);
            this.Controls.Add(this.copyBtn);
            this.Controls.Add(this.setPasswordBtn);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.addConfigurationBtn);
            this.Controls.Add(this.configurationDataGridView);
            this.Controls.Add(this.editConfigurationBtn);
            this.Controls.Add(this.deleteConfigurationBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ConfigurationFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Конфигурация";
            this.Load += new System.EventHandler(this.ConfigurationFrame_Load);
            ((System.ComponentModel.ISupportInitialize)(this.configurationDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button addConfigurationBtn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.Button editConfigurationBtn;
        private System.Windows.Forms.Button deleteConfigurationBtn;
        private System.Windows.Forms.DataGridView configurationDataGridView;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button setPasswordBtn;
        private System.Windows.Forms.Button copyBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn terget_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn protocol_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn index;
        private System.Windows.Forms.DataGridViewTextBoxColumn MCL_mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn port;
        private System.Windows.Forms.DataGridViewTextBoxColumn domenNameOrIP;
        private System.Windows.Forms.DataGridViewComboBoxColumn configCommandsQuectel;
        private System.Windows.Forms.DataGridViewTextBoxColumn frimwareForMK;
        private System.Windows.Forms.DataGridViewTextBoxColumn frimwareForQuectel;
        private System.Windows.Forms.DataGridViewTextBoxColumn APNName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ListenPort;
    }
}