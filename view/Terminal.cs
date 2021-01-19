using GSM_NBIoT_Module.classes;
using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using static GSM_NBIoT_Module.classes.CP2105_Connector;

namespace GSM_NBIoT_Module.view {
    public partial class Terminal : Form {

        CP2105_Connector cP2105 = CP2105_Connector.GetCP2105_ConnectorInstance();

        //COM порт для подключения
        SerialPort serialPort = new SerialPort();

        //Если ли сейчас подключение к ком порту
        private bool connect = false;

        //Стандартный цвет кнопки
        private Color defaultColor;

        public Terminal() {
            InitializeComponent();
        }

        private void Terminal_Load(object sender, EventArgs e) {
            //Индикатор
            defaultColor = indBtn.BackColor;
            indBtn.Enabled = false;

            //Устанавливаю слушатели

            serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);

            foreach (Control radBtn in bandRateGroup.Controls) {
                radBtn.Click += selectBandRate_Click;
            }

            //Бит данных
            foreach (Control rdBtn in dataBitGroup.Controls) {
                rdBtn.Click += selectDataBist_Click;
            }

            //Бит четности
            foreach (Control rdBtn in parityGroup.Controls) {
                rdBtn.Click += selectParity_Click;
            }

            //Стоповый бит
            foreach (Control rdBtn in stopBitGroup.Controls) {
                rdBtn.Click += selectStopBit_Click;
            }

            //Устанавливаю слушателя к чекбоксам управления состоянием GPIO
            standGPIO_0chBx.CheckedChanged += standardGPIO_CheckedChanged;
            standGPIO_1chBx.CheckedChanged += standardGPIO_CheckedChanged;
            standGPIO_2chBx.CheckedChanged += standardGPIO_CheckedChanged;

            enhanGPIO_0chBx.CheckedChanged += enhabcedGPIO_CheckedChanged;
            enhanGPIO_1chBx.CheckedChanged += enhabcedGPIO_CheckedChanged;

            //Инициализация списка COM портов
            comPortsListCmbBox.Items.AddRange(SerialPort.GetPortNames());

            if (comPortsListCmbBox.Items.Count > 0) comPortsListCmbBox.SelectedIndex = 0;

            //Установка прошлых сохранённых параметров
            comPortsListCmbBox.Text = Properties.Settings.Default.terminal_LastCOMPortNo;

            //Скорость передачи
            int bandRate = Convert.ToInt32(Properties.Settings.Default.terminal_LastBandRate);
            bool find = false;
            foreach (Control rb in bandRateGroup.Controls) {

                if (rb is RadioButton) {
                    try {
                        if (Convert.ToInt32(rb.Text) == bandRate) {
                            ((RadioButton)rb).PerformClick();
                            find = true;
                            break;
                        }
                    } catch (FormatException) { }
                }
            }

            if (!find) {
                customBandRateRdBtn.PerformClick();
                customBandRateTxtBx.Text = bandRate.ToString();
                customBandRateTxtBx.Enabled = true;
            }

            //DataBit
            int dataBit = Convert.ToInt32(Properties.Settings.Default.terminal_LastDataBit);

            foreach (RadioButton rb in dataBitGroup.Controls) {

                if (dataBit == Convert.ToInt32(rb.Text)) {
                    ((RadioButton)rb).PerformClick();
                    break;
                }
            }

            //Parity
            switch (Properties.Settings.Default.terminal_LastParity) {
                case "Even": {
                        parityEvenRdBtn.PerformClick();
                    } break;

                case "Mark": {
                        parityMarkRdBtn.PerformClick();
                    } break;

                case "None": {
                        parityNoneRdBtn.PerformClick();
                    } break;

                case "Odd": {
                        parityOddRdBtn.PerformClick();
                    } break;

                case "Space": {
                        paritySpaceRdBtn.PerformClick();
                    } break;
            }

            //StopBit
            switch(Properties.Settings.Default.terminal_LastStopBit) {
                case "One": {
                        stopBit1RdBtn.PerformClick();

                    } break;
                case "OnePointFive": {
                        stopBit1_5RdBtn.PerformClick();

                    } break;
                case "Two": {
                        stopBit2RdBtn.PerformClick();

                } break;
            }

            //Мод
            if (Properties.Settings.Default.terminal_LastMode.Equals("Text")) {
                modeTextRdBtn.PerformClick();
            } else if (Properties.Settings.Default.terminal_LastMode.Equals("Hex")) {
                modeHexRdBtn.PerformClick();
            }

        }

