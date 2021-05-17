using GSM_NBIoT_Module.classes;
using GSM_NBIoT_Module.classes.applicationHelper;
using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using GSM_NBIoT_Module.classes.modemConfig;
using GSM_NBIoT_Module.classes.modemConfig.ZPORT_protochol;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace GSM_NBIoT_Module.view {
    public partial class ModemConfig : Form {
        public ModemConfig(Form form) {
            InitializeComponent();

            flasMainForm = form as Flasher;
        }

        private Flasher flasMainForm;

        private SerialPort serialPort = new SerialPort();
        private int timeOut = 1000;

        //Если ли подключение к COM порту в окне терминала
        bool connectOnCOMterminal = false;

        //Подсказка для вкладки пользовательских настроек подключения у серверам
        private ToolTip toolTipForUserHostParam = new ToolTip();

        //Текст подсказок для полей записи адреса
        private const string domenNametoolTipMess = "Значение доменного имени должно именть знак \" в начале и в конце (помещено в кавычки)." +
                    "\nЗначение доменного имени не должно быть больше 28 символов" +
                    "\nПример записи: \"devices.226.taipit.ru\"" +
                    "\nПри записи полностью пустого значения (без знаков \") или пустого значения между знаками \" параметры сервера полностью удаляются";

        private const string ipV4toolTipMess = "Формат записи IPv4 XXX.XXX.XXX.XXX, где XXX должно быть десятичным числом в диапазоне 0..255" +
                    "\nПример записи: 66.254.114.41" +
                    "\nПри записи полностью пустого значения параметры сервера полностью удаляются";

        private const string ipV6toolTipMess = "Формат записи IPv6 XXXX:XXXX:XXXX:XXXX:XXXX:XXXX:XXXX:XXXX, диапазон каждого значения X в одном поле (XXXX)\nдолжен быть в диапазоне от 0..F (HEX) " +
                    "\nПримеры записи:" +
                    "\n2001:0DB0:0000:123A:0000:0000:0000:0030" +
                    "\n2001:DB0:0:123A:0:0:0:30" +
                    "\n2001:DB0:0:123A::30" +
                    "\nПри записи полностью пустого значения параметры сервера полностью удаляются";

        private void ModemConfig_Load(object sender, EventArgs e) {

            //Добавление настраиваемых параметров
            DataGridViewRow row = new DataGridViewRow();
            ZPORTcommandsDataGridView.Rows.Add(row);
            row.Cells[0].Value = "1. Настройка пользовательских серверов";

            row = new DataGridViewRow();
            ZPORTcommandsDataGridView.Rows.Add(row);
            ZPORTcommandsDataGridView.Rows[1].Cells[0].Value = "2. Параметры инициализации связи";

            row = new DataGridViewRow();
            ZPORTcommandsDataGridView.Rows.Add(row);
            ZPORTcommandsDataGridView.Rows[2].Cells[0].Value = "3. Настройки входящего соединения";

            ZPORTcommandsDataGridView.Columns[0].Width = ZPORTcommandsDataGridView.Size.Width;

            //Параметры COM порта
            serialPort.BaudRate = 125000;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.DataBits = 8;
            serialPort.NewLine = "\r\n";

            //Добавляю слушателей
            domenNameRdBtn_1.CheckedChanged += domenNameRdBtnGroup_1_CheckedChanged;
            IPv4RdBtn_1.CheckedChanged += domenNameRdBtnGroup_1_CheckedChanged;
            IPv6RdBtn_1.CheckedChanged += domenNameRdBtnGroup_1_CheckedChanged;

            domenNameRdBtn_2.CheckedChanged += domenNameRdBtnGroup_2_CheckedChanged;
            IPv4RdBtn_2.CheckedChanged += domenNameRdBtnGroup_2_CheckedChanged;
            IPv6RdBtn_2.CheckedChanged += domenNameRdBtnGroup_2_CheckedChanged;

            domenNameRdBtn_3.CheckedChanged += domenNameRdBtnGroup_3_CheckedChanged;
            IPv4RdBtn_3.CheckedChanged += domenNameRdBtnGroup_3_CheckedChanged;
            IPv6RdBtn_3.CheckedChanged += domenNameRdBtnGroup_3_CheckedChanged;

            domenNameRdBtn_1.PerformClick();
            domenNameRdBtn_2.PerformClick();
            domenNameRdBtn_3.PerformClick();

            cmdKeyHexRdBt.CheckedChanged += cmdKeyRdBt_CheckedChanged;

            //Установка подсказок полям
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 15000;
            toolTip.InitialDelay = 250;
            toolTip.ReshowDelay = 0;
            toolTip.ShowAlways = true;

            toolTip.SetToolTip(periodMsdTxtBx, "Формат hh:mm:ss");
            toolTip.SetToolTip(serviceMsdTxtBx, "Формат hh:mm:ss");
            toolTip.SetToolTip(letwaitMsdTxtBx, "Формат hh:mm:ss");
            toolTip.SetToolTip(trylimitMsdTxtBx, "Формат (количество)");
            toolTip.SetToolTip(sesslimitMsdTxtBx, "Формат hh:mm:ss");
            toolTip.SetToolTip(holdTimeMsdTxtBx, "Формат hh:mm:ss");
            toolTip.SetToolTip(incomholdtimeMskTxtBx, "Формат hh:mm:ss");

            toolTip.SetToolTip(portTxtBx_1, "Значение порта должно быть в диапазоне 0..65535");
            toolTip.SetToolTip(portTxtBx_2, "Значение порта должно быть в диапазоне 0..65535");
            toolTip.SetToolTip(portTxtBx_3, "Значение порта должно быть в диапазоне 0..65535");

            toolTip.SetToolTip(domenNameRdBtn_1, domenNametoolTipMess);
            toolTip.SetToolTip(domenNameRdBtn_2, domenNametoolTipMess);
            toolTip.SetToolTip(domenNameRdBtn_3, domenNametoolTipMess);

            toolTip.SetToolTip(IPv4RdBtn_1, ipV4toolTipMess);
            toolTip.SetToolTip(IPv4RdBtn_2, ipV4toolTipMess);
            toolTip.SetToolTip(IPv4RdBtn_3, ipV4toolTipMess);

            toolTip.SetToolTip(IPv6RdBtn_1, ipV6toolTipMess);
            toolTip.SetToolTip(IPv6RdBtn_2, ipV6toolTipMess);
            toolTip.SetToolTip(IPv6RdBtn_3, ipV6toolTipMess);

            //Подсказки для кнопок создания и записи скриптов
            toolTip.SetToolTip(createScript, "Создать скрипт для быстрой записи параметров в модем.\nДанный скрипт создаётся на основе значений указанных\nна данный момент в полях конфигурации модема");
            toolTip.SetToolTip(loadScript, "Записать конфигурационные параметры в модем из файла");

            //Подсказка для полей с адресом дополнительных серверов
            toolTipForUserHostParam.AutoPopDelay = 10000;
            toolTipForUserHostParam.InitialDelay = 0;
            toolTipForUserHostParam.ReshowDelay = 0;
            toolTipForUserHostParam.ShowAlways = true;

            refreshInfoBtn.PerformClick();
        }

        /// <summary>
        /// Подгоняет размер столба под таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZPORTcommandsDataGridView_SizeChanged(object sender, EventArgs e) {
            ZPORTcommandsDataGridView.Columns[0].Width = ZPORTcommandsDataGridView.Size.Width;
        }

        private void ZPORTcommandsDataGridView_SelectionChanged(object sender, EventArgs e) {
            int i = ZPORTcommandsDataGridView.SelectedCells[0].RowIndex;

            switch (i) {
                case 0: {
                        userHostTableLayoutPanel.BringToFront();
                    }
                    break;

                case 1: {
                        connectingTableLayoutPanel.BringToFront();
                    }
                    break;

                case 2: {
                        serverTabLytPnl.BringToFront();
                    }
                    break;
            }
        }

        /// <summary>
        /// Обновляет и считывает информацию из модема
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshInfo_Click(object sender, EventArgs e) {
            try {

                //Проверяю используется ли COM порт в окне терминала
                connectOnCOMterminal = flasMainForm.getStateConnectionOnCOMofTerminalForm();
                if (connectOnCOMterminal) {
                    flasMainForm.performClickConnOfTerminalForm();
                }

                Cursor = Cursors.WaitCursor;

                enhancedCOMtxtBx.Text = getNbModemPort().ToString();
                checkGPIO_CP2105();

                if (!serialPort.IsOpen) serialPort.Open();

                setAValidRangeForTheLabelInfoForConnectingParam();

                Dictionary<string, string> deviceParams = getParamsDeviceCommand();

                verFWTxtBx.Text = deviceParams["FW"];
                verProtTxtBx.Text = deviceParams["ZPORT"];
                tergetIdTxtBx.Text = deviceParams["TARGET"];
                protocolIdTxtBx.Text = deviceParams["PROT_ID"];
                IndexTxtBx.Text = deviceParams["IDX"];
                copyIDTxtBx.Text = deviceParams["COPY_ID"];

                Dictionary<string, string> MDMIMIDParams = getMDMIMIDCommandParams();
                modemImeiTxtBx.Text = MDMIMIDParams["IMEI"];
                IMSItxtBx.Text = MDMIMIDParams["IMSI"];
                ICCIDtxtBx.Text = MDMIMIDParams["ICCID"];

                Dictionary<string, string> baseHost = getBaseHostParams();
                hostIPTxtBx.Text = baseHost["IP"];
                hostPortTxtBx.Text = baseHost["PORT"];

                APNtxtBx.Text = getAPNParams()["BASE"];

                getCustomSettingsServers();

                getCONNECTINGparametersAndSetInConnectingPanel();

                //Test
                /*ServerCommand.writeServerParams(
                    ServerCommand.checkValidPortParameter("15"),
                    ServerCommand.CMDKEYParamByte(StringToByteArray("61626364")),
                    "IPV6",
                    serialPort);*/

                Dictionary<string, string> dic = ServerCommand.readServerCommnadParams(serialPort);

                if (serialPort.IsOpen) serialPort.Close();

                if (connectOnCOMterminal) flasMainForm.performClickConnOfTerminalForm();

                Cursor = Cursors.Default;

            } catch (Exception ex) {

                Cursor = Cursors.Default;
                Flasher.exceptionDialog(ex.Message);

                if (serialPort.IsOpen) serialPort.Close();
                if (connectOnCOMterminal) flasMainForm.performClickConnOfTerminalForm();
            }
        }

        /// <summary>
        /// Получает инофрмацию о пользовательских настройках добавочных серверов (вкладка: настройка пользовательских серверов)
        /// </summary>
        private void getCustomSettingsServers() {
            string iPorDomen = "";
            string port = "";

            //Заполняю информацией поля
            //Доменное имя и порт сервера 0
            getIPv4AndPortUserHost(0, ref iPorDomen, ref port);
            portTxtBx_1.Text = port;

            if (!String.IsNullOrEmpty(iPorDomen)) {
                if (iPorDomen.StartsWith("\"") && iPorDomen.EndsWith("\"")) {
                    domenNameRdBtn_1.Checked = true;

                } else if (iPorDomen.Contains('.')) {
                    IPv4RdBtn_1.Checked = true;

                } else {
                    IPv6RdBtn_1.Checked = true;
                }
            }

            ipDomenNameTxtBx_1.Text = iPorDomen;

            //Доменное имя и порт сервера 1
            getIPv4AndPortUserHost(1, ref iPorDomen, ref port);
            portTxtBx_2.Text = port;

            if (!String.IsNullOrEmpty(iPorDomen)) {
                if (iPorDomen.StartsWith("\"") && iPorDomen.EndsWith("\"")) {
                    domenNameRdBtn_2.Checked = true;

                } else if (iPorDomen.Contains('.')) {
                    IPv4RdBtn_2.Checked = true;

                } else {
                    IPv6RdBtn_2.Checked = true;
                }
            }

            ipDomenNameTxtBx_2.Text = iPorDomen;

            //Доменное имя и порт сервера 2
            getIPv4AndPortUserHost(2, ref iPorDomen, ref port);
            portTxtBx_3.Text = port;

            if (!String.IsNullOrEmpty(iPorDomen)) {
                if (iPorDomen.StartsWith("\"") && iPorDomen.EndsWith("\"")) {
                    domenNameRdBtn_3.Checked = true;

                } else if (iPorDomen.Contains('.')) {
                    IPv4RdBtn_3.Checked = true;

                } else {
                    IPv6RdBtn_3.Checked = true;
                }
            }

            ipDomenNameTxtBx_3.Text = iPorDomen;
        }

        /// <summary>
        /// Устанавливает пользовательские параметры дополнительных серверов (Вкладка: настройка пользовательских серверов)
        /// </summary>
        private void writeCustomServersProperties() {
            checkCustomServerProperties(0, formatAddresRecord(1), ipDomenNameTxtBx_1, portTxtBx_1);
            checkCustomServerProperties(1, formatAddresRecord(2), ipDomenNameTxtBx_2, portTxtBx_2);
            checkCustomServerProperties(2, formatAddresRecord(3), ipDomenNameTxtBx_3, portTxtBx_3);
        }

        /// <summary>
        /// Возвращает какой радиобтн сейчас зажат в выборе формата записи адреса
        /// </summary>
        /// <param name="i">Для какой группы радиобаттонов</param>
        /// <returns></returns>
        private int formatAddresRecord(int i) {
            switch (i) {
                case 1: {
                        if (domenNameRdBtn_1.Checked) return 0;
                        else if (IPv4RdBtn_1.Checked) return 1;
                        else return 2;
                    }

                case 2: {
                        if (domenNameRdBtn_2.Checked) return 0;
                        else if (IPv4RdBtn_2.Checked) return 1;
                        else return 2;
                    }

                case 3: {
                        if (domenNameRdBtn_3.Checked) return 0;
                        else if (IPv4RdBtn_3.Checked) return 1;
                        else return 2;
                    }
            }

            throw new ArgumentException("Сценария с аргументом " + i + " не предусмотрено");
        }

        /// <summary>
        /// Проверяет параметры доменного имени или IP адреса введённые пользователем
        /// </summary>
        /// <param name="numbServerProperties"></param>
        /// <param name="domenOrIPv4Check">
        /// 0 - формат в значении доменного имени
        /// 1 - в формате IPv4
        /// 2 - в формате IPv6
        /// </param>
        /// <param name="domenOrIPData"></param>
        /// <param name="portData"></param>
        private void checkCustomServerProperties(int numbServerProperties, int domenOrIPv4Check, TextBox domenOrIPData, TextBox portData) {

            domenOrIPData.Text = domenOrIPData.Text.Trim();

            //Отправка данных в порт
            string dataToPort;

            //Если пустое значение поля, то удаляю параметр
            if (String.IsNullOrEmpty(domenOrIPData.Text) ||
                //Или пустое значение между "  " при записи доменного имени
                (domenOrIPv4Check == 0 && domenOrIPData.Text.Length > 1 && domenOrIPData.Text.StartsWith("\"") && domenOrIPData.Text.EndsWith("\"")
                && String.IsNullOrEmpty(domenOrIPData.Text.Substring(1, domenOrIPData.Text.Length - 2).Trim()))) {

                dataToPort = "USERHOST N=" + numbServerProperties + " ALL=X";

                //Иначе записываю все данные
            } else {

                //Если должно поступить на вход доменное имя
                switch (domenOrIPv4Check) {


                    case 0: {
                            char[] domenNameArr = domenOrIPData.Text.ToCharArray();

                            if (domenNameArr.Length < 2 || (domenNameArr[0] != '\"' || domenNameArr[domenNameArr.Length - 1] != '\"')) {
                                domenOrIPData.Focus();
                                domenOrIPData.SelectAll();
                                throw new FormatException("Доменное имя должно содержать знак \" в начале и конце");
                            }

                            //Если между знаками "" пусто, то записываю команду на отчистку
                            if (String.IsNullOrEmpty(domenOrIPData.Text.Trim().Substring(1, domenOrIPData.Text.Length - 2).Trim())) {
                                dataToPort = "USERHOST N=" + numbServerProperties + " ALL=X";
                                break;
                            }

                            //C учётом кавычек
                            if (domenNameArr.Length - 2 > 28) {
                                domenOrIPData.Focus();
                                domenOrIPData.SelectAll();
                                throw new FormatException("Доменное имя не должно быть больше 28 символов");
                            }

                            for (int i = 1; i < domenNameArr.Length - 1; i++) {
                                if (domenNameArr[i] < 0x20 || domenNameArr[i] > 0x7F) {
                                    domenOrIPData.Focus();
                                    domenOrIPData.SelectAll();
                                    throw new FormatException("Доменное имя должно содержать только ASCII символы");
                                }
                            }
                        }
                        break;

                    //Если должен поступить на вход IPv4 адрес
                    case 1: {
                            //Проверка значений
                            try {

                                string[] ipv4Arr = domenOrIPData.Text.Split('.');

                                List<byte> byteList = new List<byte>();

                                if (ipv4Arr.Length != 4) throw new FormatException();

                                foreach (string ipNode in ipv4Arr) {
                                    if (String.IsNullOrEmpty(ipNode.Trim())) {
                                        byteList.Add(0);
                                        continue;
                                    }

                                    int localByte = Convert.ToInt32(ipNode.Trim());

                                    if (localByte < 0 || localByte > 255) throw new FormatException();

                                    byteList.Add((byte)localByte);
                                }

                                domenOrIPData.Text = byteList[0] + "." +
                                    byteList[1] + "." +
                                    byteList[2] + "." +
                                    byteList[3];

                            } catch (Exception) {
                                domenOrIPData.Focus();
                                domenOrIPData.SelectAll();
                                throw new FormatException("Неверный формат записи IPv4, формат должен иметь вид XXX.XXX.XXX.XXX, где XXX может иметь значение от 0..255");
                            }

                        }
                        break;

                    //Если должен поступить на вход IPv6 адрес
                    case 2: {

                            //Эта функция появилась только с версии протокола ZP009
                            //Проверяю, что у модема версия ZP009 и выше
                            if (Convert.ToInt32(verProtTxtBx.Text.Substring(2, 3)) < 9) {
                                domenOrIPData.Focus();
                                domenOrIPData.SelectAll();
                                throw new FormatException("Запись адреса в формате IPv6 возможно только с версии протокола ZP009 и выше, " +
                                    "используйте запись в формате доменного имени или IPv4");
                            }

                            try {

                                //Потому что Сергей просит добавлять 0 если сокращённая запись находится в начеле или конце (вместо ::FF писать 0::FF). Это вызывает проблемы если 6 хекстетов уже есть
                                //Но впереди обязательно нужен ноль, тогда Сергей считает, что уже больше хестетов (пример проблемы ::1:2:3:4:5:6 Сергей считает, что это неверный формат)
                                if ((domenOrIPData.Text.IndexOf("::") == 0) &&
                                    domenOrIPData.Text.Split(':').Length == 8) {

                                    domenOrIPData.Text = domenOrIPData.Text.Replace("::", "0:0:");

                                } else if (domenOrIPData.Text.LastIndexOf("::") == domenOrIPData.Text.Length - 2 &&
                                    domenOrIPData.Text.Split(':').Length == 8) {

                                    domenOrIPData.Text = domenOrIPData.Text.Replace("::", ":0:0");
                                }

                                IPv6Parser.checValidValue(domenOrIPData.Text);

                            } catch (Exception) {
                                domenOrIPData.Focus();
                                domenOrIPData.SelectAll();
                                throw;
                            }
                        }
                        break;
                }

                //Проверка коректности ввода порта
                int portValue;

                if (!int.TryParse(portData.Text, out portValue)) {
                    portData.Focus();
                    portData.SelectAll();
                    throw new ArgumentException("Значение порта дожно быть числовым и находиться в диапазоне 0..65535");
                }

                if (portValue > 65535 || portValue < 0) {
                    portData.Focus();
                    portData.SelectAll();
                    throw new ArgumentException("Значение порта должно быть в диапазоне 0..65535");
                }

                //Если доменное имя, то добавляются кавычки
                if (domenOrIPv4Check == 1) {
                    dataToPort = "USERHOST N=" + numbServerProperties +
                        " IP=" + domenOrIPData.Text + "; PORT=" + portData.Text;

                    //Если IP адрес, то просто передаётся он
                } else {
                    dataToPort = "USERHOST N=" + numbServerProperties +
                        " IP=" + domenOrIPData.Text + "; PORT=" + portData.Text;
                }
            }

            serialPort.WriteLine(dataToPort);

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line = "";

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();

                    if (line.Contains("Za) USERHOST") && line.EndsWith("OK")) {
                        return;

                    } else if (line.Contains("Za) USERHOST") && line.Contains("ERROR")) {
                        domenOrIPData.Focus();
                        domenOrIPData.SelectAll();

                        if (domenOrIPv4Check == 2) {
                            throw new FormatException("Неверный формат записи IPv6, диапазон каждого значения в одном поле (XXXX) должен быть от 0..F (HEX)" +
                            "\nДопускается сокращённый вид записи" +
                            "\nПримеры записи:" +
                            "\n2001:0DB0:0000:123A:0000:0000:0000:0030" +
                            "\n2001:DB0:0:123A:0:0:0:30" +
                            "\n2001:DB0:0:123A::30");

                        } else {
                            throw new MKCommandException("Не удалось записать параметры сервера №" + (numbServerProperties));
                        }
                    }
                }
            }

            throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
        }

        /// <summary>
        /// Находит порт для опроса модема
        /// </summary>
        private int getNbModemPort() {
            CP2105_Connector.GetCP2105_ConnectorInstance().FindDevicePorts();
            int enh = CP2105_Connector.GetCP2105_ConnectorInstance().getEnhancedPort();

            if (serialPort.IsOpen) serialPort.Close();

            serialPort.PortName = "COM" + enh;
            return enh;
        }

        /// <summary>
        /// Проверяет, что микроконтроллер не выключен
        /// </summary>
        private void checkGPIO_CP2105() {
            if (!CP2105_Connector.GetCP2105_ConnectorInstance().GetStageGPIOEnhabcedPort().stageGPIO_1) {
                CP2105_Connector.GetCP2105_ConnectorInstance().WriteGPIOStageAndSetFlags(getNbModemPort(), true, true, 200, false);
            }
        }

        /// <summary>
        /// Возвращает все параметры из микроконтроллера, которая выдаёт команда DEVICE
        /// </summary>
        /// <returns>
        /// Ключи Map'ы по параметрам: 
        /// <para>ZPORT - Версия Zport'a</para>
        /// <para>MDMMODEL - модель модуля Quectel</para>
        /// <para>FW - версия прошивки микроконтроллера</para>
        /// <para>COPY_ID - номер копии прошивки</para>
        /// <para>TARGET - targetID</para>
        /// <para>IDX - Индекс id</para>
        /// <para>PROT_ID - Протокол ID</para>
        /// <para>FUN - не знаю, но пусть будет тут</para>
        /// </returns>
        private Dictionary<string, string> getParamsDeviceCommand() {

            if (!serialPort.IsOpen) serialPort.Open();
            Dictionary<string, string> paramsDictionary = new Dictionary<string, string>();

            //Пример ответа:
            //Za) DEVICE (ZPORT:ZP009 /MDMMODEL:QBC92) (FW:ZU018 /COPY_ID:27F87014_000B"hex") (TARGET:1 /IDX:3 /PROT_ID:2 /FUN:00"hex") OK
            serialPort.WriteLine("DEVICE");

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line = "";

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();

                    if (line.Contains("Za) DEVICE") && line.EndsWith("OK")) {

                        string[] arrLine = line.Split(':');

                        paramsDictionary.Add("ZPORT", arrLine[1].Substring(0, 5));
                        paramsDictionary.Add("MDMMODEL", arrLine[2].Substring(0, arrLine[2].IndexOf(')')));
                        paramsDictionary.Add("FW", arrLine[3].Substring(0, 5));
                        paramsDictionary.Add("COPY_ID", arrLine[4].Substring(0, arrLine[4].IndexOf(')')));
                        paramsDictionary.Add("TARGET", arrLine[5].Substring(0, arrLine[5].IndexOf(' ')));
                        paramsDictionary.Add("IDX", arrLine[6].Substring(0, arrLine[6].IndexOf(' ')));
                        paramsDictionary.Add("PROT_ID", arrLine[7].Substring(0, arrLine[7].IndexOf(' ')));
                        paramsDictionary.Add("FUN", arrLine[8].Substring(0, arrLine[8].IndexOf(')')));

                        if (serialPort.IsOpen) serialPort.Close();
                        return paramsDictionary;

                    } else if (line.Contains("Za) DEVICE") && line.Contains("ERROR")) {

                        if (serialPort.IsOpen) serialPort.Close();
                        throw new MKCommandException("Ответ микроконтроллера на команду \"DEVICR\": " + line);
                    }
                }
            }

            if (serialPort.IsOpen) serialPort.Close();
            throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
        }

        /// <summary>
        /// Возвращает значения команды MDMIMID модема
        /// <br>Ключ IMEI</br>
        /// <br>Ключ IMSI</br>
        /// <br>Ключ ICCID</br>
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> getMDMIMIDCommandParams() {

            if (!serialPort.IsOpen) serialPort.Open();

            serialPort.WriteLine("MDMIMID");

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line;

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();

                    //Пример ответа: MDMIMID (IMEI:) (IMSI:) (ICCID:) OK
                    if (line.Contains("Za) MDMIMID") && line.EndsWith("OK")) {

                        string[] arrLine = line.Split('(');
                        string imei = arrLine[1].Substring(arrLine[1].IndexOf(':') + 1, arrLine[1].LastIndexOf(')') - (arrLine[1].IndexOf(':') + 1));
                        string imsi = arrLine[2].Substring(arrLine[2].IndexOf(':') + 1, arrLine[2].LastIndexOf(')') - (arrLine[2].IndexOf(':') + 1));
                        string iccid = arrLine[3].Substring(arrLine[3].IndexOf(':') + 1, arrLine[3].LastIndexOf(')') - (arrLine[3].IndexOf(':') + 1));

                        return new Dictionary<string, string>() { { "IMEI", imei }, { "IMSI", imsi }, { "ICCID", iccid } };

                    } else if (line.Contains("MDMIMID") && line.Contains("ERROR")) {

                        if (serialPort.IsOpen) serialPort.Close();
                        throw new MKCommandException("Ответ микроконтроллера на команду \"MDMIMID\": " + line);
                    }
                }
            }

            if (serialPort.IsOpen) serialPort.Close();
            throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
        }

        /// <summary>
        /// Возвращает данные на команду BASEHOST с микроконтроллера
        /// <br>Ключ "IP" выдаёт доменное имя или IP базового сервера</br>
        /// <br>Ключ "PORT" выдаёт порт базового сервера</br>
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> getBaseHostParams() {

            if (!serialPort.IsOpen) serialPort.Open();

            serialPort.WriteLine("BASEHOST");

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line;

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    /*Zc) BASEHOST
                    Za) BASEHOST (IP:0::FF) (PORT:1243) OK*/
                    line = serialPort.ReadLine();

                    if (line.Contains("Za) BASEHOST") && line.EndsWith("OK")) {

                        string[] arrLine = line.Split('(');

                        string ip = arrLine[1].Substring(arrLine[1].IndexOf(':') + 1, arrLine[1].LastIndexOf(')') - (arrLine[1].IndexOf(':') + 1));
                        string port = arrLine[2].Substring(arrLine[2].IndexOf(':') + 1, arrLine[2].LastIndexOf(')') - (arrLine[2].IndexOf(':') + 1));

                        return new Dictionary<string, string>() { { "IP", ip }, { "PORT", port } };

                    } else if (line.Contains("BASEHOST") && line.Contains("ERROR")) {

                        if (serialPort.IsOpen) serialPort.Close();
                        throw new MKCommandException("Ответ микроконтроллера на команду \"BASEHOST\": " + line);
                    }
                }
            }

            if (serialPort.IsOpen) serialPort.Close();
            throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
        }

        /// <summary>
        /// Возвращает данные на команду BASEHOST с микроконтроллера
        /// <br>Ключ "IP" выдаёт доменное имя или IP базового сервера</br>
        /// <br>Ключ "PORT" выдаёт порт базового сервера</br>
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> getAPNParams() {

            if (!serialPort.IsOpen) serialPort.Open();

            serialPort.WriteLine("APN");

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line;

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    /*Zc) APN
                    Za) APN (BASE:"testTestovskiy") (USER:) OK*/
                    line = serialPort.ReadLine();

                    if (line.Contains("Za) APN") && line.EndsWith("OK")) {

                        string[] arrLine = line.Split('(');

                        string BASE = arrLine[1].Substring(arrLine[1].IndexOf(':') + 1, arrLine[1].LastIndexOf(')') - (arrLine[1].IndexOf(':') + 1));
                        string user = arrLine[2].Substring(arrLine[2].IndexOf(':') + 1, arrLine[2].LastIndexOf(')') - (arrLine[2].IndexOf(':') + 1));

                        return new Dictionary<string, string>() { { "BASE", BASE }, { "USER", user } };

                    } else if (line.Contains("APN") && line.Contains("ERROR")) {

                        if (serialPort.IsOpen) serialPort.Close();
                        throw new MKCommandException("Ответ микроконтроллера на команду \"BASEHOST\": " + line);
                    }
                }
            }

            if (serialPort.IsOpen) serialPort.Close();
            throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
        }

        /// <summary>
        /// Считывает параметры сервера (Доменное имя/IPv4 и порт) с помощью протокал ZPORT'a
        /// </summary>
        /// <param name="i">Номер сервера</param>
        /// <param name="domenNameOrIP">Записанное доменное имя</param>
        /// <param name="port">Записанный порт</param>
        private void getIPv4AndPortUserHost(int i, ref string domenNameOrIP, ref string port) {
            domenNameOrIP = "";
            port = "";

            serialPort.WriteLine("USERHOST N=" + i);

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line;

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();

                    if (line.Contains("Za) USERHOST") && line.EndsWith("OK")) {

                        string[] arrLine = line.Split('(');

                        //Получаю IP или доменное имя
                        //IP:0:0:0:0:0:0:0:0)
                        domenNameOrIP = arrLine[2].Substring(3, arrLine[2].IndexOf(')') - 3);

                        //Получаю порт
                        //PORT:8103) OK
                        port = arrLine[3].Substring(5, arrLine[3].IndexOf(')') - 5);

                        return;
                    }
                }
            }

            throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
        }

        /// <summary>
        /// Получает значения из модема команды Connecting
        /// </summary>
        private void getCONNECTINGparametersAndSetInConnectingPanel() {

            serialPort.WriteLine("CONNECTING");

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line = "";

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();

                    if (line.Contains("Za) CONNECTING") && line.EndsWith("OK")) {

                        StringBuilder strBuild = new StringBuilder("");

                        //Za) CONNECTING (PERIOD:6:00:00) (SERVICE:20::) (LETWAIT: :1:00) (TRYLIMIT:3) (SESSLIMIT: :20:) (HOLDTIME: :4:16) "Note h:m:s" OK
                        //Za) CONNECTING (PERIOD:6:00:00) (SERVICE:20::) (LETWAIT: :1:00) (TRYLIMIT:3) (SESSLIMIT: :20:) (HOLDTIME: :4:16) (INCOMHOLDTIME: :3:00) "Note h:m:s" DEMO OK
                        string[] answer = line.Split('(');

                        //Добавляю PERIOD
                        string period = answer[1].Substring(answer[1].IndexOf(':') + 1, answer[1].IndexOf(')') - answer[1].IndexOf(':') - 1);
                        periodMsdTxtBx.Text = parseTimeForMskTxtBox(period, false);

                        //Добавляю SERVICE
                        string service = answer[2].Substring(answer[2].IndexOf(':') + 1, answer[2].IndexOf(')') - answer[2].IndexOf(':') - 1);
                        serviceMsdTxtBx.Text = parseTimeForMskTxtBox(service, false);

                        //Добавляю LETWAIT
                        string letwait = answer[3].Substring(answer[3].IndexOf(':') + 1, answer[3].IndexOf(')') - answer[3].IndexOf(':') - 1);
                        letwaitMsdTxtBx.Text = parseTimeForMskTxtBox(letwait, false);

                        //Добавляю TRYLIMIT
                        string trylimit = answer[4].Substring(answer[4].IndexOf(':') + 1, answer[4].IndexOf(')') - answer[4].IndexOf(':') - 1);
                        trylimitMsdTxtBx.Text = trylimit;

                        //Добавляю SESSLIMIT
                        string sesslimit = answer[5].Substring(answer[5].IndexOf(':') + 1, answer[5].IndexOf(')') - answer[5].IndexOf(':') - 1);
                        sesslimitMsdTxtBx.Text = parseTimeForMskTxtBox(sesslimit, false);

                        //Добавляю HOLDTIME
                        string holdtime = answer[6].Substring(answer[6].IndexOf(':') + 1, answer[6].IndexOf(')') - answer[6].IndexOf(':') - 1);
                        holdTimeMsdTxtBx.Text = parseTimeForMskTxtBox(holdtime, false);

                        if (Convert.ToInt32(verProtTxtBx.Text.Substring(2)) >= 9) {
                            //Добавляю INCOMHOLDTIME
                            holdtime = answer[7].Substring(answer[7].IndexOf(':') + 1, answer[7].IndexOf(')') - answer[7].IndexOf(':') - 1);
                            incomholdtimeMskTxtBx.Text = parseTimeForMskTxtBox(holdtime, false);
                            groupBox11.Enabled = true;
                        }

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает строку в формате hh:mm:ss
        /// </summary>
        /// <param name="modemAnswer">Строка времени, которая пришла из модема</param>
        /// <returns></returns>
        private string parseTimeForMskTxtBox(string modemAnswer, bool onlyMMandSS) {

            StringBuilder parseTime = new StringBuilder("");
            int countColon = 0;

            foreach (string answerValue in modemAnswer.Trim().Split(':')) {

                if (String.IsNullOrEmpty(answerValue.Trim())) {
                    if (onlyMMandSS) {
                        onlyMMandSS = false;
                        continue;
                    } else
                        parseTime.Append("00");

                } else if (answerValue.Length < 2) {
                    parseTime.Append("0" + answerValue);

                } else {
                    parseTime.Append(answerValue);
                }

                if (countColon < 2) {
                    parseTime.Append(":");
                    countColon++;
                }
            }

            return parseTime.ToString();
        }

        /// <summary>
        /// Записывает параметры команды CONNECTING в микроконтроллер
        /// </summary>
        private void writeConnectingParameters() {

            Dictionary<string, string> dictAllowedValues = defaultValuesForCommandCONNECTING();

            //Формирую сообщение на отправку данных
            string comandInCOM = "CONNECTING";

            if (dictAllowedValues.ContainsKey("PERIOD")) {
                comandInCOM += " PERIOD=" + periodMsdTxtBx.Text + ";";
            }

            if (dictAllowedValues.ContainsKey("SERVICE")) {
                comandInCOM += " SERVICE=" + serviceMsdTxtBx.Text + ";";
            }

            if (dictAllowedValues.ContainsKey("LETWAIT")) {
                comandInCOM += " LETWAIT=" + letwaitMsdTxtBx.Text + ";";
            }

            if (dictAllowedValues.ContainsKey("TRYLIMIT")) {
                comandInCOM += " TRYLIMIT=" + trylimitMsdTxtBx.Text + ";";
            }

            if (dictAllowedValues.ContainsKey("SESSLIMIT")) {
                comandInCOM += " SESSLIMIT=" + sesslimitMsdTxtBx.Text + ";";
            }

            if (dictAllowedValues.ContainsKey("HOLDTIME")) {
                comandInCOM += " HOLDTIME=" + holdTimeMsdTxtBx.Text + ";";
            }

            if (dictAllowedValues.ContainsKey("INCOMHOLDTIME")) {
                comandInCOM += " INCOMHOLDTIME=" + holdTimeMsdTxtBx.Text + ";";
            }

            getNbModemPort();

            if (!serialPort.IsOpen) serialPort.Open();

            serialPort.WriteLine(comandInCOM);

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line = "";

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();

                    if (line.Contains("Za) CONNECTING") && line.EndsWith("OK")) {
                        if (serialPort.IsOpen) serialPort.Close();
                        return;

                    } else if (line.Contains("Za) CONNECTING") && line.Contains("ERROR")) {
                        if (serialPort.IsOpen) serialPort.Close();
                        throw new DeviceError("Не удалось получить подтверждение от модема, что данные записаны, попробуйте снова");
                    }
                }
            }
        }

        /// <summary>
        /// Переводит значение времени в секунды
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private int getSeconds(int hours, int minutes, int seconds) {
            return (hours * 3600) + (minutes * 60) + seconds;
        }

        private int getSeconds(string timeHHmmSS) {
            string[] time = timeHHmmSS.Split(':');

            return (Convert.ToInt32(time[0]) * 3600) + (Convert.ToInt32(time[1]) * 60) + Convert.ToInt32(time[2]);
        }

        /// <summary>
        /// Проверяет формат времени введённый пользователем
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <param name="maskedTextBox"></param>
        private void checkTimeFormat(int hours, int minutes, int seconds, MaskedTextBox maskedTextBox) {
            //Проверка на корректность значений
            if (hours > 99 || minutes > 59 || seconds > 59) {
                maskedTextBox.Focus();
                maskedTextBox.SelectAll();
                throw new FormatException("Неверный формат введенных данных, значение часов не может быть больше 24-х, а значение минут или секунд не может быть больше 59-ти");
            }
        }

        /// <summary>
        /// Проверяет на то, что маска текстбокса заполнена верно
        /// </summary>
        /// <param name="timeField"></param>
        private void checkFieldTimeFormat(string[] timeField, MaskedTextBox maskedLield, string errorMess) {
            foreach (string time in timeField) {
                if (time.Trim().Length != 2) {
                    maskedLield.Focus();
                    maskedLield.SelectAll();
                    throw new FormatException(errorMess);
                }
            }
        }

        private void connectingAcceptBtn_Click(object sender, EventArgs e) {
            try {

                connectOnCOMterminal = flasMainForm.getStateConnectionOnCOMofTerminalForm();
                if (connectOnCOMterminal) {
                    flasMainForm.performClickConnOfTerminalForm();
                }

                checkValidConnectingParameters();

                writeConnectingParameters();

                if (connectOnCOMterminal) flasMainForm.performClickConnOfTerminalForm();

                refreshInfoBtn.PerformClick();

                Flasher.successfullyDialog("Параметры инициализации связи успешно записаны", "Запись параметров");

            } catch (Exception ex) {

                Flasher.exceptionDialog(ex.Message);

                if (serialPort.IsOpen) serialPort.Close();
                if (connectOnCOMterminal) flasMainForm.performClickConnOfTerminalForm();
            }
        }

        /// <summary>
        /// Действие при нажатии на кнопку запись параметров в вкладке: Настройка пользовательских серверов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setServersSettingsBtn_Click(object sender, EventArgs e) {
            try {
                Cursor = Cursors.WaitCursor;

                connectOnCOMterminal = flasMainForm.getStateConnectionOnCOMofTerminalForm();
                if (connectOnCOMterminal) {
                    flasMainForm.performClickConnOfTerminalForm();
                }

                getNbModemPort();
                checkGPIO_CP2105();

                if (!serialPort.IsOpen) serialPort.Open();

                writeCustomServersProperties();

                if (serialPort.IsOpen) serialPort.Close();

                if (connectOnCOMterminal) flasMainForm.performClickConnOfTerminalForm();

                refreshInfoBtn.PerformClick();

                Cursor = Cursors.Default;

                Flasher.successfullyDialog("Настройка пользовательских серверов успешно записаны", "Запись параметров");

            } catch (Exception ex) {

                Cursor = Cursors.Default;
                Flasher.exceptionDialog(ex.Message);
                if (serialPort.IsOpen) serialPort.Close();
                if (connectOnCOMterminal) flasMainForm.performClickConnOfTerminalForm();
            }
        }

        private void ModemConfig_FormClosed(object sender, FormClosedEventArgs e) {
            flasMainForm.removeModemConfigForm();
        }

        private string oldValueDomenName_1 = "";
        private string oldValueIPv4_1 = "";
        private string oldValueIPv6_1 = "";
        private void ipDomenNameTxtBx_1_TextChanged(object sender, EventArgs e) {
            if (domenNameRdBtn_1.Checked) {
                oldValueDomenName_1 = ipDomenNameTxtBx_1.Text;

            } else if (IPv4RdBtn_1.Checked) {
                oldValueIPv4_1 = ipDomenNameTxtBx_1.Text;

            } else {
                oldValueIPv6_1 = ipDomenNameTxtBx_1.Text;
            }
        }

        private string oldValueDomenName_2 = "";
        private string oldValueIPv4_2 = "";
        private string oldValueIPv6_2 = "";
        private void ipDomenNameTxtBx_2_TextChanged(object sender, EventArgs e) {
            if (domenNameRdBtn_2.Checked) {
                oldValueDomenName_2 = ipDomenNameTxtBx_2.Text;

            } else if (IPv4RdBtn_2.Checked) {
                oldValueIPv4_2 = ipDomenNameTxtBx_2.Text;

            } else {
                oldValueIPv6_2 = ipDomenNameTxtBx_2.Text;
            }
        }

        private string oldValueDomenName_3 = "";
        private string oldValueIPv4_3 = "";
        private string oldValueIPv6_3 = "";
        private void ipDomenNameTxtBx_3_TextChanged(object sender, EventArgs e) {
            if (domenNameRdBtn_3.Checked) {
                oldValueDomenName_3 = ipDomenNameTxtBx_3.Text;

            } else if (IPv4RdBtn_3.Checked) {
                oldValueIPv4_3 = ipDomenNameTxtBx_3.Text;

            } else {
                oldValueIPv6_3 = ipDomenNameTxtBx_3.Text;
            }
        }

        private void domenNameRdBtnGroup_1_CheckedChanged(object sender, EventArgs e) {

            if (domenNameRdBtn_1.Checked) {
                ipDomenNameTxtBx_1.Text = oldValueDomenName_1;
                toolTipForUserHostParam.SetToolTip(ipDomenNameTxtBx_1, domenNametoolTipMess);

            } else if (IPv4RdBtn_1.Checked) {
                ipDomenNameTxtBx_1.Text = oldValueIPv4_1;
                toolTipForUserHostParam.SetToolTip(ipDomenNameTxtBx_1, ipV4toolTipMess);

            } else {
                ipDomenNameTxtBx_1.Text = oldValueIPv6_1;
                toolTipForUserHostParam.SetToolTip(ipDomenNameTxtBx_1, ipV6toolTipMess);
            }
        }

        private void domenNameRdBtnGroup_2_CheckedChanged(object sender, EventArgs e) {
            if (domenNameRdBtn_2.Checked) {
                ipDomenNameTxtBx_2.Text = oldValueDomenName_2;
                toolTipForUserHostParam.SetToolTip(ipDomenNameTxtBx_2, domenNametoolTipMess);

            } else if (IPv4RdBtn_2.Checked) {
                ipDomenNameTxtBx_2.Text = oldValueIPv4_2;
                toolTipForUserHostParam.SetToolTip(ipDomenNameTxtBx_2, ipV4toolTipMess);

            } else {
                ipDomenNameTxtBx_2.Text = oldValueIPv6_2;
                toolTipForUserHostParam.SetToolTip(ipDomenNameTxtBx_2, ipV6toolTipMess);
            }
        }

        private void domenNameRdBtnGroup_3_CheckedChanged(object sender, EventArgs e) {
            if (domenNameRdBtn_3.Checked) {
                ipDomenNameTxtBx_3.Text = oldValueDomenName_3;
                toolTipForUserHostParam.SetToolTip(ipDomenNameTxtBx_3, domenNametoolTipMess);

            } else if (IPv4RdBtn_3.Checked) {
                ipDomenNameTxtBx_3.Text = oldValueIPv4_3;
                toolTipForUserHostParam.SetToolTip(ipDomenNameTxtBx_3, ipV4toolTipMess);

            } else {
                ipDomenNameTxtBx_3.Text = oldValueIPv6_3;
                toolTipForUserHostParam.SetToolTip(ipDomenNameTxtBx_3, ipV6toolTipMess);
            }
        }

        public void enableReadWritebtnInFrame(bool enbleBtn) {

            splitContainer1.Invoke((MethodInvoker)delegate {
                refreshInfoBtn.Enabled = enbleBtn;
                setServersSettingsBtn.Enabled = enbleBtn;
                connectingAcceptBtn.Enabled = enbleBtn;
                loadScript.Enabled = enbleBtn;
            });
        }

        //=============================== Работа со скриптами, запись параметров в файл и из файла ========================
        /// <summary>
        /// Действии при нажатии на кнопку сохранения скрипта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createScript_Click(object sender, EventArgs e) {

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = Properties.Settings.Default.modemConfig_pathToSaveScript;

            saveFileDialog.FileName = "taipitConfigModemScript " + DateTime.Now.ToString("yyyy_MM_dd_HH-mm-ss") + ".dat";
            saveFileDialog.Filter = "dat (*.dat*)|*.dat*";
            saveFileDialog.DefaultExt = "dat";
            saveFileDialog.FilterIndex = 0;

            if (saveFileDialog.ShowDialog() == DialogResult.OK) {

                Properties.Settings.Default.Save();

                try {
                    connectOnCOMterminal = flasMainForm.getStateConnectionOnCOMofTerminalForm();
                    if (connectOnCOMterminal) {
                        flasMainForm.performClickConnOfTerminalForm();
                    }

                    checkValidParametersForScrypt();

                    if (connectOnCOMterminal) flasMainForm.performClickConnOfTerminalForm();

                    ModemConfigScript modemConfigScript = new ModemConfigScript(generateUserHostParametersForTheScript() + "\n" + generateConnectingParametersForTheScript());

                    using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate)) {
                        new BinaryFormatter().Serialize(fs, modemConfigScript);
                    }

                    Flasher.successfullyDialog("Скрипт успешно сохранен", "Сохранение скрипта");

                } catch (Exception ex) {
                    Flasher.exceptionDialog("Произошла ошибка при записи скрипта:\n" + ex.Message);
                }
            }
        }

        /// <summary>
        /// Проверяет валидность значений в окне с конфигурацией модема
        /// </summary>
        private void checkValidParametersForScrypt() {
            //Проверка валидности значений в полях с вкладки настпройки пользовательских серверов
            checkValidUserHostParameters(formatAddresRecord(1), ipDomenNameTxtBx_1, portTxtBx_1);
            checkValidUserHostParameters(formatAddresRecord(2), ipDomenNameTxtBx_2, portTxtBx_2);
            checkValidUserHostParameters(formatAddresRecord(3), ipDomenNameTxtBx_3, portTxtBx_3);

            //Проверка валидности значений в полях с вкладки параметры инициализации связи
            checkValidConnectingParameters();
        }

        /// <summary>
        /// Возвращает сформированные параметры из вкладки параметры пользовательских серверов для текстового файла (скрипта)
        /// </summary>
        /// <returns></returns>
        private string generateUserHostParametersForTheScript() {
            /*Пример:
             * Настройка пользовательских серверов
             * 0,Доменное имя,"devices.226.taipit.ru",8080
             * 1,IPv4,124.12.55.255,8080
             * 2,IPv6,12AF:3B:38C::10,8080
             */

            return "Настройка пользовательских серверов\n" +
                generateLineUserHostParameters(0, formatAddresRecord(1), ipDomenNameTxtBx_1, portTxtBx_1) + "\n" +
                generateLineUserHostParameters(1, formatAddresRecord(2), ipDomenNameTxtBx_2, portTxtBx_2) + "\n" +
                generateLineUserHostParameters(2, formatAddresRecord(3), ipDomenNameTxtBx_3, portTxtBx_3) + "\n";
        }

        /// <summary>
        /// Генерирует строку с параметрами, которые соответствуют параметрам определённого сервера
        /// </summary>
        /// <param name="numbServerProperties">Номер настройки сервера</param>
        /// <param name="domenOrIPv4Check">Формат записи адреса</param>
        /// <param name="domenOrIPData">Значение адреса</param>
        /// <param name="portData">Значение порта</param>
        /// <returns></returns>
        private string generateLineUserHostParameters(int numbServerProperties, int domenOrIPv4Check, TextBox domenOrIPData, TextBox portData) {

            if (String.IsNullOrEmpty(domenOrIPData.Text)) {
                return numbServerProperties + ", EMPTY";

            } else {
                switch (domenOrIPv4Check) {
                    //Доменное имя
                    case 0: {
                            return numbServerProperties + ", Доменное имя, " + domenOrIPData.Text.Trim() + ", " + portData.Text.Trim();
                        }

                    //IPv4
                    case 1: {
                            return numbServerProperties + ", IPv4, " + domenOrIPData.Text.Trim() + ", " + portData.Text.Trim();
                        }

                    //IPv6
                    case 2: {
                            return numbServerProperties + ", IPv6, " + domenOrIPData.Text.Trim() + ", " + portData.Text.Trim();
                        }

                    default: {
                            throw new ArgumentException("Формат записи адреса не реализован");
                        }
                }
            }
        }

        /// <summary>
        /// Возвращает сформированные параметры из вкладки Параметры инициализации связи для текстового файла (скрипта)
        /// </summary>
        /// <returns></returns>
        private string generateConnectingParametersForTheScript() {
            /*Пример:
            Параметры инициализации связи
            PERIOD-06:00:00
            SERVICE-20
            LETWAIT-01:00
            TRYLIMIT-3
            SESSLIMIT-00:20
            HOLDTIME-04:16
            INCOMHOLDTIME-03:00*/

            return "Параметры инициализации связи\n" +
                "PERIOD-" + periodMsdTxtBx.Text + "\n" +
                "SERVICE-" + serviceMsdTxtBx.Text + "\n" +
                "LETWAIT-" + letwaitMsdTxtBx.Text + "\n" +
                "TRYLIMIT-" + trylimitMsdTxtBx.Text + "\n" +
                "SESSLIMIT-" + sesslimitMsdTxtBx.Text + "\n" +
                "HOLDTIME-" + holdTimeMsdTxtBx.Text + "\n" +
                "INCOMHOLDTIME-" + incomholdtimeMskTxtBx.Text;
        }

        /// <summary>
        /// Проверяет валидность значений с вкладки настпройки пользовательских серверов, а именно значение порта и адреса
        /// </summary>
        /// <param name="domenOrIPv4Check">В каком формате передаётся адрес</param>
        /// <param name="domenOrIPData">Значение адреса</param>
        /// <param name="portData">Значение порта</param>
        private void checkValidUserHostParameters(int domenOrIPv4Check, TextBox domenOrIPData, TextBox portData) {

            //Если значения IP адреса или Доменного имени не пустое
            if (!(String.IsNullOrEmpty(domenOrIPData.Text) ||
                (domenOrIPData.Text.Length == 2 && (domenOrIPData.Text.ElementAt(0) == '\"' && domenOrIPData.Text.ElementAt(1) == '\"')))) {

                //Если должно поступить на вход доменное имя
                switch (domenOrIPv4Check) {
                    case 0: {
                            char[] domenNameArr = domenOrIPData.Text.ToCharArray();

                            if (domenNameArr[0] != '\"' || domenNameArr[domenNameArr.Length - 1] != '\"') {
                                domenOrIPData.Focus();
                                domenOrIPData.SelectAll();
                                throw new FormatException("Доменное имя должно содержать знак \" в начале и конце");
                            }

                            //C учётом кавычек
                            if (domenNameArr.Length - 2 > 28) {
                                domenOrIPData.Focus();
                                domenOrIPData.SelectAll();
                                throw new FormatException("Доменное имя не должно быть больше 28 символов");
                            }

                            for (int i = 1; i < domenNameArr.Length - 1; i++) {
                                if (domenNameArr[i] < 0x20 || domenNameArr[i] > 0x7F) {
                                    domenOrIPData.Focus();
                                    domenOrIPData.SelectAll();
                                    throw new FormatException("Доменное имя должно содержать только ASCII символы");
                                }
                            }
                        }
                        break;

                    //Если должен поступить на вход IPv4 адрес
                    case 1: {
                            //Проверка значений
                            try {
                                string ipv4 = domenOrIPData.Text.Trim();

                                string[] ipv4Arr = ipv4.Split('.');

                                List<byte> byteList = new List<byte>();

                                if (ipv4Arr.Length != 4) throw new FormatException();

                                foreach (string ipNode in ipv4Arr) {
                                    if (String.IsNullOrEmpty(ipNode.Trim())) {
                                        byteList.Add(0);
                                        continue;
                                    }

                                    int localByte = Convert.ToInt32(ipNode.Trim());

                                    if (localByte < 0 || localByte > 255) throw new FormatException();

                                    byteList.Add((byte)localByte);
                                }

                                domenOrIPData.Text = byteList[0] + "." +
                                    byteList[1] + "." +
                                    byteList[2] + "." +
                                    byteList[3];

                            } catch (Exception) {
                                domenOrIPData.Focus();
                                domenOrIPData.SelectAll();
                                throw new FormatException("Неверный формат записи IPv4, формат должен иметь вид xxx.xxx.xxx.xxx, где xxx могут иметь значения от 0..255");
                            }

                        }
                        break;

                    //Если должен поступить на вход IPv6 адрес
                    case 2: {
                            try {
                                string ipv6 = domenOrIPData.Text.Trim();

                                IPv6Parser.checValidValue(ipv6);

                            } catch (Exception) {
                                domenOrIPData.Focus();
                                domenOrIPData.SelectAll();
                                throw;
                            }

                        }
                        break;
                }

                //Проверка коректности ввода порта
                if (String.IsNullOrEmpty(portData.Text) ||
                    (Convert.ToInt32(portData.Text) > 65535 || Convert.ToInt32(portData.Text) < 0)) {
                    portData.Focus();
                    portData.SelectAll();
                    throw new FormatException("Значение порта должно быть в диапазоне 0..65535");
                }
            }
        }

        /// <summary>
        /// Проверка валидности значений в полях с вкладки параметры инициализации связи
        /// </summary>
        private void checkValidConnectingParameters() {

            int hours;
            int minutes;
            int seconds;
            int timeInSeconds;
            string[] periodArr;

            //Если версия ZPort'a не известна, то обновляю информацию (ПОДУМАТЬ!!!!!!!!)
            if (String.IsNullOrEmpty(verProtTxtBx.Text)) {
                refreshInfoBtn.PerformClick();

                if (String.IsNullOrEmpty(verProtTxtBx.Text)) return;
            }

            //Za) CONNECTING (PERIOD:6:00:00) (SERVICE:20::) (LETWAIT: :1:00) (TRYLIMIT:3) (SESSLIMIT: :20:) (HOLDTIME: :4:16) (INCOMHOLDTIME: :3:00) "Note h:m:s" DEMO OK
            Dictionary<string, string> minValues = minValuesForCommandCONNECTING();
            Dictionary<string, string> maxValues = maxValuesForCommandCONNECTING();

            //Проверка на валидность параметров
            //Период инициализации сеансов связи
            if (minValues.ContainsKey("PERIOD")) {

                periodArr = periodMsdTxtBx.Text.Split(':');
                checkFieldTimeFormat(periodArr, periodMsdTxtBx, "Формат времени поля \"Период инициализации сеансов связи\" должен иметь формат hh:mm:ss");

                hours = Convert.ToInt32(periodArr[0]);
                minutes = Convert.ToInt32(periodArr[1]);
                seconds = Convert.ToInt32(periodArr[2]);

                checkTimeFormat(hours, minutes, seconds, periodMsdTxtBx);

                //Получаю время в секундах введённое пользователем
                timeInSeconds = getSeconds(hours, minutes, seconds);

                //Проверка на границы
                if (timeInSeconds < getSeconds(minValues["PERIOD"]) || timeInSeconds > getSeconds(maxValues["PERIOD"])) {
                    periodMsdTxtBx.Focus();
                    periodMsdTxtBx.SelectAll();
                    throw new ArgumentException("Периодичность инициации сеансов связи не может быть меньше " + minValues["PERIOD"] + " и больше " + maxValues["PERIOD"]);
                }
            }

            //Период инициализации севнсов связи с базовым сервером
            if (minValues.ContainsKey("SERVICE")) {

                periodArr = serviceMsdTxtBx.Text.Split(':');
                checkFieldTimeFormat(periodArr, serviceMsdTxtBx, "Формат времени поля \"Периодичность инициации сеансов связи с базовым сервером\" должен иметь формат hh:mm:ss");

                hours = Convert.ToInt32(periodArr[0]);
                minutes = Convert.ToInt32(periodArr[1]);
                seconds = Convert.ToInt32(periodArr[2]);

                checkTimeFormat(hours, minutes, seconds, serviceMsdTxtBx);

                timeInSeconds = getSeconds(hours, minutes, seconds);

                //Проверка на границы
                if (timeInSeconds < getSeconds(minValues["SERVICE"]) || timeInSeconds > getSeconds(maxValues["SERVICE"])) {
                    serviceMsdTxtBx.Focus();
                    serviceMsdTxtBx.SelectAll();
                    throw new ArgumentException("Периодичность инициации сеансов связи с базовым сервером не может быть меньше " + minValues["SERVICE"] + " и больше " + maxValues["SERVICE"]);
                }
            }

            //Время ожидания ответа сервера
            if (minValues.ContainsKey("LETWAIT")) {

                periodArr = letwaitMsdTxtBx.Text.Split(':');

                checkFieldTimeFormat(periodArr, letwaitMsdTxtBx, "Формат времени поля \"Время ожидания ответа сервера\" должен иметь формат hh:mm:ss");

                hours = Convert.ToInt32(periodArr[0]);
                minutes = Convert.ToInt32(periodArr[1]);
                seconds = Convert.ToInt32(periodArr[2]);

                checkTimeFormat(hours, minutes, seconds, letwaitMsdTxtBx);

                timeInSeconds = getSeconds(hours, minutes, seconds);

                if (timeInSeconds < getSeconds(minValues["LETWAIT"]) || timeInSeconds > getSeconds(maxValues["LETWAIT"])) {
                    letwaitMsdTxtBx.Focus();
                    letwaitMsdTxtBx.SelectAll();
                    throw new ArgumentException("Время ожидания ответа сервера не может быть меньше " + minValues["LETWAIT"] + " и больше " + maxValues["LETWAIT"]);
                }
            }

            //Количество попыток связи с сервером
            if (minValues.ContainsKey("TRYLIMIT")) {
                int amoutTry = Convert.ToInt32(trylimitMsdTxtBx.Text);

                if (amoutTry < Convert.ToInt32(minValues["TRYLIMIT"]) || amoutTry > Convert.ToInt32(maxValues["TRYLIMIT"])) {
                    trylimitMsdTxtBx.Focus();
                    trylimitMsdTxtBx.SelectAll();
                    throw new ArgumentException("Количество попыток связи с сервером не может быть меньше " + minValues["TRYLIMIT"] + " и больше " + maxValues["TRYLIMIT"]);
                }
            }

            //Предельное время сеанса связи
            if (minValues.ContainsKey("SESSLIMIT")) {
                periodArr = sesslimitMsdTxtBx.Text.Split(':');

                checkFieldTimeFormat(periodArr, sesslimitMsdTxtBx, "Формат времени поля \"Предельное время сеанса связи\" должен иметь формат hh:mm:ss");

                hours = Convert.ToInt32(periodArr[0]);
                minutes = Convert.ToInt32(periodArr[1]);
                seconds = Convert.ToInt32(periodArr[2]);

                checkTimeFormat(hours, minutes, seconds, sesslimitMsdTxtBx);

                timeInSeconds = getSeconds(hours, minutes, seconds);

                if (timeInSeconds < getSeconds(minValues["SESSLIMIT"]) || timeInSeconds > getSeconds(maxValues["SESSLIMIT"])) {
                    sesslimitMsdTxtBx.Focus();
                    sesslimitMsdTxtBx.SelectAll();
                    throw new ArgumentException("Предельное время сеанса связи не может быть меньше " + minValues["SESSLIMIT"] + " и больше " + maxValues["SESSLIMIT"]);
                }
            }

            //Время удержания сеанса связи при отсутствии обмена данными
            if (minValues.ContainsKey("HOLDTIME")) {

                periodArr = holdTimeMsdTxtBx.Text.Split(':');

                checkFieldTimeFormat(periodArr, holdTimeMsdTxtBx, "Формат времени поля \"Время удержания сеанса связи при отсутствии обмена данными\" должен иметь формат hh:mm:ss");

                hours = Convert.ToInt32(periodArr[0]);
                minutes = Convert.ToInt32(periodArr[1]);
                seconds = Convert.ToInt32(periodArr[2]);

                checkTimeFormat(hours, minutes, seconds, holdTimeMsdTxtBx);

                timeInSeconds = getSeconds(hours, minutes, seconds);

                if (timeInSeconds < getSeconds(minValues["HOLDTIME"]) || timeInSeconds > getSeconds(maxValues["HOLDTIME"])) {
                    holdTimeMsdTxtBx.Focus();
                    holdTimeMsdTxtBx.SelectAll();
                    throw new ArgumentException("Время удержания сеанса связи не может быть меньше " + minValues["HOLDTIME"] + " и больше " + maxValues["HOLDTIME"]);
                }
            }

            //время удержания входящего соединения при отсутствии поступления данных от подключённого устройства (счётчика)
            if (minValues.ContainsKey("INCOMHOLDTIME")) {

                periodArr = incomholdtimeMskTxtBx.Text.Split(':');

                checkFieldTimeFormat(periodArr, incomholdtimeMskTxtBx, "Формат времени поля \"Время удержания входящего соединения при отсутствии поступления данных от счетчика\" должен иметь формат hh:mm:ss");

                hours = Convert.ToInt32(periodArr[0]);
                minutes = Convert.ToInt32(periodArr[1]);
                seconds = Convert.ToInt32(periodArr[2]);

                checkTimeFormat(hours, minutes, seconds, incomholdtimeMskTxtBx);

                timeInSeconds = getSeconds(hours, minutes, seconds);

                if (timeInSeconds < getSeconds(minValues["INCOMHOLDTIME"]) || timeInSeconds > getSeconds(maxValues["INCOMHOLDTIME"])) {
                    incomholdtimeMskTxtBx.Focus();
                    incomholdtimeMskTxtBx.SelectAll();
                    throw new ArgumentException("Время удержания входящего соединения при отсутствии поступления данных от счетчика не может быть меньше " + minValues["INCOMHOLDTIME"] + " и больше " + maxValues["INCOMHOLDTIME"]);
                }
            }
        }

        /// <summary>
        /// Действие при загрузке скрипта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadScript_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Properties.Settings.Default.modemConfig_pathToLoadScript;

            openFileDialog.Filter = "dat (*.dat*)|*.dat*";
            openFileDialog.FilterIndex = 0;

            if (openFileDialog.ShowDialog() == DialogResult.OK) {

                Properties.Settings.Default.Save();

                try {
                    Cursor = Cursors.WaitCursor;

                    ModemConfigScript script;

                    try {
                        using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate)) {
                            script = (ModemConfigScript)new BinaryFormatter().Deserialize(fs);
                        }
                    } catch (InvalidCastException) {
                        throw new InvalidCastException("Выбранный файл не является файлом для конфигурации модема");
                    }

                    string[] scriptLines = script.getScriptData().Split('\n');

                    for (int i = 0; i < scriptLines.Length; i++) {

                        if (!String.IsNullOrEmpty(scriptLines[i])) {

                            switch (scriptLines[i]) {
                                case "Настройка пользовательских серверов": {

                                        i++; //Перехожу к следующей строке со значениями для параметров настройки пользовательских серверов

                                        while (i < scriptLines.Length) {

                                            //Если встречается следующий блок данных
                                            if (scriptLines[i].Equals("Параметры инициализации связи")) {
                                                i--; //Для того чтобы эта строчка попала в switch
                                                break;
                                            }

                                            parseAndSetUserHostOfScript(scriptLines[i]);
                                            i++;
                                        }
                                    }
                                    break;

                                case "Параметры инициализации связи": {

                                        i++; //Перехожу к следующей строке со значениями для параметров инициализации связи

                                        while (i < scriptLines.Length) {

                                            if (scriptLines[i].Equals("Настройка пользовательских серверов")) {
                                                i--; //Для того чтобы эта строчка попала в switch
                                                break;
                                            }

                                            parseAndSetConnectingParameters(scriptLines[i]);
                                            i++;
                                        }
                                    }
                                    break;
                            }
                        }
                    }

                    connectOnCOMterminal = flasMainForm.getStateConnectionOnCOMofTerminalForm();
                    if (connectOnCOMterminal) {
                        flasMainForm.performClickConnOfTerminalForm();
                    }

                    getNbModemPort();
                    checkGPIO_CP2105();

                    if (!serialPort.IsOpen) serialPort.Open();

                    writeCustomServersProperties();

                    checkValidConnectingParameters();

                    writeConnectingParameters();

                    if (!serialPort.IsOpen) serialPort.Close();

                    if (connectOnCOMterminal) flasMainForm.performClickConnOfTerminalForm();

                    refreshInfoBtn.PerformClick();

                    Flasher.successfullyDialog("Параметры из файла успешно записаны в модем", "Запись параметров");

                    Cursor = Cursors.Default;

                } catch (Exception ex) {
                    Cursor = Cursors.Default;
                    Flasher.exceptionDialog(ex.Message);

                    if (serialPort.IsOpen) serialPort.Close();
                    if (connectOnCOMterminal) flasMainForm.performClickConnOfTerminalForm();
                }
            }
        }

        private void parseAndSetUserHostOfScript(string userHostLine) {

            if (String.IsNullOrEmpty(userHostLine)) return;

            string[] userHostParsmsArr = userHostLine.Split(',');

            //Если не по формату
            if (userHostParsmsArr.Length == 4 || userHostParsmsArr.Length == 2) {

                //В зависимости от номера параметров сервера
                switch (Convert.ToInt32(userHostParsmsArr[0])) {
                    //Параметры сервера №0
                    case 0: {
                            //Тип записи адреса (IPv4, IPv6, Доменное имя)
                            switch (userHostParsmsArr[1].Trim().ToLower()) {

                                case "доменное имя": {
                                        domenNameRdBtn_1.Checked = true;
                                        ipDomenNameTxtBx_1.Text = userHostParsmsArr[2].Trim();
                                        portTxtBx_1.Text = userHostParsmsArr[3].Trim();
                                    }
                                    break;

                                case "ipv4": {
                                        IPv4RdBtn_1.Checked = true;
                                        ipDomenNameTxtBx_1.Text = userHostParsmsArr[2].Trim();
                                        portTxtBx_1.Text = userHostParsmsArr[3].Trim();
                                    }
                                    break;

                                case "ipv6": {
                                        IPv6RdBtn_1.Checked = true;
                                        ipDomenNameTxtBx_1.Text = userHostParsmsArr[2].Trim();
                                        portTxtBx_1.Text = userHostParsmsArr[3].Trim();
                                    }
                                    break;

                                case "empty": {
                                        ipDomenNameTxtBx_1.Text = "";
                                        portTxtBx_1.Text = "";
                                    }
                                    break;

                                default: {
                                        throw new ArgumentException("Неверный параметр: " + userHostParsmsArr[1].Trim().ToLower() + ". Проверьте параметры указанные в файле");
                                    }

                            }
                        }
                        break;

                    //Параметры сервера №1
                    case 1: {
                            //Тип записи адреса (IPv4, IPv6, Доменное имя)
                            switch (userHostParsmsArr[1].Trim().ToLower()) {

                                case "доменное имя": {
                                        domenNameRdBtn_2.Checked = true;
                                        ipDomenNameTxtBx_2.Text = userHostParsmsArr[2].Trim();
                                        portTxtBx_2.Text = userHostParsmsArr[3].Trim();
                                    }
                                    break;

                                case "ipv4": {
                                        IPv4RdBtn_2.Checked = true;
                                        ipDomenNameTxtBx_2.Text = userHostParsmsArr[2].Trim();
                                        portTxtBx_2.Text = userHostParsmsArr[3].Trim();
                                    }
                                    break;

                                case "ipv6": {
                                        IPv6RdBtn_2.Checked = true;
                                        ipDomenNameTxtBx_2.Text = userHostParsmsArr[2].Trim();
                                        portTxtBx_2.Text = userHostParsmsArr[3].Trim();
                                    }
                                    break;

                                case "empty": {
                                        ipDomenNameTxtBx_2.Text = "";
                                        portTxtBx_2.Text = "";
                                    }
                                    break;

                                default: {
                                        throw new ArgumentException("Неверный параметр: " + userHostParsmsArr[1].Trim().ToLower() + ". Проверьте параметры указанные в файле");
                                    }
                            }
                        }
                        break;

                    //Параметры сервера №2
                    case 2: {
                            //Тип записи адреса (IPv4, IPv6, Доменное имя)
                            switch (userHostParsmsArr[1].Trim().ToLower()) {

                                case "доменное имя": {
                                        domenNameRdBtn_3.Checked = true;
                                        ipDomenNameTxtBx_3.Text = userHostParsmsArr[2].Trim();
                                        portTxtBx_3.Text = userHostParsmsArr[3].Trim();
                                    }
                                    break;

                                case "ipv4": {
                                        IPv4RdBtn_3.Checked = true;
                                        ipDomenNameTxtBx_3.Text = userHostParsmsArr[2].Trim();
                                        portTxtBx_3.Text = userHostParsmsArr[3].Trim();
                                    }
                                    break;

                                case "ipv6": {
                                        IPv6RdBtn_3.Checked = true;
                                        ipDomenNameTxtBx_3.Text = userHostParsmsArr[2].Trim();
                                        portTxtBx_3.Text = userHostParsmsArr[3].Trim();
                                    }
                                    break;

                                case "empty": {
                                        ipDomenNameTxtBx_3.Text = "";
                                        portTxtBx_3.Text = "";
                                    }
                                    break;

                                default: {
                                        throw new ArgumentException("Неверный параметр: " + userHostParsmsArr[1].Trim().ToLower() + ". Проверьте параметры указанные в файле");
                                    }
                            }
                        }
                        break;

                    default: {
                            throw new ArgumentException("Настройки сервера №" + userHostParsmsArr[0] + " не предусмотрены");
                        }
                }

            } else {
                throw new FormatException("Формат строки не поддерживается.\nЗначение: " + userHostLine);
            }
        }

        private void parseAndSetConnectingParameters(string connectingParameterLine) {

            if (String.IsNullOrEmpty(connectingParameterLine)) return;

            string[] connectingParameterArr = connectingParameterLine.Split('-');

            if (connectingParameterArr.Length != 2)
                throw new ArgumentException("Формат строки не поддерживается.\nЗначение: " + connectingParameterLine);

            switch (connectingParameterArr[0]) {
                /*PERIOD-06:00:00
                SERVICE-20
                LETWAIT-01:00
                TRYLIMIT-3
                SESSLIMIT-00:20
                HOLDTIME-04:16
                INCOMHOLDTIME-03:00*/

                case "PERIOD": {
                        periodMsdTxtBx.Text = connectingParameterArr[1];
                    }
                    break;

                case "SERVICE": {
                        serviceMsdTxtBx.Text = connectingParameterArr[1];
                    }
                    break;

                case "LETWAIT": {
                        letwaitMsdTxtBx.Text = connectingParameterArr[1];
                    }
                    break;

                case "TRYLIMIT": {
                        trylimitMsdTxtBx.Text = connectingParameterArr[1];
                    }
                    break;

                case "SESSLIMIT": {
                        sesslimitMsdTxtBx.Text = connectingParameterArr[1];
                    }
                    break;

                case "HOLDTIME": {
                        holdTimeMsdTxtBx.Text = connectingParameterArr[1];
                    }
                    break;

                case "INCOMHOLDTIME": {
                        incomholdtimeMskTxtBx.Text = connectingParameterArr[1];
                    }
                    break;
            }
        }

        private void setAValidRangeForTheLabelInfoForConnectingParam() {
            Dictionary<string, string> minValues = minValuesForCommandCONNECTING();
            Dictionary<string, string> maxValues = maxValuesForCommandCONNECTING();
            Dictionary<string, string> defaultValues = defaultValuesForCommandCONNECTING();

            foreach (KeyValuePair<string, string> param in defaultValues) {

                switch (param.Key) {

                    case "PERIOD": {
                            periodLabel.Text = "От " + minValues[param.Key] + " до " + maxValues[param.Key] + ". Значение по умолчанию " + param.Value;
                        }
                        break;

                    case "SERVICE": {
                            serviceLabel.Text = "От " + minValues[param.Key] + " до " + maxValues[param.Key] + ". Значение по умолчанию " + param.Value;
                        }
                        break;

                    case "LETWAIT": {
                            letWaitLabel.Text = "От " + minValues[param.Key] + " до " + maxValues[param.Key] + ". Значение по умолчанию " + param.Value;
                        }
                        break;

                    case "TRYLIMIT": {
                            tryLimitLabel.Text = "От " + minValues[param.Key] + " до " + maxValues[param.Key] + ". Значение по умолчанию " + param.Value;
                            trylimitMsdTxtBx.Mask = new string('0', maxValues[param.Key].Length);
                        }
                        break;

                    case "SESSLIMIT": {
                            sessLimitLabel.Text = "От " + minValues[param.Key] + " до " + maxValues[param.Key] + ". Значение по умолчанию " + param.Value;
                        }
                        break;

                    case "HOLDTIME": {
                            holdTimeLabel.Text = "От " + minValues[param.Key] + " до " + maxValues[param.Key] + ". Значение по умолчанию " + param.Value;
                        }
                        break;

                    case "INCOMHOLDTIME": {
                            incomHoldTimeLabel.Text = "От " + minValues[param.Key] + " до " + maxValues[param.Key] + ". Значение по умолчанию " + param.Value;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Получает значения по умолчанию (заводские) команды CONNECTING
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> defaultValuesForCommandCONNECTING() {

            if (!serialPort.IsOpen) serialPort.Open();

            Dictionary<string, string> connectingTimesValues = new Dictionary<string, string>();

            serialPort.WriteLine("CONNECTING ALL=X DEMO");

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line = "";

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();

                    if (line.Contains("Za) CONNECTING") && line.EndsWith("OK")) {
                        //Пример строки
                        //Za) CONNECTING (PERIOD:6:00:00) (SERVICE:20::) (LETWAIT: :1:00) (TRYLIMIT:3) (SESSLIMIT: :20:) (HOLDTIME: :4:16) (INCOMHOLDTIME: :3:00) "Note h:m:s" DEMO OK

                        string[] splitLine = line.Split('(');

                        for (int i = 1; i < splitLine.Length; i++) {

                            //Получаю название параметра 
                            switch (splitLine[i].Substring(0, splitLine[i].IndexOf(':'))) {

                                case "PERIOD": {
                                        //Получаю значение времени периода
                                        string period = splitLine[i].Substring(splitLine[i].IndexOf(':') + 1, splitLine[i].LastIndexOf(')') - 1 - splitLine[i].IndexOf(':'));
                                        connectingTimesValues["PERIOD"] = parseTimeForMskTxtBox(period, false);
                                    }
                                    break;

                                case "SERVICE": {
                                        string service = splitLine[i].Substring(splitLine[i].IndexOf(':') + 1, splitLine[i].LastIndexOf(')') - 1 - splitLine[i].IndexOf(':'));
                                        connectingTimesValues["SERVICE"] = parseTimeForMskTxtBox(service, false);
                                    }
                                    break;

                                case "LETWAIT": {
                                        string letwait = splitLine[i].Substring(splitLine[i].IndexOf(':') + 1, splitLine[i].LastIndexOf(')') - 1 - splitLine[i].IndexOf(':'));
                                        connectingTimesValues["LETWAIT"] = parseTimeForMskTxtBox(letwait, false);
                                    }
                                    break;

                                case "TRYLIMIT": {
                                        string trylimit = splitLine[i].Substring(splitLine[i].IndexOf(':') + 1, splitLine[i].LastIndexOf(')') - 1 - splitLine[i].IndexOf(':'));
                                        connectingTimesValues["TRYLIMIT"] = trylimit;
                                    }
                                    break;

                                case "SESSLIMIT": {
                                        string sesslimit = splitLine[i].Substring(splitLine[i].IndexOf(':') + 1, splitLine[i].LastIndexOf(')') - 1 - splitLine[i].IndexOf(':'));
                                        connectingTimesValues["SESSLIMIT"] = parseTimeForMskTxtBox(sesslimit, false);
                                    }
                                    break;

                                case "HOLDTIME": {
                                        string holdtime = splitLine[i].Substring(splitLine[i].IndexOf(':') + 1, splitLine[i].LastIndexOf(')') - 1 - splitLine[i].IndexOf(':'));
                                        connectingTimesValues["HOLDTIME"] = parseTimeForMskTxtBox(holdtime, false);
                                    }
                                    break;

                                case "INCOMHOLDTIME": {
                                        string incomholdtime = splitLine[i].Substring(splitLine[i].IndexOf(':') + 1, splitLine[i].LastIndexOf(')') - 1 - splitLine[i].IndexOf(':'));
                                        connectingTimesValues["INCOMHOLDTIME"] = parseTimeForMskTxtBox(incomholdtime, false);
                                    }
                                    break;
                            }
                        }

                        if (serialPort.IsOpen) serialPort.Close();
                        return connectingTimesValues;

                    } else if (line.Contains("Za) CONNECTING") && line.Contains("ERROR")) {
                        if (serialPort.IsOpen) serialPort.Close();
                        throw new MKCommandException("Ответ микроконтроллера на команду \"CONNECTING ALL=X DEMO\" ERROR");
                    }
                }
            }

            if (serialPort.IsOpen) serialPort.Close();
            throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
        }

        /// <summary>
        /// Получает максимально допустимые значения параметров команды CONNECTING
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> maxValuesForCommandCONNECTING() {

            Dictionary<string, string> connectingTimesValues = new Dictionary<string, string>();

            if (!serialPort.IsOpen) serialPort.Open();

            List<string> paramsConnecting = getAllSupportParametersCommandConnecting();

            foreach (string param in paramsConnecting) {

                /*Zc) CONNECTING PERIOD=MAX DEMO
                 Za) CONNECTING (PERIOD:20:00:00) "Note h:m:s" DEMO OK*/

                serialPort.WriteLine("CONNECTING " + param + "=MAX DEMO");

                long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
                string line = "";

                bool takeAnswer = false;

                //Пока не вышло время по таймауту
                while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                    //Если данные пришли в порт
                    if (serialPort.BytesToRead != 0) {

                        //Обновляю таймаут
                        endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                        line = serialPort.ReadLine();

                        if (line.Contains("Za) CONNECTING") && line.EndsWith("OK")) {

                            if (param.Equals("TRYLIMIT")) {
                                connectingTimesValues.Add(param, line.Substring(line.IndexOf(':') + 1, line.LastIndexOf(')') - line.IndexOf(':') - 1));

                            } else {
                                connectingTimesValues.Add(param, parseTimeForMskTxtBox(line.Substring(line.IndexOf(':') + 1, line.LastIndexOf(')') - line.IndexOf(':') - 1), false));
                            }
                            takeAnswer = true;
                            break;

                        } else if (line.Contains("Za) CONNECTING") && line.Contains("ERROR")) {
                            if (serialPort.IsOpen) serialPort.Close();
                            throw new MKCommandException("Ответ микроконтроллера на команду \"CONNECTING " + param + "=MAX DEMO\" ERROR");
                        }
                    }
                }

                if (!takeAnswer) {
                    if (serialPort.IsOpen) serialPort.Close();
                    throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
                }
            }

            if (serialPort.IsOpen) serialPort.Close();
            return connectingTimesValues;
        }

        /// <summary>
        /// Получает минимально допустимые значения параметров команды CONNECTING
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> minValuesForCommandCONNECTING() {

            Dictionary<string, string> connectingTimesValues = new Dictionary<string, string>();

            if (!serialPort.IsOpen) serialPort.Open();

            List<string> paramsConnecting = getAllSupportParametersCommandConnecting();

            foreach (string param in paramsConnecting) {

                /*Zc) CONNECTING PERIOD=MAX DEMO
                 Za) CONNECTING (PERIOD:20:00:00) "Note h:m:s" DEMO OK*/

                serialPort.WriteLine("CONNECTING " + param + "=MIN DEMO");

                long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
                string line = "";

                bool takeAnswer = false;

                //Пока не вышло время по таймауту
                while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                    //Если данные пришли в порт
                    if (serialPort.BytesToRead != 0) {

                        //Обновляю таймаут
                        endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                        line = serialPort.ReadLine();

                        if (line.Contains("Za) CONNECTING") && line.EndsWith("OK")) {

                            if (param.Equals("TRYLIMIT")) {
                                connectingTimesValues.Add(param, line.Substring(line.IndexOf(':') + 1, line.LastIndexOf(')') - line.IndexOf(':') - 1));

                            } else {
                                connectingTimesValues.Add(param, parseTimeForMskTxtBox(line.Substring(line.IndexOf(':') + 1, line.LastIndexOf(')') - line.IndexOf(':') - 1), false));
                            }

                            takeAnswer = true;
                            break;

                        } else if (line.Contains("Za) CONNECTING") && line.Contains("ERROR")) {

                            if (serialPort.IsOpen) serialPort.Close();
                            throw new MKCommandException("Ответ микроконтроллера на команду \"CONNECTING " + param + "=MAX DEMO\" ERROR");
                        }
                    }
                }

                if (!takeAnswer) {
                    if (serialPort.IsOpen) serialPort.Close();
                    throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
                }
            }

            if (serialPort.IsOpen) serialPort.Close();

            return connectingTimesValues;
        }

        /// <summary>
        /// Получает все поддерживаемые параметры коменды CONNECTING
        /// </summary>
        /// <returns></returns>
        private List<string> getAllSupportParametersCommandConnecting() {

            List<string> paramList = new List<string>();

            serialPort.WriteLine("CONNECTING ALL=X DEMO");

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line = "";

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();

                    if (line.Contains("Za) CONNECTING") && line.EndsWith("OK")) {
                        //Пример строки
                        //Za) CONNECTING (PERIOD:6:00:00) (SERVICE:20::) (LETWAIT: :1:00) (TRYLIMIT:3) (SESSLIMIT: :20:) (HOLDTIME: :4:16) (INCOMHOLDTIME: :3:00) "Note h:m:s" DEMO OK

                        string[] splitLine = line.Split('(');

                        for (int i = 1; i < splitLine.Length; i++) {
                            paramList.Add(splitLine[i].Substring(0, splitLine[i].IndexOf(':')));
                        }

                        return paramList;

                    } else if (line.Contains("Za) CONNECTING") && line.Contains("ERROR")) {
                        throw new MKCommandException("Ответ микроконтроллера на команду \"CONNECTING ALL=X DEMO\" ERROR");
                    }
                }
            }

            throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
        }

        private void cancelBtn_Click(object sender, EventArgs e) {
            if (serialPort.IsOpen) Close();
            Close();
        }

        private string cmdKeyHexString = "";
        private string cmdKeyTextString = "";
        private void cmdKeyTxtBx_TextChanged(object sender, EventArgs e) {

            if (cmdKeyHexRdBt.Checked) {

                if (!String.IsNullOrEmpty(cmdKeyTxtBx.Text.Trim())) {

                    string byteString = cmdKeyTxtBx.Text.Replace(" ", "");

                    char[] byteStringCharArr = cmdKeyTxtBx.Text.Trim().ToCharArray();

                    try {
                        cmdKeyConvertTxtBx.Text = Encoding.ASCII.GetString(StringToByteArray(byteString));
                        cmdKeyHexString = cmdKeyTxtBx.Text.Trim();

                    } catch (ArgumentOutOfRangeException) { } catch (FormatException) {
                        Flasher.exceptionDialog("Неверный формат ввода данных. Значение в одном октете должно быть в диапазоне от 00 до FF ");
                        cmdKeyTxtBx.Text = new string(byteStringCharArr, 0, byteStringCharArr.Length - 2);
                        cmdKeyTxtBx.Focus();
                        cmdKeyTxtBx.SelectionStart = cmdKeyTxtBx.Text.Length;
                    }
                }

            } else {
                byte[] bytesString = Encoding.UTF8.GetBytes(cmdKeyTxtBx.Text);

                cmdKeyConvertTxtBx.Text = string.Join(" ", bytesString.Select(i => i.ToString("X2")));
                cmdKeyTextString = cmdKeyTxtBx.Text;
            }
        }

        private byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private void cmdKeyRdBt_CheckedChanged(object sender, EventArgs e) {

            if (cmdKeyHexRdBt.Checked) {
                cmdKeyTxtBx.Text = cmdKeyHexString;

            } else {
                cmdKeyTxtBx.Text = cmdKeyTextString;
            }
        }
    }
}