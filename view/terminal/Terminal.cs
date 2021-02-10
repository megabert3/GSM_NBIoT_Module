using GSM_NBIoT_Module.classes;
using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using GSM_NBIoT_Module.classes.terminal;
using GSM_NBIoT_Module.view.terminal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private ToolTip toolTipForMacros;

        //Окно с настройкой макросов
        private MacrosSettings macrosSettingsFlame;

        //Буфер посленних введённых комманд
        private List<string> lastCommandsList = new List<string>();

        //Индекс выбора последних введённых комманд
        private int lastCommandChoiseIndex = 0;

        //Лист с макросами которые запущены в цикле
        public List<string> threadMacroses = new List<string>();

        //Байт новой строки
        public int LF_Byte { get; set; }
        //Байт переноса каретки
        public int CR_Byte { get; set; }

        public Terminal() {
            InitializeComponent();
        }

        private void Terminal_Load(object sender, EventArgs e) {

            LF_Byte = 10;
            CR_Byte = 13;

            //Индикатор
            defaultColor = indBtn.BackColor;
            indBtn.Enabled = false;

            refreshMacrosBtns();

            setToolTipsTerminalFrame();

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
                    }
                    break;

                case "Mark": {
                        parityMarkRdBtn.PerformClick();
                    }
                    break;

                case "None": {
                        parityNoneRdBtn.PerformClick();
                    }
                    break;

                case "Odd": {
                        parityOddRdBtn.PerformClick();
                    }
                    break;

                case "Space": {
                        paritySpaceRdBtn.PerformClick();
                    }
                    break;
            }

            //StopBit
            switch (Properties.Settings.Default.terminal_LastStopBit) {
                case "One": {
                        stopBit1RdBtn.PerformClick();

                    }
                    break;
                case "OnePointFive": {
                        stopBit1_5RdBtn.PerformClick();

                    }
                    break;
                case "Two": {
                        stopBit2RdBtn.PerformClick();

                    }
                    break;
            }

            //Вывод информации из лога
            if (Properties.Settings.Default.terminal_inputLastMode.Equals("Text")) {
                inputModeTextRdBtn.PerformClick();
            } else if (Properties.Settings.Default.terminal_inputLastMode.Equals("Hex")) {
                inputModeHexRdBtn.PerformClick();
            }

            //ввод информации в лог
            if (Properties.Settings.Default.terminal_outLastMode.Equals("Text")) {
                modeTextRdBtn.PerformClick();
            } else if (Properties.Settings.Default.terminal_outLastMode.Equals("Hex")) {
                modeHexRdBtn.PerformClick();
            }

            if (Properties.Settings.Default.terminal_CLequalsRF) {
                clEqualsRf.Checked = true;
            } else {
                clEqualsRf.Checked = false;
            }

            if (Properties.Settings.Default.terminal_plus_CL_RF) {
                addEndLine.Checked = true;
            } else {
                addEndLine.Checked = false;
            }

            //Установка слушателя кнопкам макросов
            foreach (Control tabPage in macrosTabControl.Controls) {
                foreach (Control btn in tabPage.Controls) {
                    btn.Click += macrosBtns_Click;
                }
            }

            refreshToolTipsForMacros();

            //Заполняю лист пустыми значениями
            for (int i = 0; i < 11; i++) {
                lastCommandsList.Add("");
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

                //Сохраняю последние используемые настройки
                Properties.Settings.Default.terminal_LastCOMPortNo = comPortsListCmbBox.Text;
                Properties.Settings.Default.terminal_LastBandRate = serialPort.BaudRate.ToString();
                Properties.Settings.Default.terminal_LastDataBit = serialPort.DataBits.ToString();
                Properties.Settings.Default.terminal_LastParity = serialPort.Parity.ToString();
                Properties.Settings.Default.terminal_LastStopBit = serialPort.StopBits.ToString();

                if (inputModeTextRdBtn.Checked) {
                    Properties.Settings.Default.terminal_inputLastMode = "Text";
                } else {
                    Properties.Settings.Default.terminal_inputLastMode = "Hex";
                }

                if (modeTextRdBtn.Checked) {
                    Properties.Settings.Default.terminal_outLastMode = "Text";
                } else {
                    Properties.Settings.Default.terminal_outLastMode = "Hex";
                }

                Properties.Settings.Default.Save();

                enablePortSettings(false);
                indBtn.BackColor = Color.Lime;
                connect = true;
                connOrDisCOMBtn.Text = "Disconnect";

                //Если порт уже открыт
            } else {

                try {
                    abortAllCycleMacros();

                    Task.Factory.StartNew(() => {
                        try {
                            serialPort.DiscardOutBuffer();
                            serialPort.DiscardInBuffer();
                            serialPort.Close();

                        }catch (InvalidOperationException) {
                            connect = false;
                            enablePortSettings(true);
                            indBtn.BackColor = defaultColor;

                            connOrDisCOMBtn.Invoke((MethodInvoker)delegate {
                                connOrDisCOMBtn.Text = "Connect";
                            });
                        }
                    });

                } catch (IOException ex) {
                    Flasher.exceptionDialog("Возникла ошибка при закрытии COM порта:\n" + ex.Message);
                    return;
                }

                connect = false;
                enablePortSettings(true);
                indBtn.BackColor = defaultColor;
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
            bandRateGroup.Invoke((MethodInvoker)delegate {
                bandRateGroup.Enabled = enable;
            });

            dataBitGroup.Invoke((MethodInvoker)delegate {
                dataBitGroup.Enabled = enable;
            });

            parityGroup.Invoke((MethodInvoker)delegate {
                parityGroup.Enabled = enable;
            });

            stopBitGroup.Invoke((MethodInvoker)delegate {
                stopBitGroup.Enabled = enable;
            });
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

            } catch (CP_Error ex) {

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

            } catch (DeviceNotFoundException ex) {

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

            try {
                StateGPIO_OnStandardPort stageGPIOSta = cP2105.GetStageGPIOStandardPort();
                standGPIO_0chBx.Checked = stageGPIOSta.stageGPIO_0;
                standGPIO_1chBx.Checked = stageGPIOSta.stageGPIO_1;
                standGPIO_2chBx.Checked = stageGPIOSta.stageGPIO_2;

                StateGPIO_OnEnhabcedPort stageGPIOEnh = cP2105.GetStageGPIOEnhabcedPort();
                enhanGPIO_0chBx.Checked = stageGPIOEnh.stageGPIO_0;
                enhanGPIO_1chBx.Checked = stageGPIOEnh.stageGPIO_1;

            } catch (CP_Error ex) {
                Flasher.exceptionDialog(ex.Message);
            }

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

                customBandRateRdBtn.PerformClick();
                customBandRateTxtBx.Text = "125000";
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

                abortAllCycleMacros();

                if (connect) {
                    connOrDisCOMBtn.PerformClick();
                }

            } catch (IOException ex) {
                e.Cancel = true;

                Flasher.addMessageInMainLog("Произошла ошибка при закрытии COM порта\n" + ex.Message);
            }
        }

        private int dataByte;
        //Байт с новой строки т.е. необходимо добавить таймштамп
        private bool first = true;
        private StringBuilder parseAnswer = new StringBuilder("");
        private string localString;
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            try {
                while (serialPort.BytesToRead > 0 && connect) {

                    terminalLogRichTxtBx.Invoke((MethodInvoker)delegate {

                        try {
                            dataByte = serialPort.ReadByte();
                        }catch (InvalidOperationException) {
                            //Пользователь нажал закрыть порт или он отвалился
                        }

                        //Если режим текста
                        if (inputModeTextRdBtn.Checked) {
                            
                            if (dataByte == CR_Byte) {
                                //Если установлен флаг, что /r == /n
                                if (clEqualsRf.Checked) {
                                    if (first) {
                                        terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + Environment.NewLine);
                                        terminalLogRichTxtBx.ScrollToCaret();

                                    } else {
                                        terminalLogRichTxtBx.AppendText(Environment.NewLine);
                                        terminalLogRichTxtBx.ScrollToCaret();

                                    }
                                    appendTextToParseLog();
                                    first = true;
                                }

                            } else if (dataByte == LF_Byte) {
                                if (first) {
                                    terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + Environment.NewLine);
                                    terminalLogRichTxtBx.ScrollToCaret();

                                } else {
                                    terminalLogRichTxtBx.AppendText(Environment.NewLine);
                                    terminalLogRichTxtBx.ScrollToCaret();
                                }

                                appendTextToParseLog();
                                first = true;

                            } else {

                                if (first) {
                                    localString = DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + Convert.ToChar(dataByte).ToString();

                                    terminalLogRichTxtBx.AppendText(localString);
                                    parseAnswer.Append(localString);
                                    first = false;
                                    terminalLogRichTxtBx.ScrollToCaret();

                                } else {
                                    localString = Convert.ToChar(dataByte).ToString();
                                    terminalLogRichTxtBx.AppendText(localString);
                                    parseAnswer.Append(localString);
                                }
                            }

                            //=============================================Старая логика
                            /*switch (dataByte) {

                                //Эквивалентно /r
                                case 13: {
                                        //Если установлен флаг, что /r == /n
                                        if (clEqualsRf.Checked) {
                                            if (first) {
                                                terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + Environment.NewLine);
                                                terminalLogRichTxtBx.ScrollToCaret();

                                            } else {
                                                terminalLogRichTxtBx.AppendText(Environment.NewLine);
                                                terminalLogRichTxtBx.ScrollToCaret();

                                            }
                                            appendTextToParseLog();
                                            first = true;
                                        }
                                    }
                                    break;

                                //Эквивалентно /n
                                case LF_Byte: {
                                        if (first) {
                                            terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + Environment.NewLine);
                                            terminalLogRichTxtBx.ScrollToCaret();

                                        } else {
                                            terminalLogRichTxtBx.AppendText(Environment.NewLine);
                                            terminalLogRichTxtBx.ScrollToCaret();
                                        }

                                        appendTextToParseLog();
                                        first = true;
                                    }
                                    break;

                                default: {
                                        if (first) {
                                            localString = DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + Convert.ToChar(dataByte).ToString();

                                            terminalLogRichTxtBx.AppendText(localString);
                                            parseAnswer.Append(localString);
                                            first = false;
                                            terminalLogRichTxtBx.ScrollToCaret();

                                        } else {
                                            localString = Convert.ToChar(dataByte).ToString();
                                            terminalLogRichTxtBx.AppendText(localString);
                                            parseAnswer.Append(localString);
                                        }
                                    }
                                    break;
                            }*/

                            //Если режим вывода информации HEX
                        } else {
                            //Эквивалентно /r
                            if (dataByte == CR_Byte) {
                                //Если установлен флаг, что /r == /n
                                if (clEqualsRf.Checked) {
                                    if (first) {
                                        terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + dataByte.ToString("X2") + Environment.NewLine);
                                        terminalLogRichTxtBx.ScrollToCaret();

                                    } else {
                                        terminalLogRichTxtBx.AppendText(dataByte.ToString("X2") + Environment.NewLine);
                                        terminalLogRichTxtBx.ScrollToCaret();
                                    }

                                    first = true;

                                } else {
                                    if (first) {
                                        terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + dataByte.ToString("X2") + " ");

                                    } else {

                                        terminalLogRichTxtBx.AppendText(dataByte.ToString("X2") + " ");
                                    }

                                }
                            } else if (dataByte == LF_Byte) {
                                if (first) {
                                    terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + dataByte.ToString("X2") + Environment.NewLine);
                                    terminalLogRichTxtBx.ScrollToCaret();

                                } else {
                                    terminalLogRichTxtBx.AppendText(dataByte.ToString("X2") + Environment.NewLine);
                                    terminalLogRichTxtBx.ScrollToCaret();
                                }
                                first = true;
                            } else {
                                if (first) {
                                    terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + dataByte.ToString("X2") + " ");
                                    first = false;

                                } else {
                                    terminalLogRichTxtBx.AppendText(dataByte.ToString("X2") + " ");
                                }
                            }

                           /* switch (dataByte) {

                                //Эквивалентно /r
                                case 13: {

                                        //Если установлен флаг, что /r == /n
                                        if (clEqualsRf.Checked) {
                                            if (first) {
                                                terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + dataByte.ToString("X2") + Environment.NewLine);
                                                terminalLogRichTxtBx.ScrollToCaret();

                                            } else {
                                                terminalLogRichTxtBx.AppendText(dataByte.ToString("X2") + Environment.NewLine);
                                                terminalLogRichTxtBx.ScrollToCaret();
                                            }

                                            first = true;

                                        } else {
                                            if (first) {
                                                terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + dataByte.ToString("X2") + " ");

                                            } else {

                                                terminalLogRichTxtBx.AppendText(dataByte.ToString("X2") + " ");
                                            }

                                        }
                                    }
                                    break;

                                //Эквивалентно /n
                                case LF_Byte: {

                                        if (first) {
                                            terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + dataByte.ToString("X2") + Environment.NewLine);
                                            terminalLogRichTxtBx.ScrollToCaret();

                                        } else {
                                            terminalLogRichTxtBx.AppendText(dataByte.ToString("X2") + Environment.NewLine);
                                            terminalLogRichTxtBx.ScrollToCaret();
                                        }
                                        first = true;
                                    }
                                    break;

                                default: {
                                        if (first) {
                                            terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " >> " + dataByte.ToString("X2") + " ");
                                            first = false;

                                        } else {
                                            terminalLogRichTxtBx.AppendText(dataByte.ToString("X2") + " ");
                                        }
                                    }
                                    break;
                            }*/
                        }
                    });
                }

            } catch (InvalidOperationException) {
                //Пользователь нажал кнопку закрыть порт или отвалился COM
            }
        }

        string localParseLog;
        string prefix;
        /// <summary>
        /// Добавляет полученную информацию из COM порта в необходимый лог в зависимости от полученного сообщения
        /// </summary>
        private void appendTextToParseLog() {
            localParseLog = parseAnswer.ToString();

            try {
                prefix = localParseLog.Substring(16, 3);
            } catch (ArgumentOutOfRangeException) {
                //Пришла пустая строчка
                return;
            }

            if (prefix.Equals("Zc)") || prefix.Equals("Za)")) {
                MK_AnswerRhTxtBox.AppendText(localParseLog + Environment.NewLine);
                MK_AnswerRhTxtBox.ScrollToCaret();

            } else if (prefix.Equals("Qc)") || prefix.Equals("Qa)")) {
                quectelAnswerRhTxtBx.AppendText(localParseLog + Environment.NewLine);
                MK_AnswerRhTxtBox.ScrollToCaret();
            }

            parseAnswer.Clear();
        }

        private void editMacros_Click(object sender, EventArgs e) {

            //Если окно уже открыто
            if (macrosSettingsFlame != null) {
                macrosSettingsFlame.Activate();

                if (macrosSettingsFlame.WindowState == FormWindowState.Minimized) {
                    macrosSettingsFlame.WindowState = FormWindowState.Normal;
                }
                macrosSettingsFlame.BringToFront();

            } else {
                macrosSettingsFlame = new MacrosSettings(this);
                macrosSettingsFlame.Show();
            }
        }

        /// <summary>
        /// Устанавливает нулевую ссылку на окно редактирования макросов при его закрытии
        /// </summary>
        public void setNullMacrosForm() {
            macrosSettingsFlame = null;
        }

        /// <summary>
        /// Обновляет название кнопок в сответствии с именем макроса
        /// </summary>
        public void refreshMacrosBtns() {
            MacrosesGroupStorage macrosStorage = MacrosesGroupStorage.getMacrosesGroupStorageInstance();

            //Устанавливаю вкладкам имена групп
            for (int i = 0; i < macrosTabControl.Controls.Count; i++) {

                MacrosesGroup macrosesGroup = macrosStorage.getMacrosesGroupsList().ElementAt(i);

                TabPage tabPage = macrosTabControl.GetControl(i) as TabPage;

                //Установка названия группы
                tabPage.Text = macrosesGroup.Name;

                Button btn;
                int indexBtn;
                Macros macros;

                //Установка имени кнопки именем макроса
                foreach (Control btnControl in tabPage.Controls) {

                    btn = btnControl as Button;
                    indexBtn = Convert.ToInt32(btnControl.Name.Substring(5));

                    macros = macrosesGroup.getMacrosesDic()[indexBtn];

                    btn.Text = macros.MacrosName;
                }
            }

            refreshCycleThreadMacros();
        }

        private object locker = new object();
        private List<byte> sendByteList;
        private char[] charArr;
        private StringBuilder dataBytesBuilder = new StringBuilder();
        /// <summary>
        /// Отправляет данные в COM порт
        /// </summary>
        /// <param name="mess"></param>
        public void sendCommandInCOMPort(string mess) {
            if (serialPort.IsOpen) {

                if (String.IsNullOrEmpty(mess)) return;

                lock (locker) {

                    addMessInLocalBuff(mess);

                    if (modeTextRdBtn.Checked) {

                        sendByteList = new List<byte>();

                        charArr = mess.ToCharArray();

                        for (int i = 0; i < charArr.Length; i++) {
                            if (charArr[i] == '$') {
                                //Проверка, что после знака есть ещё символы
                                if (i < charArr.Length - 2) {
                                    try {
                                        sendByteList.Add(Convert.ToByte((charArr[i + 1].ToString() + charArr[i + 2].ToString()), 16));
                                        i += 2;
                                    } catch (FormatException) {
                                        Flasher.exceptionDialog("Не удалось преобразовать значение " + charArr[i + 1].ToString() + charArr[i + 2].ToString() + " в байты");
                                        return;
                                    }
                                    //Если нет
                                } else {
                                    Flasher.exceptionDialog("Не удалось преобразовать значение в байты после знака \'$\'");
                                    return;
                                }

                            } else {
                                sendByteList.Add(Encoding.ASCII.GetBytes(charArr[i].ToString())[0]);
                            }
                        }

                        //Если режим ввода HEX
                    } else {
                        try {

                            string newStrWithoutSpase = mess.Trim().Replace(" ", "");

                            sendByteList = new List<byte>(StringToByteArray(newStrWithoutSpase));
                            
                        } catch (ArgumentException) {
                            Flasher.exceptionDialog("Неверный формат введнных данных. Количество знаков должно быть чётным и иметь диапазон от 00 до FF");
                            return;
                        } catch (FormatException) {
                            Flasher.exceptionDialog("Неверный формат введнных данных. Количество знаков должно быть чётным и иметь диапазон от 00 до FF");
                            return;
                        }
                    }

                    //Если нужно добавить \r\n
                    if (addEndLine.Checked) {
                        sendByteList.Add(13);
                        sendByteList.Add(10);
                    }

                    //Если необходимо показывать отправленные данные
                    if (showSendDataInCOMChBx.Checked) {
                        if (modeTextRdBtn.Checked) {

                            if (clEqualsRf.Checked) {
                                addMessInComLog(mess);
                                addMessInComLog("");
                            } else {
                                addMessInComLog(mess);
                            }

                        } else {
                            dataBytesBuilder.Clear();

                            foreach (byte dataByte in sendByteList) {
                                dataBytesBuilder.Append(dataByte.ToString("X2") + " ");
                            }

                            if (clEqualsRf.Checked) {
                                addMessInComLog(dataBytesBuilder.ToString());
                                addMessInComLog("");
                            } else {
                                addMessInComLog(dataBytesBuilder.ToString());
                            }
                        }
                    }
                    
                    //Отправка данных в ком порт
                    serialPort.Write(sendByteList.ToArray(), 0, sendByteList.Count);
                }

            } else {
                Flasher.exceptionDialog("Откройте COM порт для отправки данных");
            }
        }

        /// <summary>
        /// Добавляет сообщение в буфер к 10 последним результатам
        /// </summary>
        /// <param name="mess"></param>
        private void addMessInLocalBuff(string mess) {

            if (lastCommandsList.Count > 10) {
                lastCommandsList.RemoveAt(10);
                lastCommandsList.Insert(1, mess);

            } else {
                lastCommandsList.Insert(1, mess);
            }

            lastCommandChoiseIndex = 0;
        }

        /// <summary>
        /// Конвертирует стринговые байты в числовые
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private void addMessInComLog(string mess) {
            terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " << " + mess + Environment.NewLine);
        }

        /// <summary>
        /// Выводит отправляемое в COM порт сообщение в лог
        /// </summary>
        /// <param name="messInLog"></param>
        private void outputMessegeToTerminalLog(string messInLog) {
            terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + " << " + messInLog + Environment.NewLine);
            terminalLogRichTxtBx.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// Выводит полученное из COM порта сообщение в лог
        /// </summary>
        /// <param name="messInLog"></param>
        private void inputMessegeToTerminalLog(string messInLog) {
            terminalLogRichTxtBx.AppendText(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + " >> " + messInLog + Environment.NewLine);
            terminalLogRichTxtBx.AppendText(Environment.NewLine);
        }

        private void messInCOMTxtBx_KeyDown(object sender, KeyEventArgs e) {
            
            if (e.KeyCode == Keys.Enter) {
                if (messInCOMTxtBx.Focused) {
                    sendBtn.PerformClick();

                    e.SuppressKeyPress = true;
                }

                //Достаёт прошлые отправленные сообщения в COM порт
            } else if (e.KeyCode == Keys.Down) {

                //В бeфере не больше 11
                if (lastCommandChoiseIndex < 10) {

                    //Если значение в буфере пустое, то больше значений не было
                    if (String.IsNullOrEmpty(lastCommandsList.ElementAt(lastCommandChoiseIndex + 1))) return;

                    lastCommandChoiseIndex++;
                }

                messInCOMTxtBx.Text = lastCommandsList.ElementAt(lastCommandChoiseIndex);
                messInCOMTxtBx.SelectionStart = messInCOMTxtBx.Text.Length;

                e.SuppressKeyPress = true;

            } else if (e.KeyCode == Keys.Up) {

                if (lastCommandChoiseIndex > 0) lastCommandChoiseIndex--;

                messInCOMTxtBx.Text = lastCommandsList.ElementAt(lastCommandChoiseIndex);
                messInCOMTxtBx.SelectionStart = messInCOMTxtBx.Text.Length;
                e.SuppressKeyPress = true;
            }
        }

        private void sendBtn_Click(object sender, EventArgs e) {
            sendCommandInCOMPort(messInCOMTxtBx.Text);
            //messInCOMTxtBx.Text = ""; Нужно ли удалять данные после отправки
        }

        /// <summary>
        /// Возвращает индекс выбранной страницы TabControl с макросами
        /// </summary>
        /// <returns></returns>
        public int getMacrosPanelIndex() {
            return macrosTabControl.SelectedIndex;
        }

        private void Terminal_SizeChanged(object sender, EventArgs e) {
            if (groupBox2.Location.Y > 5) {
                splitContainer1.SplitterDistance = 144 * 2 + 1;
            } else {
                try {
                    splitContainer1.SplitterDistance = 144;
                } catch (InvalidOperationException) { }
            }
        }

        private void macrosBtns_Click(object sender, EventArgs e) {
            MacrosesGroupStorage macrosStorage = MacrosesGroupStorage.getMacrosesGroupStorageInstance();

            MacrosesGroup macrosGroup = macrosStorage.getMacrosesGroupsList().ElementAt(macrosTabControl.SelectedIndex);

            Macros macros = macrosGroup.getMacrosesDic()[Convert.ToInt32((sender as Button).Name.Substring(5))];

            //Если опция переодической отправки сообщений включена
            if (macros.MacrosInCycle) {

                if (!serialPort.IsOpen) {
                    Flasher.exceptionDialog("Откройте COM порт для отправки данных");
                    return;
                }

                //Если поток циклической отправки был когда-либо создан
                if (macros.getCycleThreadSendData() != null) {

                    //Если поток циклической отправки ещё работает
                    if (macros.cycleThreadIsLeave()) {
                        macros.getCycleThreadSendData().Abort();

                        threadMacroses.Remove((sender as Button).Name);

                        //Меняю цвет кнопки на стандартный
                        (sender as Button).BackColor = defaultColor;

                        //Если поток был убит
                    } else {

                        if (threadMacroses.Count >= 10) {
                            Flasher.exceptionDialog("Нельзя использовать больше 10 циклических макросов");
                            return;
                        }

                        //Запускаю новый
                        macros.startCycleSendDataInCOM(this);

                        threadMacroses.Add((sender as Button).Name);

                        (sender as Button).BackColor = Color.PaleGreen;
                    }

                    //Если не было
                } else {

                    if (threadMacroses.Count >= 10) {
                        Flasher.exceptionDialog("Нельзя использовать больше 10 циклических макросов");
                        return;
                    }

                    //Запускаю новый
                    macros.startCycleSendDataInCOM(this);
                    threadMacroses.Add((sender as Button).Name);
                    (sender as Button).BackColor = Color.PaleGreen;
                }

                //Если выключена, то просто единоразово отправляю данные в COM порт
            } else {
                sendCommandInCOMPort(macros.MacrosValue);
            }
        }

        private void Terminal_KeyDown(object sender, KeyEventArgs e) {

            if (e.KeyCode == Keys.Escape) {
                clearLog.PerformClick();
                e.SuppressKeyPress = true;

            } else if (e.KeyCode == Keys.M && e.Modifiers == Keys.Control) {
                editMacros.PerformClick();
                e.SuppressKeyPress = true;

            } else if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control) {
                saveLog.PerformClick();
                e.SuppressKeyPress = true;

            } else if (e.KeyCode == Keys.B && e.Modifiers == Keys.Control) {
                new CR_LF(this).ShowDialog();
                e.SuppressKeyPress = true;
            }

            if (!messInCOMTxtBx.Focused &&
                !customBandRateTxtBx.Focused &&
                !cpNumbStandartPortTxtBx.Focused &&
                !cpNumbEnhancedPortTxtBx.Focused &&
                !comPortsListCmbBox.Focused) {

                if (e.KeyCode == Keys.S) {
                    showOrHideControlPanel.PerformClick();
                    e.SuppressKeyPress = true;
                    return;
                }

                if ((e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1)
                    && e.Modifiers == Keys.Control) {
                    selectHotCaseMacrosBtn(11);
                    e.SuppressKeyPress = true;

                } else if ((e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2)
                    && e.Modifiers == Keys.Control) {
                    selectHotCaseMacrosBtn(12);
                    e.SuppressKeyPress = true;

                } else if ((e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3)
                     && e.Modifiers == Keys.Control) {
                    selectHotCaseMacrosBtn(13);
                    e.SuppressKeyPress = true;

                } else if ((e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.D4)
                     && e.Modifiers == Keys.Control) {
                    selectHotCaseMacrosBtn(14);
                    e.SuppressKeyPress = true;

                } else if ((e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.D5)
                     && e.Modifiers == Keys.Control) {
                    selectHotCaseMacrosBtn(15);
                    e.SuppressKeyPress = true;

                } else if ((e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.D6)
                     && e.Modifiers == Keys.Control) {
                    selectHotCaseMacrosBtn(16);
                    e.SuppressKeyPress = true;

                } else if ((e.KeyCode == Keys.NumPad7 || e.KeyCode == Keys.D7)
                     && e.Modifiers == Keys.Control) {
                    selectHotCaseMacrosBtn(17);
                    e.SuppressKeyPress = true;

                } else if ((e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.D8)
                     && e.Modifiers == Keys.Control) {
                    selectHotCaseMacrosBtn(18);
                    e.SuppressKeyPress = true;

                } else if ((e.KeyCode == Keys.NumPad9 || e.KeyCode == Keys.D9)
                     && e.Modifiers == Keys.Control) {
                    selectHotCaseMacrosBtn(19);
                    e.SuppressKeyPress = true;

                } else if ((e.KeyCode == Keys.NumPad0 || e.KeyCode == Keys.D0)
                     && e.Modifiers == Keys.Control) {
                    selectHotCaseMacrosBtn(20);
                    e.SuppressKeyPress = true;

                    //Отправка макросов начиная с 1
                } else if (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1) {
                    selectHotCaseMacrosBtn(1);
                    e.SuppressKeyPress = true;

                } else if (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2) {
                    selectHotCaseMacrosBtn(2);
                    e.SuppressKeyPress = true;

                } else if (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3) {
                    selectHotCaseMacrosBtn(3);
                    e.SuppressKeyPress = true;

                } else if (e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.D4) {
                    selectHotCaseMacrosBtn(4);
                    e.SuppressKeyPress = true;

                } else if (e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.D5) {
                    selectHotCaseMacrosBtn(5);
                    e.SuppressKeyPress = true;

                } else if (e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.D6) {
                    selectHotCaseMacrosBtn(6);
                    e.SuppressKeyPress = true;

                } else if (e.KeyCode == Keys.NumPad7 || e.KeyCode == Keys.D7) {
                    selectHotCaseMacrosBtn(7);
                    e.SuppressKeyPress = true;

                } else if (e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.D8) {
                    selectHotCaseMacrosBtn(8);
                    e.SuppressKeyPress = true;

                } else if (e.KeyCode == Keys.NumPad9 || e.KeyCode == Keys.D9) {
                    selectHotCaseMacrosBtn(9);
                    e.SuppressKeyPress = true;

                } else if (e.KeyCode == Keys.NumPad0 || e.KeyCode == Keys.D0) {
                    selectHotCaseMacrosBtn(10);
                    e.SuppressKeyPress = true;
                }
            }
        }

        /// <summary>
        /// Нажимает на кнопку по порядуому номеру макроса
        /// </summary>
        /// <param name="i"></param>
        private void selectHotCaseMacrosBtn(int i) {
            foreach (Control btn in macrosTabControl.SelectedTab.Controls) {
                if (Convert.ToInt32(btn.Name.Substring(5)) == i) {
                    (btn as Button).PerformClick();
                    break;
                }
            }
        }

        /// <summary>
        /// Обновляет содержание подсказок для макросов
        /// </summary>
        public void refreshToolTipsForMacros() {

            List<MacrosesGroup> macrosesGroupList = MacrosesGroupStorage.getMacrosesGroupStorageInstance().getMacrosesGroupsList();

            toolTipForMacros = new ToolTip();
            toolTipForMacros.InitialDelay = 250;
            toolTipForMacros.ReshowDelay = 150;

            MacrosesGroup macrosesGroup;
            Macros macros;
            string toolTipMess;
            for (int i = 0; i < macrosTabControl.Controls.Count; i++) {
                macrosesGroup = macrosesGroupList.ElementAt(i);

                for (int j = 1; j <= macrosTabControl.GetControl(i).Controls.Count; j++) {
                    macros = macrosesGroup.getMacrosesDic()[j];

                    foreach (Control btn in macrosTabControl.GetControl(i).Controls) {

                        if (Convert.ToInt32(btn.Name.Substring(5)) == j) {

                            toolTipMess = "Отправляемые данные: " + macros.MacrosValue;

                            if (macros.MacrosInCycle) {
                                toolTipMess += "\nПериод отправки: " + macros.TimeCycle + " мс";
                            }

                            if (j <= 10) {

                                if (j != 10) {
                                    toolTipMess += "\nНажнимите цифру " + j + " для отправки данных в порт";
                                } else {
                                    toolTipMess += "\nНажнимите цифру 0 для отправки данных в порт";
                                }
                            } else {

                                if (j != 20) {
                                    toolTipMess += "\nЗажмите Ctrl и нажнимите цифру " + (j - 10) + " для отправки данных в порт";
                                } else {
                                    toolTipMess += "\nЗажмите Ctrl и нажмите цифру 0 для отправки данных в порт";
                                }
                            }

                            toolTipForMacros.SetToolTip(btn, toolTipMess);
                            break;
                        }
                    }
                }
            }
        }

        private void clearLog_Click(object sender, EventArgs e) {
            bool answer = Flasher.YesOrNoDialog("Вы уверены, что хотите очистить лог?", "Очистка лога");

            if (answer) {
                terminalLogRichTxtBx.Clear();
                MK_AnswerRhTxtBox.Clear();
                quectelAnswerRhTxtBx.Clear();
            }
        }

        private void saveLog_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            string logText = terminalLogRichTxtBx.Text;

            saveFileDialog.InitialDirectory = Properties.Settings.Default.terminal_SaveLogDir;

            saveFileDialog.FileName = "taipitTerminalLog_" + DateTime.Now.ToString("yyyy_MM_dd_HH-mm-ss") + ".log";
            saveFileDialog.Filter = "All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;


            if (saveFileDialog.ShowDialog() == DialogResult.OK) {

                Properties.Settings.Default.terminal_SaveLogDir = Path.GetDirectoryName(saveFileDialog.FileName);
                Properties.Settings.Default.Save();

                // сохраняем текст в файл
                File.WriteAllText(saveFileDialog.FileName, logText);

                Flasher.successfullyDialog("Лог успешно сохранен", "Сохранение лога");
            }
        }

        /// <summary>
        /// Выставляет необходимые подсказки кнопкам и группам
        /// </summary>
        private void setToolTipsTerminalFrame() {
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 250;
            toolTip.ReshowDelay = 100;

            toolTip.SetToolTip(groupBox3, "Выбор отображения информации в логе");
            toolTip.SetToolTip(inputModeTextRdBtn, "Нажмите чтобы информация в логе отображалась в текстовом формате");
            toolTip.SetToolTip(inputModeHexRdBtn, "Нажмите чтобы информация в логе отображалась в шестнадцатеричном формате");

            toolTip.SetToolTip(modeGroup, "Выбор формата ввода информации");
            toolTip.SetToolTip(modeTextRdBtn, "Нажмите, если хотите вводить информацию в текстовом виде");
            toolTip.SetToolTip(modeHexRdBtn, "Нажмите, если хотите вводить информацию в шестнадцатеричном формате");

            toolTip.SetToolTip(clearLog, "Очистка лога сообщений\nИспользуйте: Esc");
            toolTip.SetToolTip(saveLog, "Сохранение лога\nИспользуйте: Ctrl+S");
            toolTip.SetToolTip(connectToModuleBtn, "Автоматически выставляет параметры, необходимые для общения с модулем Qectel");
            toolTip.SetToolTip(connectToMKBtn, "Автоматически выставляет параметры, необходимые для общения с микрокотроллером");

            toolTip.SetToolTip(groupBox6, "Управление состоянием ног CP2105");
            toolTip.SetToolTip(searchCP2105Ports, "Находит порты модема и считывает состояния ног CP2105");

            toolTip.SetToolTip(showOrHideControlPanel, "Нажмите чтобы скрыть/раскрыть панель с настройками\nИспользуйте: S");

            toolTip.SetToolTip(editMacros, "Нажмите чтобы перейти в настройки макросов\nИспользуйте: Ctrl+M");

            toolTip.SetToolTip(clEqualsRf, "Использовать байт CR как новую строку");

            toolTip.SetToolTip(changeAlignment, "Поменять размещение рабочих экранов окна отсортированных сообщений");
        }

        private void showOrHideControlPanel_Click(object sender, EventArgs e) {
            int position = groupBox2.Size.Height / 2;

            if (splitContainer1.SplitterDistance > position) {
                splitContainer1.SplitterDistance = 0;
            } else {
                splitContainer1.SplitterDistance = groupBox2.Location.Y + groupBox2.Size.Height;
            }
        }

        private void addEndLine_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.terminal_plus_CL_RF = addEndLine.Checked;
            Properties.Settings.Default.Save();
        }

        private void clEqualsRf_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.terminal_CLequalsRF = clEqualsRf.Checked;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Останавливает все циклические макросы
        /// </summary>
        public void abortAllCycleMacros() {

            MacrosesGroupStorage macrosStorage = MacrosesGroupStorage.getMacrosesGroupStorageInstance();

            //Останавливаю потоки
            foreach (MacrosesGroup macrosesGroup in macrosStorage.getMacrosesGroupsList()) {

                foreach (Macros macros in macrosesGroup.getMacrosesDic().Values) {
                    if (macros.getCycleThreadSendData() != null) {
                        if (macros.getCycleThreadSendData().IsAlive) {
                            macros.getCycleThreadSendData().Abort();
                        }
                    }
                }
            }

            //Выставляю цвет макроса
            foreach (Control tab in macrosTabControl.Controls) {
                foreach (Control btn in tab.Controls) {

                    (btn as Button).BackColor = defaultColor;
                }
            }
        }

        /// <summary>
        /// Запускает ранее запущенные макросы после изменении в окне редактирования макросов
        /// </summary>
        private void refreshCycleThreadMacros() {

            //Прохожусь по списку ранее запущенных макросов
            for (int i = 0; i < threadMacroses.Count; i++) {

                Macros macros = returmMacrosOnBtn(threadMacroses.ElementAt(i));

                if (macros.MacrosInCycle) {
                    int group = Convert.ToInt32(threadMacroses.ElementAt(i).Substring(2, 1)) - 1;

                    foreach (Control macrosBtn in macrosTabControl.GetControl(group).Controls) {
                        if (threadMacroses.ElementAt(i).Equals((macrosBtn as Button).Name)) {
                            (macrosBtn as Button).BackColor = Color.PaleGreen;
                            macros.startCycleSendDataInCOM(this);
                            break;
                        }
                    }

                } else {
                    threadMacroses.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Возвращает макрос принадлежащий к ID кнопки
        /// </summary>
        /// <param name="btnText"></param>
        /// <returns></returns>
        private Macros returmMacrosOnBtn(string btnText) {
            int group = Convert.ToInt32(btnText.Substring(2, 1)) - 1;
            int macrosId = Convert.ToInt32(btnText.Substring(5));

            return MacrosesGroupStorage.getMacrosesGroupStorageInstance().getMacrosesGroupsList().ElementAt(group).getMacrosesDic()[macrosId];
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            if (tabControl1.SelectedIndex == 0) {
                changeAlignment.Enabled = false;

            } else {
                changeAlignment.Enabled = true;
            }
        }

        private void changeAlignment_Click(object sender, EventArgs e) {
            if (splitContainer2.Orientation == Orientation.Horizontal) {
                splitContainer2.Orientation = Orientation.Vertical;

                splitContainer2.SplitterDistance = splitContainer2.Size.Width / 2;

            } else {
                splitContainer2.Orientation = Orientation.Horizontal;

                splitContainer2.SplitterDistance = splitContainer2.Size.Height / 2;
            }            
        }

        private void showSendDataInCOMChBx_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.terminal_showSendDataInCOMChBx = showSendDataInCOMChBx.Checked;
            Properties.Settings.Default.Save();
        }
    }
}