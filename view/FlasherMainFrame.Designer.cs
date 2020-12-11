namespace GSM_NBIoT_Module {
    partial class Flasher {

        //Типы модемов
        private string[] modemsType = { "GSM3" };

        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Flasher));
            this.progressBarFlashing = new System.Windows.Forms.ProgressBar();
            this.startFlashBtn = new System.Windows.Forms.Button();
            this.pathToQuectelFirmwareTextBox = new System.Windows.Forms.TextBox();
            this.pathToSTM32FirmwareTextBox = new System.Windows.Forms.TextBox();
            this.pathToSTM32FirmwareBtn = new System.Windows.Forms.Button();
            this.pathToQuectelFirmwareBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.modemTypeCmBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.flashProcessRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.configurationCmBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.editConfiguration = new System.Windows.Forms.Button();
            this.configurationTextBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.saveLogBtn = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBarFlashing
            // 
            this.progressBarFlashing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarFlashing.Location = new System.Drawing.Point(12, 592);
            this.progressBarFlashing.Maximum = 1000;
            this.progressBarFlashing.Name = "progressBarFlashing";
            this.progressBarFlashing.Size = new System.Drawing.Size(963, 39);
            this.progressBarFlashing.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarFlashing.TabIndex = 0;
            // 
            // startFlashBtn
            // 
            this.startFlashBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startFlashBtn.Location = new System.Drawing.Point(981, 592);
            this.startFlashBtn.Name = "startFlashBtn";
            this.startFlashBtn.Size = new System.Drawing.Size(116, 39);
            this.startFlashBtn.TabIndex = 1;
            this.startFlashBtn.Text = "Начать";
            this.startFlashBtn.UseVisualStyleBackColor = true;
            this.startFlashBtn.Click += new System.EventHandler(this.startFlashBtn_Click);
            // 
            // pathToQuectelFirmwareTextBox
            // 
            this.pathToQuectelFirmwareTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathToQuectelFirmwareTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pathToQuectelFirmwareTextBox.Location = new System.Drawing.Point(215, 47);
            this.pathToQuectelFirmwareTextBox.Name = "pathToQuectelFirmwareTextBox";
            this.pathToQuectelFirmwareTextBox.Size = new System.Drawing.Size(807, 29);
            this.pathToQuectelFirmwareTextBox.TabIndex = 2;
            this.pathToQuectelFirmwareTextBox.Visible = false;
            // 
            // pathToSTM32FirmwareTextBox
            // 
            this.pathToSTM32FirmwareTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathToSTM32FirmwareTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pathToSTM32FirmwareTextBox.Location = new System.Drawing.Point(215, 95);
            this.pathToSTM32FirmwareTextBox.Name = "pathToSTM32FirmwareTextBox";
            this.pathToSTM32FirmwareTextBox.Size = new System.Drawing.Size(807, 29);
            this.pathToSTM32FirmwareTextBox.TabIndex = 3;
            this.pathToSTM32FirmwareTextBox.Visible = false;
            // 
            // pathToSTM32FirmwareBtn
            // 
            this.pathToSTM32FirmwareBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pathToSTM32FirmwareBtn.Location = new System.Drawing.Point(1028, 95);
            this.pathToSTM32FirmwareBtn.Name = "pathToSTM32FirmwareBtn";
            this.pathToSTM32FirmwareBtn.Size = new System.Drawing.Size(69, 29);
            this.pathToSTM32FirmwareBtn.TabIndex = 4;
            this.pathToSTM32FirmwareBtn.Text = "Выбрать";
            this.pathToSTM32FirmwareBtn.UseVisualStyleBackColor = true;
            this.pathToSTM32FirmwareBtn.Visible = false;
            this.pathToSTM32FirmwareBtn.Click += new System.EventHandler(this.pathToSTM32FirmwareBtn_Click);
            // 
            // pathToQuectelFirmwareBtn
            // 
            this.pathToQuectelFirmwareBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pathToQuectelFirmwareBtn.Location = new System.Drawing.Point(1028, 47);
            this.pathToQuectelFirmwareBtn.Name = "pathToQuectelFirmwareBtn";
            this.pathToQuectelFirmwareBtn.Size = new System.Drawing.Size(69, 29);
            this.pathToQuectelFirmwareBtn.TabIndex = 5;
            this.pathToQuectelFirmwareBtn.Text = "Выбрать";
            this.pathToQuectelFirmwareBtn.UseVisualStyleBackColor = true;
            this.pathToQuectelFirmwareBtn.Visible = false;
            this.pathToQuectelFirmwareBtn.Click += new System.EventHandler(this.pathToQuectelFirmwareBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(212, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Путь к прошивке микроконтроллера";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(212, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Путь к прошивке модуля Quectel";
            this.label2.Visible = false;
            // 
            // modemTypeCmBox
            // 
            this.modemTypeCmBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modemTypeCmBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.modemTypeCmBox.FormattingEnabled = true;
            this.modemTypeCmBox.Location = new System.Drawing.Point(12, 68);
            this.modemTypeCmBox.Name = "modemTypeCmBox";
            this.modemTypeCmBox.Size = new System.Drawing.Size(309, 24);
            this.modemTypeCmBox.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Модем";
            // 
            // flashProcessRichTxtBox
            // 
            this.flashProcessRichTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flashProcessRichTxtBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.flashProcessRichTxtBox.Location = new System.Drawing.Point(339, 113);
            this.flashProcessRichTxtBox.Name = "flashProcessRichTxtBox";
            this.flashProcessRichTxtBox.ReadOnly = true;
            this.flashProcessRichTxtBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.flashProcessRichTxtBox.Size = new System.Drawing.Size(758, 459);
            this.flashProcessRichTxtBox.TabIndex = 11;
            this.flashProcessRichTxtBox.Text = "";
            // 
            // configurationCmBox
            // 
            this.configurationCmBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configurationCmBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.configurationCmBox.FormattingEnabled = true;
            this.configurationCmBox.Location = new System.Drawing.Point(12, 23);
            this.configurationCmBox.Name = "configurationCmBox";
            this.configurationCmBox.Size = new System.Drawing.Size(309, 24);
            this.configurationCmBox.TabIndex = 12;
            this.configurationCmBox.SelectedValueChanged += new System.EventHandler(this.configurationCmBox_SelectedValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Конфигурация";
            // 
            // editConfiguration
            // 
            this.editConfiguration.Location = new System.Drawing.Point(339, 23);
            this.editConfiguration.Name = "editConfiguration";
            this.editConfiguration.Size = new System.Drawing.Size(170, 24);
            this.editConfiguration.TabIndex = 14;
            this.editConfiguration.Text = "Настройки конфигурации";
            this.editConfiguration.UseVisualStyleBackColor = true;
            this.editConfiguration.Click += new System.EventHandler(this.editConfiguration_Click);
            // 
            // configurationTextBox
            // 
            this.configurationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.configurationTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.configurationTextBox.Location = new System.Drawing.Point(12, 113);
            this.configurationTextBox.Multiline = true;
            this.configurationTextBox.Name = "configurationTextBox";
            this.configurationTextBox.ReadOnly = true;
            this.configurationTextBox.Size = new System.Drawing.Size(309, 459);
            this.configurationTextBox.TabIndex = 15;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::GSM_NBIoT_Module.Properties.Resources.Logo_R;
            this.pictureBox1.Location = new System.Drawing.Point(879, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(232, 84);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // saveLogBtn
            // 
            this.saveLogBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveLogBtn.Image = global::GSM_NBIoT_Module.Properties.Resources.log_30px;
            this.saveLogBtn.Location = new System.Drawing.Point(1103, 533);
            this.saveLogBtn.Name = "saveLogBtn";
            this.saveLogBtn.Size = new System.Drawing.Size(41, 39);
            this.saveLogBtn.TabIndex = 16;
            this.saveLogBtn.UseVisualStyleBackColor = true;
            this.saveLogBtn.Click += new System.EventHandler(this.saveLogBtn_Click);
            // 
            // Flasher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1150, 669);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.saveLogBtn);
            this.Controls.Add(this.configurationTextBox);
            this.Controls.Add(this.editConfiguration);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.configurationCmBox);
            this.Controls.Add(this.flashProcessRichTxtBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pathToQuectelFirmwareTextBox);
            this.Controls.Add(this.pathToQuectelFirmwareBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.modemTypeCmBox);
            this.Controls.Add(this.pathToSTM32FirmwareBtn);
            this.Controls.Add(this.startFlashBtn);
            this.Controls.Add(this.progressBarFlashing);
            this.Controls.Add(this.pathToSTM32FirmwareTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Flasher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Тайпит Flasher V1.4";
            this.Load += new System.EventHandler(this.Flasher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarFlashing;
        private System.Windows.Forms.Button startFlashBtn;
        private System.Windows.Forms.TextBox pathToQuectelFirmwareTextBox;
        private System.Windows.Forms.TextBox pathToSTM32FirmwareTextBox;
        private System.Windows.Forms.Button pathToSTM32FirmwareBtn;
        private System.Windows.Forms.Button pathToQuectelFirmwareBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox modemTypeCmBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox flashProcessRichTxtBox;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ComboBox configurationCmBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button editConfiguration;
        private System.Windows.Forms.TextBox configurationTextBox;
        private System.Windows.Forms.Button saveLogBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