        /// <summary>
        /// Действие при нажатии кнопки подключится к COM порту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connOrDisCOMBtn_Click(object sender, EventArgs e) {

            if (!connect) {
                
                //Установка COM порта
                if (String.IsNullOrEmpty(comPortsListCmbBox.Text.Trim())) {
                    Flasher.exceptionDialog("Выберите номер COM порта");
                    return;
                }

                try {
                    serialPort.PortName = comPortsListCmbBox.Text.Trim();
                } catch (ArgumentException) {
                    Flasher.exceptionDialog("Неверный формат имени COM порта");
                    return;
                }
                
                //Установка скорости
                //Если выбрана пользователькая скорость
                if (customBandRateRdBtn.Checked) {

                    if (String.IsNullOrEmpty(customBandRateTxtBx.Text.Trim())) {
                        Flasher.exceptionDialog("Укажите скорость обмена данными по COM порту");
                        return;

                    }

                    try {
                        int bandRateCustom = Convert.ToInt32(customBandRateTxtBx.Text.Trim());

                        try {
                            serialPort.BaudRate = bandRateCustom;
                        } catch (ArgumentOutOfRangeException) {
                            Flasher.exceptionDialog("Значение \"скорость передачи данных по COM порту\" находится вне диапазона допустимых значенй");
                            return;
                        }

                    } catch (FormatException) {
                        Flasher.exceptionDialog("Значение скорости обмена данными по COM порту должно быть числовым");
                        return;
                    }
                }

                //Открытие СOM порта
                try {
                    serialPort.Open();
                } catch (Exception ex) {
                    Flasher.exceptionDialog("Возникла ошибка при открытии COM порта:\n" + ex.Message);
                    return;
                }

                enablePortSettings(false);
                indBtn.BackColor = Color.Lime;
                connect = true;
                connOrDisCOMBtn.Text = "Disconnect";

                //Если порт уже открыт
            } else {

                try {
                    serialPort.Close();
                } catch (IOException ex) {
                    Flasher.exceptionDialog("Возникла ошибка при закрытии COM порта:\n" + ex.Message);
                    return;
                }

                enablePortSettings(true);
                indBtn.BackColor = defaultColor;
                connect = false;
                connOrDisCOMBtn.Text = "Connect";
            }
        }

        /// <summary>
        /// Обработка действия пользователя при изменения параметра скорости передачи порта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectBandRate_Click(object sender, EventArgs e) {

            //Изменение скорости передачи данных
            if (sender != customBandRateTxtBx) {

                if (sender == customBandRateRdBtn) {
                    customBandRateTxtBx.Enabled = true;

                } else {
                    serialPort.BaudRate = Convert.ToInt32(((RadioButton)sender).Text);
                    customBandRateTxtBx.Text = "";
                    customBandRateTxtBx.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Действие при изменении количества бит
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectDataBist_Click(object sender, EventArgs e) {

            foreach (RadioButton radBtn in dataBitGroup.Controls) {

                if (sender == radBtn) {
                    serialPort.DataBits = Convert.ToInt32(((RadioButton)sender).Text);
                }
            }
        }

        /// <summary>
        /// Действие при изменении бита чётности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectParity_Click(object sender, EventArgs e) {

            if (sender == parityNoneRdBtn) {
                serialPort.Parity = Parity.None;

            } else if (sender == parityOddRdBtn) {
                serialPort.Parity = Parity.Odd;

            } else if (sender == parityEvenRdBtn) {
                serialPort.Parity = Parity.Even;

            } else if (sender == parityMarkRdBtn) {
                serialPort.Parity = Parity.Mark;

            } else if (sender == paritySpaceRdBtn) {
                serialPort.Parity = Parity.Space;
            }
        }

        /// <summary>
        /// /// Действие при изменении Стопового бита
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectStopBit_Click(object sender, EventArgs e) {

            if (sender == stopBit1RdBtn) {
                serialPort.StopBits = StopBits.One;

            } else if (sender == stopBit1_5RdBtn) {
                serialPort.StopBits = StopBits.OnePointFive;

            } else if (sender == stopBit2RdBtn) {
                serialPort.StopBits = StopBits.Two;
            }
        }


        /// <summary>
        /// Блокирует группы с настройками ком порта
        /// </summary>
        /// <param name="enable"></param>
        private void enablePortSettings(bool enable) {
            bandRateGroup.Enabled = enable;
            dataBitGroup.Enabled = enable;
            parityGroup.Enabled = enable;
            stopBitGroup.Enabled = enable;
        }

        private void rescanCOMsBtn_Click(object sender, EventArgs e) {
            comPortsListCmbBox.Items.Clear();
            comPortsListCmbBox.Items.AddRange(SerialPort.GetPortNames());

            if (comPortsListCmbBox.Items.Count > 0) {

                if (connect) {
                    comPortsListCmbBox.Text = serialPort.PortName;
                } else {
                    comPortsListCmbBox.SelectedIndex = 0;
                }
            }
        }

        private void searchCP2105Ports_Click(object sender, EventArgs e) {

            Cursor = Cursors.WaitCursor;

            //Было ли подключение к COM порту
            bool connectToCOM = connect;

            cpNumbStandartPortTxtBx.Text = "";
            cpNumbEnhancedPortTxtBx.Text = "";

            //Если есть подключение, то отключаюсь
            if (connectToCOM) {
                connOrDisCOMBtn.PerformClick();
            }

            //Проверка, что подключен один модем
            try {
                cP2105.amountDevicesConnect();

            }catch (CP_Error ex) {

                Flasher.exceptionDialog(ex.Message);
                Cursor = Cursors.Default;

                if (connectToCOM) {
                    connOrDisCOMBtn.PerformClick();
                }
                return;
            }

            //Ищу порты
            try {
                cP2105.FindDevicePorts();

            }catch (DeviceNotFoundException ex) {

                Flasher.exceptionDialog(ex.Message);
                Cursor = Cursors.Default;

                if (connectToCOM) {
                   connOrDisCOMBtn.PerformClick();
                }
                return;
            }

            int stand = cP2105.getStandardPort();
            int enh = cP2105.getEnhancedPort();

            cpNumbStandartPortTxtBx.Text = stand.ToString();
            cpNumbEnhancedPortTxtBx.Text = enh.ToString();

            StateGPIO_OnStandardPort stageGPIOSta = cP2105.GetStageGPIOStandardPort();
            standGPIO_0chBx.Checked = stageGPIOSta.stageGPIO_0;
            standGPIO_1chBx.Checked = stageGPIOSta.stageGPIO_1;
            standGPIO_2chBx.Checked = stageGPIOSta.stageGPIO_2;

            StateGPIO_OnEnhabcedPort stageGPIOEnh = cP2105.GetStageGPIOEnhabcedPort();
            enhanGPIO_0chBx.Checked = stageGPIOEnh.stageGPIO_0;
            enhanGPIO_1chBx.Checked = stageGPIOEnh.stageGPIO_1;

            Cursor = Cursors.Default;

            //Снова подключаюсь если было подключение
            if (connectToCOM) {
                connOrDisCOMBtn.PerformClick();
            }
        }

        private void standardGPIO_CheckedChanged(object sender, EventArgs e) {

            if (String.IsNullOrEmpty(cpNumbStandartPortTxtBx.Text.Trim())) {
                Flasher.exceptionDialog("Ввдите значение \"COM\" Standard порта");
                return;
            }

            int stdPortNo;
            try {
                stdPortNo = Convert.ToInt32(cpNumbStandartPortTxtBx.Text.Trim());
            } catch (FormatException) {
                Flasher.exceptionDialog("Значение поля \"COM\" Standard порта должно быть числовым");
                return;
            }

            Cursor = Cursors.WaitCursor;

            //Было ли подключение к COM порту
            bool connectToCOM = connect;

            //Если есть подключение, то отключаюсь
            if (connectToCOM) {
                connOrDisCOMBtn.PerformClick();
            }

            try {
                cP2105.WriteGPIOStageAndSetFlags(stdPortNo, standGPIO_0chBx.Checked, standGPIO_1chBx.Checked, standGPIO_2chBx.Checked, 100, false);
            } catch (CP_Error ex) {

                Flasher.exceptionDialog(ex.Message);
                Cursor = Cursors.Default;
                return;
            }

            Cursor = Cursors.Default;

            if (connectToCOM) {
                connOrDisCOMBtn.PerformClick();
            }
        }

        private void enhabcedGPIO_CheckedChanged(object sender, EventArgs e) {

            if (String.IsNullOrEmpty(cpNumbEnhancedPortTxtBx.Text.Trim())) {
                Flasher.exceptionDialog("Ввдите значение \"COM\" Standard порта");
                return;
            }

            int stdPortNo;
            try {
                stdPortNo = Convert.ToInt32(cpNumbEnhancedPortTxtBx.Text.Trim());
            } catch (FormatException) {
                Flasher.exceptionDialog("Значение поля \"COM\" Standard порта должно быть числовым");
                return;
            }

            Cursor = Cursors.WaitCursor;

            //Было ли подключение к COM порту
            bool connectToCOM = connect;

            //Если есть подключение, то отключаюсь
            if (connectToCOM) {
                connOrDisCOMBtn.PerformClick();
            }

            try {
                cP2105.WriteGPIOStageAndSetFlags(stdPortNo, enhanGPIO_0chBx.Checked, enhanGPIO_1chBx.Checked, 100, false);
            } catch (CP_Error ex) {

                Flasher.exceptionDialog(ex.Message);
                Cursor = Cursors.Default;
                return;
            }

            Cursor = Cursors.Default;

            if (connectToCOM) {
                connOrDisCOMBtn.PerformClick();
            }
        }

        private void connectToMKBtn_Click(object sender, EventArgs e) {

            if (connect) {
                connOrDisCOMBtn.PerformClick();
            }

            int sta;
            int enh;

            Cursor = Cursors.WaitCursor;
            
            //Если поле с COM портом пустое
            if (String.IsNullOrEmpty(cpNumbStandartPortTxtBx.Text.Trim()) ||
                String.IsNullOrEmpty(cpNumbEnhancedPortTxtBx.Text.Trim())) {
                //Ищу модем
                searchCP2105Ports.PerformClick();

                //если не удалось найти, то значит сработала ошибка, выхожу
                if (String.IsNullOrEmpty(cpNumbStandartPortTxtBx.Text.Trim()) ||
                    String.IsNullOrEmpty(cpNumbEnhancedPortTxtBx.Text.Trim())) {
                    Cursor = Cursors.Default;
                    return;

                } else {
                    sta = Convert.ToInt32(cpNumbStandartPortTxtBx.Text.Trim());
                    enh = Convert.ToInt32(cpNumbEnhancedPortTxtBx.Text.Trim());
                }
                
                //Если есть какое-то значение
            } else {

                //то проверяю на валидность
                try {
                    sta = Convert.ToInt32(cpNumbStandartPortTxtBx.Text.Trim());
                    enh = Convert.ToInt32(cpNumbEnhancedPortTxtBx.Text.Trim()); 
                } catch (FormatException) {
                    //Повторяю поиск
                    searchCP2105Ports.PerformClick();

                    //если не удалось найти, то значит сработала ошибка, выхожу
                    if (String.IsNullOrEmpty(cpNumbStandartPortTxtBx.Text.Trim()) ||
                        String.IsNullOrEmpty(cpNumbEnhancedPortTxtBx.Text.Trim())) {

                        Cursor = Cursors.Default;
                        return;

                    } else {
                        sta = Convert.ToInt32(cpNumbStandartPortTxtBx.Text.Trim());
                        enh = Convert.ToInt32(cpNumbEnhancedPortTxtBx.Text.Trim());
                    }
                }
            }

            //Если удалось найти порт
            if (sta != 0 && enh != 0) {

                try {
                    cP2105.WriteGPIOStageAndSetFlags(enh, true, true, 100, false);

                    StateGPIO_OnEnhabcedPort stageEnh = cP2105.GetStageGPIOEnhabcedPort();
                    enhanGPIO_0chBx.Checked = stageEnh.stageGPIO_0;
                    enhanGPIO_1chBx.Checked = stageEnh.stageGPIO_1;

                    cP2105.WriteGPIOStageAndSetFlags(sta, true, true, true, 100, false);

                    StateGPIO_OnStandardPort stageSta = cP2105.GetStageGPIOStandardPort();
                    standGPIO_0chBx.Checked = stageSta.stageGPIO_0;
                    standGPIO_1chBx.Checked = stageSta.stageGPIO_1;
                    standGPIO_2chBx.Checked = stageSta.stageGPIO_2;

                } catch (CP_Error ex) {
                    Flasher.exceptionDialog(ex.Message);
                    Cursor = Cursors.Default;
                    return;
                }

                comPortsListCmbBox.Text = "COM" + enh;

                bandRate128000rdBtn.PerformClick();
                dataBit8rdBtn.PerformClick();
                parityNoneRdBtn.PerformClick();
                stopBit1RdBtn.PerformClick();

                connOrDisCOMBtn.PerformClick();
            }

            Cursor = Cursors.Default;
        }

        private void connectToModuleBtn_Click(object sender, EventArgs e) {

            if (connect) {
                connOrDisCOMBtn.PerformClick();
            }

            int sta;
            int enh;

            Cursor = Cursors.WaitCursor;

            //Если поле с COM портом пустое
            if (String.IsNullOrEmpty(cpNumbStandartPortTxtBx.Text.Trim()) ||
                String.IsNullOrEmpty(cpNumbEnhancedPortTxtBx.Text.Trim())) {
                //Ищу модем
                searchCP2105Ports.PerformClick();

                //если не удалось найти, то значит сработала ошибка, выхожу
                if (String.IsNullOrEmpty(cpNumbStandartPortTxtBx.Text.Trim()) ||
                    String.IsNullOrEmpty(cpNumbEnhancedPortTxtBx.Text.Trim())) {
                    Cursor = Cursors.Default;
                    return;

                } else {
                    sta = Convert.ToInt32(cpNumbStandartPortTxtBx.Text.Trim());
                    enh = Convert.ToInt32(cpNumbEnhancedPortTxtBx.Text.Trim());
                }

                //Если есть какое-то значение
            } else {

                //то проверяю на валидность
                try {
                    sta = Convert.ToInt32(cpNumbStandartPortTxtBx.Text.Trim());
                    enh = Convert.ToInt32(cpNumbEnhancedPortTxtBx.Text.Trim());
                } catch (FormatException) {
                    //Повторяю поиск
                    searchCP2105Ports.PerformClick();

                    //если не удалось найти, то значит сработала ошибка, выхожу
                    if (String.IsNullOrEmpty(cpNumbStandartPortTxtBx.Text.Trim()) ||
                        String.IsNullOrEmpty(cpNumbEnhancedPortTxtBx.Text.Trim())) {

                        Cursor = Cursors.Default;
                        return;

                    } else {
                        sta = Convert.ToInt32(cpNumbStandartPortTxtBx.Text.Trim());
                        enh = Convert.ToInt32(cpNumbEnhancedPortTxtBx.Text.Trim());
                    }
                }
            }

            if (enh != 0 && sta != 0) {

                try {
                    cP2105.WriteGPIOStageAndSetFlags(enh, true, false, 100, false);

                    StateGPIO_OnEnhabcedPort stageEnh = cP2105.GetStageGPIOEnhabcedPort();
                    enhanGPIO_0chBx.Checked = stageEnh.stageGPIO_0;
                    enhanGPIO_1chBx.Checked = stageEnh.stageGPIO_1;

                    cP2105.WriteGPIOStageAndSetFlags(sta, true, true, false, 100, false);

                    StateGPIO_OnStandardPort stageSta = cP2105.GetStageGPIOStandardPort();
                    standGPIO_0chBx.Checked = stageSta.stageGPIO_0;
                    standGPIO_1chBx.Checked = stageSta.stageGPIO_1;
                    standGPIO_2chBx.Checked = stageSta.stageGPIO_2;

                } catch (CP_Error ex) {
                    Flasher.exceptionDialog(ex.Message);
                    Cursor = Cursors.Default;
                    return;
                }

                comPortsListCmbBox.Text = "COM" + enh;
                bandRate9600rdBtn.PerformClick();
                dataBit8rdBtn.PerformClick();
                parityNoneRdBtn.PerformClick();
                stopBit1RdBtn.PerformClick();

                connOrDisCOMBtn.PerformClick();
            }

            Cursor = Cursors.Default;
        }

        private void Terminal_FormClosing(object sender, FormClosingEventArgs e) {
            try {
                serialPort.Close();

            } catch (IOException ex) {
                e.Cancel = true;

                Flasher.addMessageInMainLog("Произошла ошибка при закрытии COM порта\n" + ex.Message + "\nЗакройте COM порт");
            }
        }

        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            
            terminalLogTxtBx.Invoke((MethodInvoker)delegate {
                terminalLogTxtBx.AppendText(serialPort.ReadLine() + "\r\n");
            });
        }

        private void button2_Click(object sender, EventArgs e) {
            if (serialPort.IsOpen) {
                serialPort.WriteLine(textBox5.Text + "\r\n");
            }

            textBox5.Text = "";
        }
    }
}