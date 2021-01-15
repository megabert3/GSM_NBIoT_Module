namespace GSM_NBIoT_Module.view {
    partial class Terminal {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.indBtn = new System.Windows.Forms.Button();
            this.rescanCOMsBtn = new System.Windows.Forms.Button();
            this.connOrDisCOMBtn = new System.Windows.Forms.Button();
            this.comPortsListCmbBox = new System.Windows.Forms.ComboBox();
            this.bandRateGroup = new System.Windows.Forms.GroupBox();
            this.customBandRateTxtBx = new System.Windows.Forms.TextBox();
            this.customBandRateRdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate256000rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate128000rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate115200rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate57600rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate56000rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate38400rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate28800rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate19200rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate14400rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate9600rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate4800rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate2400rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate1200rdBtn = new System.Windows.Forms.RadioButton();
            this.bandRate600rdBtn = new System.Windows.Forms.RadioButton();
            this.dataBitGroup = new System.Windows.Forms.GroupBox();
            this.dataBit8rdBtn = new System.Windows.Forms.RadioButton();
            this.dataBit7rdBtn = new System.Windows.Forms.RadioButton();
            this.dataBit5rdBtn = new System.Windows.Forms.RadioButton();
            this.dataBit6rdBtn = new System.Windows.Forms.RadioButton();
            this.parityGroup = new System.Windows.Forms.GroupBox();
            this.paritySpaceRdBtn = new System.Windows.Forms.RadioButton();
            this.parityMarkRdBtn = new System.Windows.Forms.RadioButton();
            this.parityEvenRdBtn = new System.Windows.Forms.RadioButton();
            this.parityNoneRdBtn = new System.Windows.Forms.RadioButton();
            this.parityOddRdBtn = new System.Windows.Forms.RadioButton();
            this.stopBitGroup = new System.Windows.Forms.GroupBox();
            this.stopBit2RdBtn = new System.Windows.Forms.RadioButton();
            this.stopBit1RdBtn = new System.Windows.Forms.RadioButton();
            this.stopBit1_5RdBtn = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.enhanGPIO_1chBx = new System.Windows.Forms.CheckBox();
            this.enhanGPIO_0chBx = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cpNumbEnhancedPortTxtBx = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.standGPIO_2chBx = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.standGPIO_1chBx = new System.Windows.Forms.CheckBox();
            this.cpNumbStandartPortTxtBx = new System.Windows.Forms.TextBox();
            this.standGPIO_0chBx = new System.Windows.Forms.CheckBox();
            this.searchCP2105Ports = new System.Windows.Forms.Button();
            this.terminalLogTxtBx = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.modeGroup = new System.Windows.Forms.GroupBox();
            this.modeTextRdBtn = new System.Windows.Forms.RadioButton();
            this.modeHexRdBtn = new System.Windows.Forms.RadioButton();
            this.connectToModuleBtn = new System.Windows.Forms.Button();
            this.connectToMKBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.bandRateGroup.SuspendLayout();
            this.dataBitGroup.SuspendLayout();
            this.parityGroup.SuspendLayout();
            this.stopBitGroup.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.modeGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.indBtn);
            this.groupBox1.Controls.Add(this.rescanCOMsBtn);
            this.groupBox1.Controls.Add(this.connOrDisCOMBtn);
            this.groupBox1.Controls.Add(this.comPortsListCmbBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(123, 131);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "COM порт";
            // 
            // indBtn
            // 
            this.indBtn.FlatAppearance.BorderSize = 0;
            this.indBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.indBtn.Location = new System.Drawing.Point(6, 48);
            this.indBtn.Name = "indBtn";
            this.indBtn.Size = new System.Drawing.Size(20, 19);
            this.indBtn.TabIndex = 24;
            this.indBtn.UseVisualStyleBackColor = true;
            // 
            // rescanCOMsBtn
            // 
            this.rescanCOMsBtn.Image = global::GSM_NBIoT_Module.Properties.Resources.restart_15px;
            this.rescanCOMsBtn.Location = new System.Drawing.Point(95, 19);
            this.rescanCOMsBtn.Name = "rescanCOMsBtn";
            this.rescanCOMsBtn.Size = new System.Drawing.Size(22, 22);
            this.rescanCOMsBtn.TabIndex = 3;
            this.rescanCOMsBtn.UseVisualStyleBackColor = true;
            this.rescanCOMsBtn.Click += new System.EventHandler(this.rescanCOMsBtn_Click);
            // 
            // connOrDisCOMBtn
            // 
            this.connOrDisCOMBtn.Location = new System.Drawing.Point(28, 47);
            this.connOrDisCOMBtn.Name = "connOrDisCOMBtn";
            this.connOrDisCOMBtn.Size = new System.Drawing.Size(89, 21);
            this.connOrDisCOMBtn.TabIndex = 1;
            this.connOrDisCOMBtn.Text = "Connect";
            this.connOrDisCOMBtn.UseVisualStyleBackColor = true;
            this.connOrDisCOMBtn.Click += new System.EventHandler(this.connOrDisCOMBtn_Click);
            // 
            // comPortsListCmbBox
            // 
            this.comPortsListCmbBox.FormattingEnabled = true;
            this.comPortsListCmbBox.Location = new System.Drawing.Point(6, 19);
            this.comPortsListCmbBox.Name = "comPortsListCmbBox";
            this.comPortsListCmbBox.Size = new System.Drawing.Size(84, 21);
            this.comPortsListCmbBox.TabIndex = 0;
            // 
            // bandRateGroup
            // 
            this.bandRateGroup.Controls.Add(this.customBandRateTxtBx);
            this.bandRateGroup.Controls.Add(this.customBandRateRdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate256000rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate128000rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate115200rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate57600rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate56000rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate38400rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate28800rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate19200rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate14400rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate9600rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate4800rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate2400rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate1200rdBtn);
            this.bandRateGroup.Controls.Add(this.bandRate600rdBtn);
            this.bandRateGroup.Location = new System.Drawing.Point(141, 3);
            this.bandRateGroup.Name = "bandRateGroup";
            this.bandRateGroup.Size = new System.Drawing.Size(276, 131);
            this.bandRateGroup.TabIndex = 1;
            this.bandRateGroup.TabStop = false;
            this.bandRateGroup.Text = "Скорость";
            // 
            // customBandRateTxtBx
            // 
            this.customBandRateTxtBx.Location = new System.Drawing.Point(191, 105);
            this.customBandRateTxtBx.Name = "customBandRateTxtBx";
            this.customBandRateTxtBx.Size = new System.Drawing.Size(75, 20);
            this.customBandRateTxtBx.TabIndex = 2;
            // 
            // customBandRateRdBtn
            // 
            this.customBandRateRdBtn.AutoSize = true;
            this.customBandRateRdBtn.Location = new System.Drawing.Point(135, 107);
            this.customBandRateRdBtn.Name = "customBandRateRdBtn";
            this.customBandRateRdBtn.Size = new System.Drawing.Size(50, 17);
            this.customBandRateRdBtn.TabIndex = 14;
            this.customBandRateRdBtn.TabStop = true;
            this.customBandRateRdBtn.Text = "Своя";
            this.customBandRateRdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate256000rdBtn
            // 
            this.bandRate256000rdBtn.AutoSize = true;
            this.bandRate256000rdBtn.Location = new System.Drawing.Point(135, 85);
            this.bandRate256000rdBtn.Name = "bandRate256000rdBtn";
            this.bandRate256000rdBtn.Size = new System.Drawing.Size(61, 17);
            this.bandRate256000rdBtn.TabIndex = 13;
            this.bandRate256000rdBtn.TabStop = true;
            this.bandRate256000rdBtn.Text = "256000";
            this.bandRate256000rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate128000rdBtn
            // 
            this.bandRate128000rdBtn.AutoSize = true;
            this.bandRate128000rdBtn.Location = new System.Drawing.Point(135, 63);
            this.bandRate128000rdBtn.Name = "bandRate128000rdBtn";
            this.bandRate128000rdBtn.Size = new System.Drawing.Size(61, 17);
            this.bandRate128000rdBtn.TabIndex = 12;
            this.bandRate128000rdBtn.TabStop = true;
            this.bandRate128000rdBtn.Text = "128000";
            this.bandRate128000rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate115200rdBtn
            // 
            this.bandRate115200rdBtn.AutoSize = true;
            this.bandRate115200rdBtn.Location = new System.Drawing.Point(135, 41);
            this.bandRate115200rdBtn.Name = "bandRate115200rdBtn";
            this.bandRate115200rdBtn.Size = new System.Drawing.Size(61, 17);
            this.bandRate115200rdBtn.TabIndex = 11;
            this.bandRate115200rdBtn.TabStop = true;
            this.bandRate115200rdBtn.Text = "115200";
            this.bandRate115200rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate57600rdBtn
            // 
            this.bandRate57600rdBtn.AutoSize = true;
            this.bandRate57600rdBtn.Location = new System.Drawing.Point(135, 19);
            this.bandRate57600rdBtn.Name = "bandRate57600rdBtn";
            this.bandRate57600rdBtn.Size = new System.Drawing.Size(55, 17);
            this.bandRate57600rdBtn.TabIndex = 10;
            this.bandRate57600rdBtn.TabStop = true;
            this.bandRate57600rdBtn.Text = "57600";
            this.bandRate57600rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate56000rdBtn
            // 
            this.bandRate56000rdBtn.AutoSize = true;
            this.bandRate56000rdBtn.Location = new System.Drawing.Point(68, 107);
            this.bandRate56000rdBtn.Name = "bandRate56000rdBtn";
            this.bandRate56000rdBtn.Size = new System.Drawing.Size(55, 17);
            this.bandRate56000rdBtn.TabIndex = 9;
            this.bandRate56000rdBtn.TabStop = true;
            this.bandRate56000rdBtn.Text = "56000";
            this.bandRate56000rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate38400rdBtn
            // 
            this.bandRate38400rdBtn.AutoSize = true;
            this.bandRate38400rdBtn.Location = new System.Drawing.Point(68, 85);
            this.bandRate38400rdBtn.Name = "bandRate38400rdBtn";
            this.bandRate38400rdBtn.Size = new System.Drawing.Size(55, 17);
            this.bandRate38400rdBtn.TabIndex = 8;
            this.bandRate38400rdBtn.TabStop = true;
            this.bandRate38400rdBtn.Text = "38400";
            this.bandRate38400rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate28800rdBtn
            // 
            this.bandRate28800rdBtn.AutoSize = true;
            this.bandRate28800rdBtn.Location = new System.Drawing.Point(68, 63);
            this.bandRate28800rdBtn.Name = "bandRate28800rdBtn";
            this.bandRate28800rdBtn.Size = new System.Drawing.Size(55, 17);
            this.bandRate28800rdBtn.TabIndex = 7;
            this.bandRate28800rdBtn.TabStop = true;
            this.bandRate28800rdBtn.Text = "28800";
            this.bandRate28800rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate19200rdBtn
            // 
            this.bandRate19200rdBtn.AutoSize = true;
            this.bandRate19200rdBtn.Location = new System.Drawing.Point(68, 41);
            this.bandRate19200rdBtn.Name = "bandRate19200rdBtn";
            this.bandRate19200rdBtn.Size = new System.Drawing.Size(55, 17);
            this.bandRate19200rdBtn.TabIndex = 6;
            this.bandRate19200rdBtn.TabStop = true;
            this.bandRate19200rdBtn.Text = "19200";
            this.bandRate19200rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate14400rdBtn
            // 
            this.bandRate14400rdBtn.AutoSize = true;
            this.bandRate14400rdBtn.Location = new System.Drawing.Point(68, 19);
            this.bandRate14400rdBtn.Name = "bandRate14400rdBtn";
            this.bandRate14400rdBtn.Size = new System.Drawing.Size(55, 17);
            this.bandRate14400rdBtn.TabIndex = 5;
            this.bandRate14400rdBtn.TabStop = true;
            this.bandRate14400rdBtn.Text = "14400";
            this.bandRate14400rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate9600rdBtn
            // 
            this.bandRate9600rdBtn.AutoSize = true;
            this.bandRate9600rdBtn.Location = new System.Drawing.Point(7, 106);
            this.bandRate9600rdBtn.Name = "bandRate9600rdBtn";
            this.bandRate9600rdBtn.Size = new System.Drawing.Size(49, 17);
            this.bandRate9600rdBtn.TabIndex = 4;
            this.bandRate9600rdBtn.TabStop = true;
            this.bandRate9600rdBtn.Text = "9600";
            this.bandRate9600rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate4800rdBtn
            // 
            this.bandRate4800rdBtn.AutoSize = true;
            this.bandRate4800rdBtn.Location = new System.Drawing.Point(7, 84);
            this.bandRate4800rdBtn.Name = "bandRate4800rdBtn";
            this.bandRate4800rdBtn.Size = new System.Drawing.Size(49, 17);
            this.bandRate4800rdBtn.TabIndex = 3;
            this.bandRate4800rdBtn.TabStop = true;
            this.bandRate4800rdBtn.Text = "4800";
            this.bandRate4800rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate2400rdBtn
            // 
            this.bandRate2400rdBtn.AutoSize = true;
            this.bandRate2400rdBtn.Location = new System.Drawing.Point(7, 62);
            this.bandRate2400rdBtn.Name = "bandRate2400rdBtn";
            this.bandRate2400rdBtn.Size = new System.Drawing.Size(49, 17);
            this.bandRate2400rdBtn.TabIndex = 2;
            this.bandRate2400rdBtn.TabStop = true;
            this.bandRate2400rdBtn.Text = "2400";
            this.bandRate2400rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate1200rdBtn
            // 
            this.bandRate1200rdBtn.AutoSize = true;
            this.bandRate1200rdBtn.Location = new System.Drawing.Point(7, 40);
            this.bandRate1200rdBtn.Name = "bandRate1200rdBtn";
            this.bandRate1200rdBtn.Size = new System.Drawing.Size(49, 17);
            this.bandRate1200rdBtn.TabIndex = 1;
            this.bandRate1200rdBtn.TabStop = true;
            this.bandRate1200rdBtn.Text = "1200";
            this.bandRate1200rdBtn.UseVisualStyleBackColor = true;
            // 
            // bandRate600rdBtn
            // 
            this.bandRate600rdBtn.AutoSize = true;
            this.bandRate600rdBtn.Location = new System.Drawing.Point(7, 19);
            this.bandRate600rdBtn.Name = "bandRate600rdBtn";
            this.bandRate600rdBtn.Size = new System.Drawing.Size(43, 17);
            this.bandRate600rdBtn.TabIndex = 0;
            this.bandRate600rdBtn.TabStop = true;
            this.bandRate600rdBtn.Text = "600";
            this.bandRate600rdBtn.UseVisualStyleBackColor = true;
            // 
            // dataBitGroup
            // 
            this.dataBitGroup.Controls.Add(this.dataBit8rdBtn);
            this.dataBitGroup.Controls.Add(this.dataBit7rdBtn);
            this.dataBitGroup.Controls.Add(this.dataBit5rdBtn);
            this.dataBitGroup.Controls.Add(this.dataBit6rdBtn);
            this.dataBitGroup.Location = new System.Drawing.Point(423, 3);
            this.dataBitGroup.Name = "dataBitGroup";
            this.dataBitGroup.Size = new System.Drawing.Size(65, 131);
            this.dataBitGroup.TabIndex = 2;
            this.dataBitGroup.TabStop = false;
            this.dataBitGroup.Text = "Data Bits";
            // 
            // dataBit8rdBtn
            // 
            this.dataBit8rdBtn.AutoSize = true;
            this.dataBit8rdBtn.Location = new System.Drawing.Point(6, 100);
            this.dataBit8rdBtn.Name = "dataBit8rdBtn";
            this.dataBit8rdBtn.Size = new System.Drawing.Size(31, 17);
            this.dataBit8rdBtn.TabIndex = 18;
            this.dataBit8rdBtn.TabStop = true;
            this.dataBit8rdBtn.Text = "8";
            this.dataBit8rdBtn.UseVisualStyleBackColor = true;
            // 
            // dataBit7rdBtn
            // 
            this.dataBit7rdBtn.AutoSize = true;
            this.dataBit7rdBtn.Location = new System.Drawing.Point(6, 73);
            this.dataBit7rdBtn.Name = "dataBit7rdBtn";
            this.dataBit7rdBtn.Size = new System.Drawing.Size(31, 17);
            this.dataBit7rdBtn.TabIndex = 17;
            this.dataBit7rdBtn.TabStop = true;
            this.dataBit7rdBtn.Text = "7";
            this.dataBit7rdBtn.UseVisualStyleBackColor = true;
            // 
            // dataBit5rdBtn
            // 
            this.dataBit5rdBtn.AutoSize = true;
            this.dataBit5rdBtn.Location = new System.Drawing.Point(6, 19);
            this.dataBit5rdBtn.Name = "dataBit5rdBtn";
            this.dataBit5rdBtn.Size = new System.Drawing.Size(31, 17);
            this.dataBit5rdBtn.TabIndex = 15;
            this.dataBit5rdBtn.TabStop = true;
            this.dataBit5rdBtn.Text = "5";
            this.dataBit5rdBtn.UseVisualStyleBackColor = true;
            // 
            // dataBit6rdBtn
            // 
            this.dataBit6rdBtn.AutoSize = true;
            this.dataBit6rdBtn.Location = new System.Drawing.Point(6, 46);
            this.dataBit6rdBtn.Name = "dataBit6rdBtn";
            this.dataBit6rdBtn.Size = new System.Drawing.Size(31, 17);
            this.dataBit6rdBtn.TabIndex = 16;
            this.dataBit6rdBtn.TabStop = true;
            this.dataBit6rdBtn.Text = "6";
            this.dataBit6rdBtn.UseVisualStyleBackColor = true;
            // 
            // parityGroup
            // 
            this.parityGroup.Controls.Add(this.paritySpaceRdBtn);
            this.parityGroup.Controls.Add(this.parityMarkRdBtn);
            this.parityGroup.Controls.Add(this.parityEvenRdBtn);
            this.parityGroup.Controls.Add(this.parityNoneRdBtn);
            this.parityGroup.Controls.Add(this.parityOddRdBtn);
            this.parityGroup.Location = new System.Drawing.Point(494, 3);
            this.parityGroup.Name = "parityGroup";
            this.parityGroup.Size = new System.Drawing.Size(65, 131);
            this.parityGroup.TabIndex = 19;
            this.parityGroup.TabStop = false;
            this.parityGroup.Text = "Parity";
            // 
            // paritySpaceRdBtn
            // 
            this.paritySpaceRdBtn.AutoSize = true;
            this.paritySpaceRdBtn.Location = new System.Drawing.Point(6, 107);
            this.paritySpaceRdBtn.Name = "paritySpaceRdBtn";
            this.paritySpaceRdBtn.Size = new System.Drawing.Size(54, 17);
            this.paritySpaceRdBtn.TabIndex = 19;
            this.paritySpaceRdBtn.TabStop = true;
            this.paritySpaceRdBtn.Text = "space";
            this.paritySpaceRdBtn.UseVisualStyleBackColor = true;
            // 
            // parityMarkRdBtn
            // 
            this.parityMarkRdBtn.AutoSize = true;
            this.parityMarkRdBtn.Location = new System.Drawing.Point(6, 85);
            this.parityMarkRdBtn.Name = "parityMarkRdBtn";
            this.parityMarkRdBtn.Size = new System.Drawing.Size(48, 17);
            this.parityMarkRdBtn.TabIndex = 18;
            this.parityMarkRdBtn.TabStop = true;
            this.parityMarkRdBtn.Text = "mark";
            this.parityMarkRdBtn.UseVisualStyleBackColor = true;
            // 
            // parityEvenRdBtn
            // 
            this.parityEvenRdBtn.AutoSize = true;
            this.parityEvenRdBtn.Location = new System.Drawing.Point(6, 63);
            this.parityEvenRdBtn.Name = "parityEvenRdBtn";
            this.parityEvenRdBtn.Size = new System.Drawing.Size(49, 17);
            this.parityEvenRdBtn.TabIndex = 17;
            this.parityEvenRdBtn.TabStop = true;
            this.parityEvenRdBtn.Text = "even";
            this.parityEvenRdBtn.UseVisualStyleBackColor = true;
            // 
            // parityNoneRdBtn
            // 
            this.parityNoneRdBtn.AutoSize = true;
            this.parityNoneRdBtn.Location = new System.Drawing.Point(6, 19);
            this.parityNoneRdBtn.Name = "parityNoneRdBtn";
            this.parityNoneRdBtn.Size = new System.Drawing.Size(49, 17);
            this.parityNoneRdBtn.TabIndex = 15;
            this.parityNoneRdBtn.TabStop = true;
            this.parityNoneRdBtn.Text = "none";
            this.parityNoneRdBtn.UseVisualStyleBackColor = true;
            // 
            // parityOddRdBtn
            // 
            this.parityOddRdBtn.AutoSize = true;
            this.parityOddRdBtn.Location = new System.Drawing.Point(6, 41);
            this.parityOddRdBtn.Name = "parityOddRdBtn";
            this.parityOddRdBtn.Size = new System.Drawing.Size(43, 17);
            this.parityOddRdBtn.TabIndex = 16;
            this.parityOddRdBtn.TabStop = true;
            this.parityOddRdBtn.Text = "odd";
            this.parityOddRdBtn.UseVisualStyleBackColor = true;
            // 
            // stopBitGroup
            // 
            this.stopBitGroup.Controls.Add(this.stopBit2RdBtn);
            this.stopBitGroup.Controls.Add(this.stopBit1RdBtn);
            this.stopBitGroup.Controls.Add(this.stopBit1_5RdBtn);
            this.stopBitGroup.Location = new System.Drawing.Point(565, 3);
            this.stopBitGroup.Name = "stopBitGroup";
            this.stopBitGroup.Size = new System.Drawing.Size(65, 131);
            this.stopBitGroup.TabIndex = 19;
            this.stopBitGroup.TabStop = false;
            this.stopBitGroup.Text = "Stop Bits";
            // 
            // stopBit2RdBtn
            // 
            this.stopBit2RdBtn.AutoSize = true;
            this.stopBit2RdBtn.Location = new System.Drawing.Point(6, 94);
            this.stopBit2RdBtn.Name = "stopBit2RdBtn";
            this.stopBit2RdBtn.Size = new System.Drawing.Size(40, 17);
            this.stopBit2RdBtn.TabIndex = 17;
            this.stopBit2RdBtn.TabStop = true;
            this.stopBit2RdBtn.Text = "2.0";
            this.stopBit2RdBtn.UseVisualStyleBackColor = true;
            // 
            // stopBit1RdBtn
            // 
            this.stopBit1RdBtn.AutoSize = true;
            this.stopBit1RdBtn.Location = new System.Drawing.Point(6, 19);
            this.stopBit1RdBtn.Name = "stopBit1RdBtn";
            this.stopBit1RdBtn.Size = new System.Drawing.Size(31, 17);
            this.stopBit1RdBtn.TabIndex = 15;
            this.stopBit1RdBtn.TabStop = true;
            this.stopBit1RdBtn.Text = "1";
            this.stopBit1RdBtn.UseVisualStyleBackColor = true;
            // 
            // stopBit1_5RdBtn
            // 
            this.stopBit1_5RdBtn.AutoSize = true;
            this.stopBit1_5RdBtn.Location = new System.Drawing.Point(6, 56);
            this.stopBit1_5RdBtn.Name = "stopBit1_5RdBtn";
            this.stopBit1_5RdBtn.Size = new System.Drawing.Size(40, 17);
            this.stopBit1_5RdBtn.TabIndex = 16;
            this.stopBit1_5RdBtn.TabStop = true;
            this.stopBit1_5RdBtn.Text = "1.5";
            this.stopBit1_5RdBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.groupBox8);
            this.groupBox6.Controls.Add(this.groupBox7);
            this.groupBox6.Location = new System.Drawing.Point(1233, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(181, 131);
            this.groupBox6.TabIndex = 20;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "CP2105";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.enhanGPIO_1chBx);
            this.groupBox8.Controls.Add(this.enhanGPIO_0chBx);
            this.groupBox8.Controls.Add(this.label2);
            this.groupBox8.Controls.Add(this.cpNumbEnhancedPortTxtBx);
            this.groupBox8.Location = new System.Drawing.Point(91, 19);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(82, 106);
            this.groupBox8.TabIndex = 1;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Enhanced";
            // 
            // enhanGPIO_1chBx
            // 
            this.enhanGPIO_1chBx.AutoSize = true;
            this.enhanGPIO_1chBx.Location = new System.Drawing.Point(8, 63);
            this.enhanGPIO_1chBx.Name = "enhanGPIO_1chBx";
            this.enhanGPIO_1chBx.Size = new System.Drawing.Size(64, 17);
            this.enhanGPIO_1chBx.TabIndex = 25;
            this.enhanGPIO_1chBx.Text = "GPIO_1";
            this.enhanGPIO_1chBx.UseVisualStyleBackColor = true;
            // 
            // enhanGPIO_0chBx
            // 
            this.enhanGPIO_0chBx.AutoSize = true;
            this.enhanGPIO_0chBx.Location = new System.Drawing.Point(8, 44);
            this.enhanGPIO_0chBx.Name = "enhanGPIO_0chBx";
            this.enhanGPIO_0chBx.Size = new System.Drawing.Size(64, 17);
            this.enhanGPIO_0chBx.TabIndex = 24;
            this.enhanGPIO_0chBx.Text = "GPIO_0";
            this.enhanGPIO_0chBx.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "COM";
            // 
            // cpNumbEnhancedPortTxtBx
            // 
            this.cpNumbEnhancedPortTxtBx.Location = new System.Drawing.Point(38, 18);
            this.cpNumbEnhancedPortTxtBx.Name = "cpNumbEnhancedPortTxtBx";
            this.cpNumbEnhancedPortTxtBx.Size = new System.Drawing.Size(34, 20);
            this.cpNumbEnhancedPortTxtBx.TabIndex = 22;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.standGPIO_2chBx);
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Controls.Add(this.standGPIO_1chBx);
            this.groupBox7.Controls.Add(this.cpNumbStandartPortTxtBx);
            this.groupBox7.Controls.Add(this.standGPIO_0chBx);
            this.groupBox7.Location = new System.Drawing.Point(7, 19);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(78, 104);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Standard";
            // 
            // standGPIO_2chBx
            // 
            this.standGPIO_2chBx.AutoSize = true;
            this.standGPIO_2chBx.Location = new System.Drawing.Point(8, 81);
            this.standGPIO_2chBx.Name = "standGPIO_2chBx";
            this.standGPIO_2chBx.Size = new System.Drawing.Size(64, 17);
            this.standGPIO_2chBx.TabIndex = 29;
            this.standGPIO_2chBx.Text = "GPIO_2";
            this.standGPIO_2chBx.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "COM";
            // 
            // standGPIO_1chBx
            // 
            this.standGPIO_1chBx.AutoSize = true;
            this.standGPIO_1chBx.Location = new System.Drawing.Point(8, 62);
            this.standGPIO_1chBx.Name = "standGPIO_1chBx";
            this.standGPIO_1chBx.Size = new System.Drawing.Size(64, 17);
            this.standGPIO_1chBx.TabIndex = 28;
            this.standGPIO_1chBx.Text = "GPIO_1";
            this.standGPIO_1chBx.UseVisualStyleBackColor = true;
            // 
            // cpNumbStandartPortTxtBx
            // 
            this.cpNumbStandartPortTxtBx.Location = new System.Drawing.Point(38, 18);
            this.cpNumbStandartPortTxtBx.Name = "cpNumbStandartPortTxtBx";
            this.cpNumbStandartPortTxtBx.Size = new System.Drawing.Size(34, 20);
            this.cpNumbStandartPortTxtBx.TabIndex = 21;
            // 
            // standGPIO_0chBx
            // 
            this.standGPIO_0chBx.AutoSize = true;
            this.standGPIO_0chBx.Location = new System.Drawing.Point(8, 43);
            this.standGPIO_0chBx.Name = "standGPIO_0chBx";
            this.standGPIO_0chBx.Size = new System.Drawing.Size(64, 17);
            this.standGPIO_0chBx.TabIndex = 27;
            this.standGPIO_0chBx.Text = "GPIO_0";
            this.standGPIO_0chBx.UseVisualStyleBackColor = true;
            // 
            // searchCP2105Ports
            // 
            this.searchCP2105Ports.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchCP2105Ports.Location = new System.Drawing.Point(1152, 8);
            this.searchCP2105Ports.Name = "searchCP2105Ports";
            this.searchCP2105Ports.Size = new System.Drawing.Size(75, 23);
            this.searchCP2105Ports.TabIndex = 2;
            this.searchCP2105Ports.Text = "Поиск";
            this.searchCP2105Ports.UseVisualStyleBackColor = true;
            this.searchCP2105Ports.Click += new System.EventHandler(this.searchCP2105Ports_Click);
            // 
            // terminalLogTxtBx
            // 
            this.terminalLogTxtBx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.terminalLogTxtBx.Location = new System.Drawing.Point(12, 170);
            this.terminalLogTxtBx.Multiline = true;
            this.terminalLogTxtBx.Name = "terminalLogTxtBx";
            this.terminalLogTxtBx.Size = new System.Drawing.Size(1402, 617);
            this.terminalLogTxtBx.TabIndex = 21;
            // 
            // textBox5
            // 
            this.textBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox5.Location = new System.Drawing.Point(12, 793);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(1294, 20);
            this.textBox5.TabIndex = 22;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(1312, 793);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 23);
            this.button2.TabIndex = 23;
            this.button2.Text = "Отправить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // modeGroup
            // 
            this.modeGroup.Controls.Add(this.modeTextRdBtn);
            this.modeGroup.Controls.Add(this.modeHexRdBtn);
            this.modeGroup.Location = new System.Drawing.Point(636, 3);
            this.modeGroup.Name = "modeGroup";
            this.modeGroup.Size = new System.Drawing.Size(65, 131);
            this.modeGroup.TabIndex = 20;
            this.modeGroup.TabStop = false;
            this.modeGroup.Text = "Mode";
            // 
            // modeTextRdBtn
            // 
            this.modeTextRdBtn.AutoSize = true;
            this.modeTextRdBtn.Location = new System.Drawing.Point(6, 19);
            this.modeTextRdBtn.Name = "modeTextRdBtn";
            this.modeTextRdBtn.Size = new System.Drawing.Size(46, 17);
            this.modeTextRdBtn.TabIndex = 15;
            this.modeTextRdBtn.TabStop = true;
            this.modeTextRdBtn.Text = "Text";
            this.modeTextRdBtn.UseVisualStyleBackColor = true;
            // 
            // modeHexRdBtn
            // 
            this.modeHexRdBtn.AutoSize = true;
            this.modeHexRdBtn.Location = new System.Drawing.Point(6, 42);
            this.modeHexRdBtn.Name = "modeHexRdBtn";
            this.modeHexRdBtn.Size = new System.Drawing.Size(44, 17);
            this.modeHexRdBtn.TabIndex = 16;
            this.modeHexRdBtn.TabStop = true;
            this.modeHexRdBtn.Text = "Hex";
            this.modeHexRdBtn.UseVisualStyleBackColor = true;
            // 
            // connectToModuleBtn
            // 
            this.connectToModuleBtn.Location = new System.Drawing.Point(186, 141);
            this.connectToModuleBtn.Name = "connectToModuleBtn";
            this.connectToModuleBtn.Size = new System.Drawing.Size(168, 23);
            this.connectToModuleBtn.TabIndex = 24;
            this.connectToModuleBtn.Text = "Связь с модулем";
            this.connectToModuleBtn.UseVisualStyleBackColor = true;
            this.connectToModuleBtn.Click += new System.EventHandler(this.connectToModuleBtn_Click);
            // 
            // connectToMKBtn
            // 
            this.connectToMKBtn.Location = new System.Drawing.Point(12, 141);
            this.connectToMKBtn.Name = "connectToMKBtn";
            this.connectToMKBtn.Size = new System.Drawing.Size(168, 23);
            this.connectToMKBtn.TabIndex = 25;
            this.connectToMKBtn.Text = "Связь с микроконтроллером";
            this.connectToMKBtn.UseVisualStyleBackColor = true;
            this.connectToMKBtn.Click += new System.EventHandler(this.connectToMKBtn_Click);
            // 
            // Terminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 859);
            this.Controls.Add(this.connectToMKBtn);
            this.Controls.Add(this.connectToModuleBtn);
            this.Controls.Add(this.modeGroup);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.terminalLogTxtBx);
            this.Controls.Add(this.searchCP2105Ports);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.stopBitGroup);
            this.Controls.Add(this.parityGroup);
            this.Controls.Add(this.dataBitGroup);
            this.Controls.Add(this.bandRateGroup);
            this.Controls.Add(this.groupBox1);
            this.Name = "Terminal";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Терминал";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Terminal_FormClosing);
            this.Load += new System.EventHandler(this.Terminal_Load);
            this.groupBox1.ResumeLayout(false);
            this.bandRateGroup.ResumeLayout(false);
            this.bandRateGroup.PerformLayout();
            this.dataBitGroup.ResumeLayout(false);
            this.dataBitGroup.PerformLayout();
            this.parityGroup.ResumeLayout(false);
            this.parityGroup.PerformLayout();
            this.stopBitGroup.ResumeLayout(false);
            this.stopBitGroup.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.modeGroup.ResumeLayout(false);
            this.modeGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comPortsListCmbBox;
        private System.Windows.Forms.Button connOrDisCOMBtn;
        private System.Windows.Forms.Button rescanCOMsBtn;
        private System.Windows.Forms.GroupBox bandRateGroup;
        private System.Windows.Forms.RadioButton customBandRateRdBtn;
        private System.Windows.Forms.RadioButton bandRate256000rdBtn;
        private System.Windows.Forms.RadioButton bandRate128000rdBtn;
        private System.Windows.Forms.RadioButton bandRate115200rdBtn;
        private System.Windows.Forms.RadioButton bandRate57600rdBtn;
        private System.Windows.Forms.RadioButton bandRate56000rdBtn;
        private System.Windows.Forms.RadioButton bandRate38400rdBtn;
        private System.Windows.Forms.RadioButton bandRate28800rdBtn;
        private System.Windows.Forms.RadioButton bandRate19200rdBtn;
        private System.Windows.Forms.RadioButton bandRate14400rdBtn;
        private System.Windows.Forms.RadioButton bandRate9600rdBtn;
        private System.Windows.Forms.RadioButton bandRate4800rdBtn;
        private System.Windows.Forms.RadioButton bandRate2400rdBtn;
        private System.Windows.Forms.RadioButton bandRate1200rdBtn;
        private System.Windows.Forms.RadioButton bandRate600rdBtn;
        private System.Windows.Forms.TextBox customBandRateTxtBx;
        private System.Windows.Forms.GroupBox dataBitGroup;
        private System.Windows.Forms.RadioButton dataBit8rdBtn;
        private System.Windows.Forms.RadioButton dataBit7rdBtn;
        private System.Windows.Forms.RadioButton dataBit5rdBtn;
        private System.Windows.Forms.RadioButton dataBit6rdBtn;
        private System.Windows.Forms.GroupBox parityGroup;
        private System.Windows.Forms.RadioButton parityMarkRdBtn;
        private System.Windows.Forms.RadioButton parityEvenRdBtn;
        private System.Windows.Forms.RadioButton parityNoneRdBtn;
        private System.Windows.Forms.RadioButton parityOddRdBtn;
        private System.Windows.Forms.RadioButton paritySpaceRdBtn;
        private System.Windows.Forms.GroupBox stopBitGroup;
        private System.Windows.Forms.RadioButton stopBit2RdBtn;
        private System.Windows.Forms.RadioButton stopBit1RdBtn;
        private System.Windows.Forms.RadioButton stopBit1_5RdBtn;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox cpNumbEnhancedPortTxtBx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox cpNumbStandartPortTxtBx;
        private System.Windows.Forms.CheckBox enhanGPIO_1chBx;
        private System.Windows.Forms.CheckBox enhanGPIO_0chBx;
        private System.Windows.Forms.CheckBox standGPIO_2chBx;
        private System.Windows.Forms.CheckBox standGPIO_1chBx;
        private System.Windows.Forms.CheckBox standGPIO_0chBx;
        private System.Windows.Forms.Button searchCP2105Ports;
        private System.Windows.Forms.TextBox terminalLogTxtBx;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox modeGroup;
        private System.Windows.Forms.RadioButton modeTextRdBtn;
        private System.Windows.Forms.RadioButton modeHexRdBtn;
        private System.Windows.Forms.Button indBtn;
        private System.Windows.Forms.Button connectToModuleBtn;
        private System.Windows.Forms.Button connectToMKBtn;
    }
}