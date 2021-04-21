using GSM_NBIoT_Module.classes;
using GSM_NBIoT_Module.classes.applicationHelper;
using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using GSM_NBIoT_Module.classes.modemConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        //Подсказка для вкладки пользовательских настроек подключения у серверам
        private ToolTip toolTipForUserHostParam = new ToolTip();

        //Текст подсказок для полей записи адреса
        private const string domenNametoolTipMess = "Значение доменного имени должно быть помещено в знак \" в начале и в конце." +
                    "\nЗначение доменного имени не должно быть больше 28 символов" +
                    "\nПример записи: \"devices.226.taipit.ru\"" +
                    "\nПри записи полностью пустого значения (без знаков \") параметр сервера полностью удаляется";

        private const string ipV4toolTipMess = "Формат записи IPv4 XXX.XXX.XXX.XXX, где XXX должен иметь диапазон 0..255" +
                    "\nПример записи: 66.254.114.41" +
                    "\nПри записи полностью пустого значения параметр сервера полностью удаляется";

        private const string ipV6toolTipMess = "Формат записи IPv6 XXXX:XXXX:XXXX:XXXX:XXXX:XXXX:XXXX:XXXX, где X должен иметь диапазон 0..F" +
                    "\nПримеры записи:" +
                    "\n2001:0DB0:0000:123A:0000:0000:0000:0030" +
                    "\n2001:DB0:0:123A:0:0:0:30" +
                    "\n2001:DB0:0:123A::30" +
                    "\nПри записи полностью пустого значения параметр сервера полностью удаляется";

        private void ModemConfig_Load(object sender, EventArgs e) {

            //Добавление настраиваемых параметров
            DataGridViewRow row = new DataGridViewRow();
            ZPORTcommandsDataGridView.Rows.Add(row);
            row.Cells[0].Value = "1. Настройка пользовательских серверов";

            row = new DataGridViewRow();
            ZPORTcommandsDataGridView.Rows.Add(row);
            ZPORTcommandsDataGridView.Rows[1].Cells[0].Value = "2. Параметры инициализации связи";

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

            //Установка подсказок полям
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 7000;
            toolTip.InitialDelay = 250;
            toolTip.ReshowDelay = 0;
            toolTip.ShowAlways = true;

            toolTip.SetToolTip(periodMsdTxtBx, "Формат hh:mm:ss\nЗначение по умолчанию 06:00:00");
            toolTip.SetToolTip(serviceMsdTxtBx, "Формат hh\nЗначение по умолчанию 20");
            toolTip.SetToolTip(letwaitMsdTxtBx, "Формат mm:ss\nЗначение по умолчанию 01:00");
            toolTip.SetToolTip(trylimitMsdTxtBx, "Формат (количество)\nЗначение по умолчанию 3");
            toolTip.SetToolTip(sesslimitMsdTxtBx, "Формат hh:mm\nЗначение по умолчанию 00:20");
            toolTip.SetToolTip(holdTimeMsdTxtBx, "Формат mm:ss\nЗначение по умолчанию 04:16");
            toolTip.SetToolTip(incomholdtimeMskTxtBx, "Формат mm:ss\nЗначение по умолчанию 03:00");

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

            //Подсказка для полей с адресом дополнительных серверов
            toolTipForUserHostParam.AutoPopDelay = 15000;
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
            }
        }

        /// <summary>
        /// Обновляет и считывает информацию из модема
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshInfo_Click(object sender, EventArgs e) {
            try {

                Cursor = Cursors.WaitCursor;

                getNbModemPort();
                checkGPIO_CP2105();

                if (!serialPort.IsOpen) serialPort.Open();

                Dictionary<string, string> deviceParams = getParamsDeviceCommand();

                verFWTxtBx.Text = deviceParams["FW"];
                verProtTxtBx.Text = deviceParams["ZPORT"];
                tergetIdTxtBx.Text = deviceParams["TARGET"];
                protocolIdTxtBx.Text = deviceParams["PROT_ID"];
                IndexTxtBx.Text = deviceParams["IDX"];
                copyIDTxtBx.Text = deviceParams["COPY_ID"];
                modemImeiTxtBx.Text = getModemIMEI();

                getCustomSettingsServers();

                getCONNECTINGparametersAndSetInConnectingPanel();

                if (serialPort.IsOpen) serialPort.Close();

                Cursor = Cursors.Default;

            } catch (Exception ex) {

                Flasher.exceptionDialog(ex.Message);
                Cursor = Cursors.Default;
                serialPort.Close();
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

                ipDomenNameTxtBx_1.Text = iPorDomen;
            }

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

                ipDomenNameTxtBx_2.Text = iPorDomen;
            }

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

                ipDomenNameTxtBx_3.Text = iPorDomen;
            }
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

            //Отправка данных в порт
            string dataToPort;

            if (String.IsNullOrEmpty(domenOrIPData.Text) ||
                (domenOrIPData.Text.Length == 2 && (domenOrIPData.Text.ElementAt(0) == '\"' && domenOrIPData.Text.ElementAt(1) == '\"'))) {

                dataToPort = "USERHOST N=" + numbServerProperties + " ALL=X";

                //Иначе записываю все данные
            } else {

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

                            //Эта функция появилась только с версии протокола ZP009
                            //Проверяю, что у модема версия ZP009 и выше
                            if (Convert.ToInt32(verProtTxtBx.Text.Substring(2, 3)) < 9) {
                                domenOrIPData.Focus();
                                domenOrIPData.SelectAll();
                                throw new FormatException("Запись адреса в формате IPv6 возможно только с версии протокола ZP009 и выше, " +
                                    "используйте запись в формате доменного имени или IPv4");
                            }

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
                int portValue;

                if (!int.TryParse(portData.Text, out portValue)) {
                    portData.Focus();
                    portData.SelectAll();
                    throw new ArgumentException("Значение порта дожно быть числовым и находиться в диапазоне 0..65535");
                }

                if (portValue > 65535 || portValue < 0) {
                    portData.Focus();
                    portData.SelectAll();
                    throw new FormatException("Значение порта должно быть в диапазоне 0..65535");
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

                    if (line.EndsWith("OK")) {
                        return;

                    } else if (line.Contains("ERROR")) {
                        domenOrIPData.Focus();
                        domenOrIPData.SelectAll();
                        throw new MKCommandException("Не удалось записать параметры сервера №" + (numbServerProperties));
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

            Dictionary<string, string> paramsDictionary = new Dictionary<string, string>();

            //Пример ответа:
            //Za) DEVICE (ZPORT:ZP009 /MDMMODEL:QBC92) (FW:ZU018 /COPY_ID:27F87014_000B"hex") (TARGET:1 /IDX:3 /PROT_ID:2 /FUN:00"hex") OK
            serialPort.WriteLine("device");

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line = "";

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();

                    if (line.EndsWith("OK")) {
                        string[] arrLine = line.Split(':');

                        paramsDictionary.Add("ZPORT", arrLine[1].Substring(0, 5));
                        paramsDictionary.Add("MDMMODEL", arrLine[2].Substring(0, arrLine[2].IndexOf(')')));
                        paramsDictionary.Add("FW", arrLine[3].Substring(0, 5));
                        paramsDictionary.Add("COPY_ID", arrLine[4].Substring(0, arrLine[4].IndexOf(')')));
                        paramsDictionary.Add("TARGET", arrLine[5].Substring(0, arrLine[5].IndexOf(' ')));
                        paramsDictionary.Add("IDX", arrLine[6].Substring(0, arrLine[6].IndexOf(' ')));
                        paramsDictionary.Add("PROT_ID", arrLine[7].Substring(0, arrLine[7].IndexOf(' ')));
                        paramsDictionary.Add("FUN", arrLine[8].Substring(0, arrLine[8].IndexOf(')')));

                        return paramsDictionary;

                    } else if (line.Contains("ERROR")) {

                        throw new MKCommandException("Ответ микроконтроллера на команду \"device\": " + line);
                    }
                }
            }

            throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
        }

        /// <summary>
        /// Возвращает значение IMEI модема
        /// </summary>
        /// <returns></returns>
        private string getModemIMEI() {
            serialPort.WriteLine("mdmimid");

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line = "";

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();

                    //Пример ответа: MDMIMID (IMEI:) (IMSI:) (ICCID:) OK
                    if (line.EndsWith("OK")) {

                        string[] arrLine = line.Split(':');

                        return arrLine[1].Substring(0, arrLine[1].IndexOf(')'));

                    } else if (line.Contains("ERROR")) {

                        throw new MKCommandException("Ответ микроконтроллера на команду \"mdmimid\": " + line);
                    }
                }
            }

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
            string line = "";

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();

                    if (line.EndsWith("OK")) {

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

                    if (line.EndsWith("OK")) {

                        StringBuilder strBuild = new StringBuilder("");
                        int lastIndex = 0;

                        //Za) CONNECTING (PERIOD:6:00:00) (SERVICE:20::) (LETWAIT: :1:00) (TRYLIMIT:3) (SESSLIMIT: :20:) (HOLDTIME: :4:16) "Note h:m:s" OK
                        string[] answer = line.Split('(');

                        //Добавляю PERIOD
                        string period = answer[1].Substring(7, answer[1].Length - 2 - 7);
                        periodMsdTxtBx.Text = parseTimeForMskTxtBox(period, false);

                        //Добавляю SERVICE
                        string service = answer[2].Substring(8, answer[2].Length - 2 - 8);
                        serviceMsdTxtBx.Text = service;

                        //Добавляю LETWAIT
                        string letwait = answer[3].Substring(9, answer[3].Length - 2 - 9);
                        letwaitMsdTxtBx.Text = parseTimeForMskTxtBox(letwait, true);

                        //Добавляю TRYLIMIT
                        string trylimit = answer[4].Substring(9, 1);
                        trylimitMsdTxtBx.Text = trylimit;

                        //Добавляю SESSLIMIT
                        string sesslimit = answer[5].Substring(11, answer[5].Length - 2 - 10);
                        sesslimitMsdTxtBx.Text = parseTimeForMskTxtBox(sesslimit, false);

                        //Добавляю HOLDTIME
                        if (Convert.ToInt32(verProtTxtBx.Text.Substring(2)) < 9) {
                            string holdtime = answer[6].Substring(10, answer[6].Length - 17 - 9);
                            holdTimeMsdTxtBx.Text = parseTimeForMskTxtBox(holdtime, true);

                            //Отключаю возможность редактирования последней команды, которую не поддерживает данная версия протокола
                            groupBox11.Enabled = false;
                        } else {

                            string holdtime = answer[6].Substring(9, answer[6].Length - 2 - 8);
                            holdTimeMsdTxtBx.Text = parseTimeForMskTxtBox(holdtime, true);

                            holdtime = answer[7].Substring(15, answer[7].Length - 2 - 15 - 15);
                            incomholdtimeMskTxtBx.Text = parseTimeForMskTxtBox(holdtime, true);
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

        private void writeConnectingParameters() {

            //Если версия ZPort'a не известна, то обновляю информацию
            if (String.IsNullOrEmpty(verProtTxtBx.Text)) {
                refreshInfoBtn.PerformClick();

                if (String.IsNullOrEmpty(verProtTxtBx.Text)) return;
            }

            //Формирую сообщение на отправку данных
            string comandInCOM = "CONNECTING PERIOD=" + periodMsdTxtBx.Text +
                "; SERVICE=" + serviceMsdTxtBx.Text + ":" + ":" +
                "; LETWAIT=" + ":" + letwaitMsdTxtBx.Text +
                "; TRYLIMIT=" + trylimitMsdTxtBx.Text +
                "; SESSLIMIT=" + sesslimitMsdTxtBx.Text + ":" +
                "; HOLDTIME=" + ":" + holdTimeMsdTxtBx.Text;

            //Если версия ZPorta больше 8-ой, то добавляется эта команда
            if (Convert.ToInt32(verProtTxtBx.Text.Substring(2)) > 8) {

                //Добавляю команду к сообщению
                comandInCOM += "; INCOMHOLDTIME=" + ":" + incomholdtimeMskTxtBx.Text;
            }

            getNbModemPort();

            if (!serialPort.IsOpen) serialPort.Open();

            serialPort.WriteLine(comandInCOM);

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line = "";
            bool successfully = false;

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();
                }

                if (line.EndsWith("OK")) {
                    successfully = true;
                    break;
                }
            }

            if (!successfully) {
                throw new DeviceError("Не удалось получить подтверждение от модема, что данные записаны, попробуйте снова");
            }

            if (serialPort.IsOpen) serialPort.Close();

            refreshInfoBtn.PerformClick();
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

        /// <summary>
        /// Проверяет формат времени введённый пользователем
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <param name="maskedTextBox"></param>
        private void checkTimeFormat(int hours, int minutes, int seconds, MaskedTextBox maskedTextBox) {
            //Проверка на корректность значений
            if (hours > 24 || minutes > 59 || seconds > 59) {
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
                checkValidConnectingParameters();

                writeConnectingParameters();

                Flasher.successfullyDialog("Параметры инициализации связи успешно записаны", "Запись параметров");

            } catch (Exception ex) {

                if (serialPort.IsOpen) serialPort.Close();
                Flasher.exceptionDialog(ex.Message);
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

                getNbModemPort();
                checkGPIO_CP2105();

                if (!serialPort.IsOpen) serialPort.Open();

                writeCustomServersProperties();

                if (serialPort.IsOpen) serialPort.Close();

                refreshInfoBtn.PerformClick();

                Cursor = Cursors.Default;
                Flasher.successfullyDialog("Настройка пользовательских серверов успешно записаны", "Запись параметров");

            } catch (Exception ex) {
                Cursor = Cursors.Default;
                serialPort.Close();
                Flasher.exceptionDialog(ex.Message);
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
                    checkValidParametersForScrypt();

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

            //Если версия ZPort'a не известна, то обновляю информацию
            if (String.IsNullOrEmpty(verProtTxtBx.Text)) {
                refreshInfoBtn.PerformClick();

                if (String.IsNullOrEmpty(verProtTxtBx.Text)) return;
            }

            //Проверка на валидность параметров
            //Период инициализации сеансов связи
            periodArr = periodMsdTxtBx.Text.Split(':');

            checkFieldTimeFormat(periodArr, periodMsdTxtBx, "Формат времени поля \"Период инициализации севнсов связи\" должен иметь формат hh:mm:ss");

            hours = Convert.ToInt32(periodArr[0]);
            minutes = Convert.ToInt32(periodArr[1]);
            seconds = Convert.ToInt32(periodArr[2]);

            timeInSeconds = getSeconds(hours, minutes, seconds);

            checkTimeFormat(hours, minutes, seconds, periodMsdTxtBx);

            //Проверка на границы
            if (timeInSeconds > getSeconds(20, 0, 0) || timeInSeconds < 60) {
                periodMsdTxtBx.Focus();
                periodMsdTxtBx.SelectAll();
                throw new ArgumentException("Периодичность инициации сеансов связи не может быть меньше 1-ой минуты и больше 20-ти часов");
            }

            //Период инициализации севнсов связи с базовым сервером
            checkFieldTimeFormat(serviceMsdTxtBx.Text.Split(':'), serviceMsdTxtBx, "Формат времени поля \"Периодичность инициации сеансов связи с базовым сервером\" должен иметь формат hh");

            hours = Convert.ToInt32(serviceMsdTxtBx.Text);

            if (hours > 24 || hours < 12) {
                serviceMsdTxtBx.Focus();
                serviceMsdTxtBx.SelectAll();
                throw new ArgumentException("Периодичность инициации сеансов связи не может быть меньше 12-ти часов и больше 24-х часов");
            }

            //Время ожидания ответа сервера
            periodArr = letwaitMsdTxtBx.Text.Split(':');

            checkFieldTimeFormat(periodArr, letwaitMsdTxtBx, "Формат времени поля \"Время ожидания ответа сервера\" должен иметь формат mm:ss");

            minutes = Convert.ToInt32(periodArr[0]);
            seconds = Convert.ToInt32(periodArr[1]);

            checkTimeFormat(0, minutes, seconds, letwaitMsdTxtBx);

            timeInSeconds = getSeconds(0, minutes, seconds);

            if (timeInSeconds > 255 || timeInSeconds < 15) {
                letwaitMsdTxtBx.Focus();
                letwaitMsdTxtBx.SelectAll();
                throw new ArgumentException("Время ожидания ответа сервера не может быть меньше 00:15 и больше 04:15");
            }

            //Количество попыток связи с сервером
            int amoutTry = Convert.ToInt32(trylimitMsdTxtBx.Text);
            if (amoutTry > 6 || amoutTry < 1) {
                trylimitMsdTxtBx.Focus();
                trylimitMsdTxtBx.SelectAll();
                throw new ArgumentException("Количество попыток связи с сервером не может быть меньше 1-й и больше 6-ти");
            }

            //Предельное время сеанса связи
            periodArr = sesslimitMsdTxtBx.Text.Split(':');

            checkFieldTimeFormat(periodArr, sesslimitMsdTxtBx, "Формат времени поля \"Предельное время сеанса связи\" должен иметь формат hh:mm");

            hours = Convert.ToInt32(periodArr[0]);
            minutes = Convert.ToInt32(periodArr[1]);

            checkTimeFormat(hours, minutes, 0, sesslimitMsdTxtBx);

            timeInSeconds = getSeconds(hours, minutes, 0);

            if (timeInSeconds > getSeconds(4, 15, 0) || timeInSeconds < getSeconds(0, 5, 0)) {
                sesslimitMsdTxtBx.Focus();
                sesslimitMsdTxtBx.SelectAll();
                throw new ArgumentException("Предельное время сеанса связи не может быть меньше 00:05 и больше 04:15");

            }

            //Время удержания сеанса связи при отсутствии обмена данными                
            periodArr = holdTimeMsdTxtBx.Text.Split(':');

            checkFieldTimeFormat(periodArr, holdTimeMsdTxtBx, "Формат времени поля \"Время удержания сеанса связи при отсутствии обмена данными\" должен иметь формат mm:ss");

            minutes = Convert.ToInt32(periodArr[0]);
            seconds = Convert.ToInt32(periodArr[1]);

            checkTimeFormat(0, minutes, seconds, holdTimeMsdTxtBx);

            timeInSeconds = getSeconds(0, minutes, seconds);

            if (timeInSeconds > 1005 || timeInSeconds < 15) {
                holdTimeMsdTxtBx.Focus();
                holdTimeMsdTxtBx.SelectAll();
                throw new ArgumentException("Время удержания сеанса связи не может быть меньше 00:15 и больше 16:45");
            }

            //время удержания входящего соединения при отсутствии поступления данных от подключённого устройства (счётчика)
            periodArr = incomholdtimeMskTxtBx.Text.Split(':');

            checkFieldTimeFormat(periodArr, incomholdtimeMskTxtBx, "Формат времени поля \"Время удержания входящего соединения при отсутствии поступления данных от счетчика\" должен иметь формат mm:ss");

            minutes = Convert.ToInt32(periodArr[0]);
            seconds = Convert.ToInt32(periodArr[1]);

            checkTimeFormat(0, minutes, seconds, incomholdtimeMskTxtBx);

            timeInSeconds = getSeconds(0, minutes, seconds);

            if (timeInSeconds > 900 || timeInSeconds < 30) {
                throw new ArgumentException("Время удержания входящего соединения при отсутствии поступления данных от счетчика не может быть меньше 00:30 и больше 15:00");
            }

            //Если версия ZPorta больше 8-ой, то добавляется эта команда
            if (Convert.ToInt32(verProtTxtBx.Text.Substring(2)) > 8) {

                periodArr = incomholdtimeMskTxtBx.Text.Split(':');

                checkFieldTimeFormat(periodArr, incomholdtimeMskTxtBx, "Формат времени поля \"Время удержания входящего соединения при отсутствии поступления данных от счетчика\" должен иметь формат mm:ss");

                minutes = Convert.ToInt32(periodArr[0]);
                seconds = Convert.ToInt32(periodArr[1]);

                checkTimeFormat(0, minutes, seconds, incomholdtimeMskTxtBx);

                timeInSeconds = getSeconds(0, minutes, seconds);

                if (timeInSeconds > 900 || timeInSeconds < 30) {
                    throw new ArgumentException("Время удержания входящего соединения при отсутствии поступления данных от счетчика не может быть меньше 00:30 и больше 15:00");
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

                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate)) {
                        script = (ModemConfigScript) new BinaryFormatter().Deserialize(fs);
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

                    getNbModemPort();
                    checkGPIO_CP2105();

                    if (!serialPort.IsOpen) serialPort.Open();

                    writeCustomServersProperties();

                    checkValidConnectingParameters();

                    writeConnectingParameters();

                    if (!serialPort.IsOpen) serialPort.Close();

                    refreshInfoBtn.PerformClick();

                    Flasher.successfullyDialog("Параметры из файла успешно записаны в модем", "Запись параметров");

                    Cursor = Cursors.Default;

                } catch (Exception ex) {

                    if (serialPort.IsOpen) serialPort.Close();
                    Cursor = Cursors.Default;
                    Flasher.exceptionDialog(ex.Message);
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
                        break;
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
    }
}