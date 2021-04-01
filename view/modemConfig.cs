using GSM_NBIoT_Module.classes;
using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
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

        private void ModemConfig_Load(object sender, EventArgs e) {

            //Добавление настраиваемых параметров
            DataGridViewRow row = new DataGridViewRow();
            ZPORTcommandsDataGridView.Rows.Add(row);
            row.Cells[0].Value = "Настройка пользовательских серверов";

            row = new DataGridViewRow();
            ZPORTcommandsDataGridView.Rows.Add(row);
            ZPORTcommandsDataGridView.Rows[1].Cells[0].Value = "Параметры инициализации связи";

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

            //Установка подсказок полям
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 7000;
            toolTip.InitialDelay = 250;
            toolTip.ReshowDelay = 0;

            toolTip.SetToolTip(periodMsdTxtBx, "Формат hh:mm:ss\nЗначение по умолчанию 06:00:00");
            toolTip.SetToolTip(serviceMsdTxtBx, "Формат hh\nЗначение по умолчанию 20");
            toolTip.SetToolTip(letwaitMsdTxtBx, "Формат mm:ss\nЗначение по умолчанию 01:00");
            toolTip.SetToolTip(trylimitMsdTxtBx, "Формат (количество)\nЗначение по умолчанию 3");
            toolTip.SetToolTip(sesslimitMsdTxtBx, "Формат hh:mm\nЗначение по умолчанию 00:20");
            toolTip.SetToolTip(holdTimeMsdTxtBx, "Формат mm:ss\nЗначение по умолчанию 04:16");
            toolTip.SetToolTip(incomholdtimeMskTxtBx, "Формат mm:ss\nЗначение по умолчанию 03:00");

            refreshInfo.PerformClick();
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

                serialPort.Open();

                verFWTxtBx.Text = getVerFW();
                verProtTxtBx.Text = getVerZport();

                getCustomSettingsServers();

                getCONNECTINGparametersAndSetInConnectingPanel();

                if (serialPort.IsOpen) {
                    serialPort.Close();
                }

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
            //Доменное имя и порт сервера 1
            getIPv4AndPortUserHost(0, ref iPorDomen, ref port);
            portTxtBx_1.Text = port;

            if (iPorDomen.StartsWith("\"") && iPorDomen.EndsWith("\"")) {
                domenNameRdBtn_1.Checked = true;

            } else if (iPorDomen.Contains('.')) {
                IPv4RdBtn_1.Checked = true;

            } else {
                IPv6RdBtn_1.Checked = true;
            }

            ipDomenNameTxtBx_1.Text = iPorDomen;

            //Доменное имя и порт сервера 2
            getIPv4AndPortUserHost(1, ref iPorDomen, ref port);
            portTxtBx_2.Text = port;

            if (iPorDomen.StartsWith("\"") && iPorDomen.EndsWith("\"")) {
                domenNameRdBtn_2.Checked = true;

            } else if (iPorDomen.Contains('.')) {
                IPv4RdBtn_2.Checked = true;

            } else {
                IPv6RdBtn_2.Checked = true;
            }

            ipDomenNameTxtBx_2.Text = iPorDomen;

            //Доменное имя и порт сервера 3
            getIPv4AndPortUserHost(2, ref iPorDomen, ref port);
            portTxtBx_3.Text = port;

            if (iPorDomen.StartsWith("\"") && iPorDomen.EndsWith("\"")) {
                domenNameRdBtn_3.Checked = true;

            } else if (iPorDomen.Contains('.')) {
                IPv4RdBtn_3.Checked = true;

            } else {
                IPv6RdBtn_3.Checked = true;
            }

            ipDomenNameTxtBx_3.Text = iPorDomen;
        }

        /// <summary>
        /// Устанавливает пользовательские параметры дополнительных серверов (Вкладка: настройка пользовательских серверов)
        /// </summary>
        private void writeCustomServersProperties() {
            try {
                Cursor = Cursors.WaitCursor;

                getNbModemPort();
                checkGPIO_CP2105();

                serialPort.Open();
             
                checkCustomServerProperties(0, formatAddresRecord(1), ipDomenNameTxtBx_1, portTxtBx_1);
                checkCustomServerProperties(1, formatAddresRecord(2), ipDomenNameTxtBx_2, portTxtBx_2);
                checkCustomServerProperties(2, formatAddresRecord(3), ipDomenNameTxtBx_3, portTxtBx_3);

                serialPort.Close();

                refreshInfo.PerformClick();

                Cursor = Cursors.Default;
                Flasher.successfullyDialog("Настройка пользовательских серверов успешно записаны", "Запись параметров");

            } catch (Exception ex) {
                Cursor = Cursors.Default;
                serialPort.Close();
                Flasher.exceptionDialog(ex.Message);
            }
        }

        /// <summary>
        /// Возвращает какой радиобтн сейчас зажат в выборе формата записи адреса
        /// </summary>
        /// <param name="i">Для какой группы радиобаттонов</param>
        /// <returns></returns>
        private int formatAddresRecord(int i) {
            switch(i) {
                case 1: {
                        if (domenNameRdBtn_1.Checked) return 0;
                        else if (IPv4RdBtn_1.Checked) return 1;
                        else return 2;

                    } break;

                case 2: {
                        if (domenNameRdBtn_2.Checked) return 0;
                        else if (IPv4RdBtn_2.Checked) return 1;
                        else return 2;

                    } break;

                case 3: {
                        if (domenNameRdBtn_3.Checked) return 0;
                        else if (IPv4RdBtn_3.Checked) return 1;
                        else return 2;

                    }break;
            }

            throw new ArgumentException("Сценария с аргументом i не предусмотрено");
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
        private void checkCustomServerProperties(int numbServerProperties, int domenOrIPv4Check, TextBox domenOrIPData, MaskedTextBox portData) {

            //Отправка данных в порт
            string dataToPort;

            //Если поле IP адреса или доменного имени пустое, то удаляю значения из микроконтроллера
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
                        } break;

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

                        } break;

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

                                string localDomenName = "";

                                string[] ipv6Arr = ipv6.Split(':');

                                if (ipv6Arr.Length != 8) throw new FormatException();

                                foreach (string cell in ipv6Arr) {

                                    string cell_2_Byte = cell.Trim();

                                    if (cell_2_Byte.Length > 4) throw new FormatException();

                                    if (String.IsNullOrEmpty(cell_2_Byte)) {
                                        localDomenName += "0:";
                                        continue;
                                    }

                                    foreach (char ch in cell_2_Byte.ToUpper().ToArray()) {
                                        //Проверка на диапазон значений от 0..f
                                        if (!((ch >= 48 && ch <= 57) || (ch >= 65 && ch <= 70))) throw new FormatException();
                                    }

                                    localDomenName += cell_2_Byte + ":";
                                }

                                //Пример 2001:DB0:0:123A:0:0:0:30
                                domenOrIPData.Text = localDomenName.Substring(0, localDomenName.Length - 1);

                            } catch(Exception) {

                                domenOrIPData.Focus();
                                domenOrIPData.SelectAll();
                                throw new FormatException("Неверный формат записи IPv6, диапазон каждого значения в одном хекстете может быть от 0..F (HEX)" + 
                                    "\nПример записи: 2001:DB0:0:123A::::30");
                            }

                        } break;
                }

                if (String.IsNullOrEmpty(portData.Text) ||
                    (Convert.ToInt32(portData.Text) > 65535 || Convert.ToInt32(portData.Text) < 0)) {

                    //Проверка коректности ввода порта
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
                        throw new MKCommandException("Не удалось записать параметры сервера №" + (numbServerProperties + 1));
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
        /// Получает версию Zporta прошивки
        /// </summary>
        /// <returns></returns>
        private string getVerZport() {
            return sendCommandMKForVerFWAndVerZPort("device zport");
        }

        /// <summary>
        /// Позвращает версию прошивки
        /// </summary>
        /// <returns></returns>
        private string getVerFW() {
            return sendCommandMKForVerFWAndVerZPort("device FW");
        }

        /// <summary>
        /// Получает персию ZPORT'a и версию прошивки
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private string sendCommandMKForVerFWAndVerZPort(string command) {

            serialPort.WriteLine(command);

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
                        return arrLine[1].Substring(0, 5);
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
                        char[] iPCharArr = arrLine[2].ToCharArray();

                        for (int j = 3; j < iPCharArr.Length; j++) {
                            if (iPCharArr[j] != ')') {
                                domenNameOrIP += iPCharArr[j];
                            } else {
                                break;
                            }
                        }

                        //Получаю порт
                        //PORT:8103) OK
                        char[] arrCharPort = arrLine[3].ToCharArray();
                        for (int k = 5; k < arrCharPort.Length; k++) {
                            if (arrCharPort[k] != ')') {
                                port += arrCharPort[k];
                            } else {
                                break;
                            }
                        }

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
            int hours;
            int minutes;
            int seconds;
            int timeInSeconds;
            string[] periodArr;

            try {
                if (String.IsNullOrEmpty(verProtTxtBx.Text)) {
                    refreshInfo.PerformClick();
                }

                //Проверка на валидность параметров-----------------------------
                //Период инициализации севнсов связи
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
                    Flasher.exceptionDialog("Периодичность инициации сеансов связи не может быть больше 20-ти часов и меньше 1-ой минуты");
                    return;
                }

                //Период инициализации севнсов связи с базовым сервером
                checkFieldTimeFormat(serviceMsdTxtBx.Text.Split(':'), serviceMsdTxtBx, "Формат времени поля \"Периодичность инициации сеансов связи с базовым сервером\" должен иметь формат hh");

                hours = Convert.ToInt32(serviceMsdTxtBx.Text);

                if (hours > 24 || hours < 12) {
                    serviceMsdTxtBx.Focus();
                    serviceMsdTxtBx.SelectAll();
                    Flasher.exceptionDialog("Периодичность инициации сеансов связи не может быть больше 24-х часови меньше 12-ти часов");
                    return;
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
                    Flasher.exceptionDialog("Время ожидания ответа сервера не может быть больше 04:15 и меньше 00:15");
                    return;
                }

                //Количество попыток связи с сервером
                int amoutTry = Convert.ToInt32(trylimitMsdTxtBx.Text);
                if (amoutTry > 6 || amoutTry < 1) {
                    trylimitMsdTxtBx.Focus();
                    trylimitMsdTxtBx.SelectAll();
                    Flasher.exceptionDialog("Количество попыток связи с сервером не может быть больше 6-ти и меньше 1-й");
                    return;
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
                    Flasher.exceptionDialog("Предельное время сеанса связи не может быть больше 04:15 и меньше 00:05");
                    return;
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
                    Flasher.exceptionDialog("Время удержания сеанса связи не может быть больше 16:45 и меньше 00:15");
                    return;
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

                    periodArr = incomholdtimeMskTxtBx.Text.Split(':');

                    checkFieldTimeFormat(periodArr, incomholdtimeMskTxtBx, "Формат времени поля \"Время удержания входящего соединения при отсутствии поступления данных от счетчика\" должен иметь формат mm:ss");

                    minutes = Convert.ToInt32(periodArr[0]);
                    seconds = Convert.ToInt32(periodArr[1]);

                    checkTimeFormat(0, minutes, seconds, incomholdtimeMskTxtBx);

                    timeInSeconds = getSeconds(0, minutes, seconds);

                    if (timeInSeconds > 900 || timeInSeconds < 30) {
                        Flasher.exceptionDialog("Время удержания входящего соединения при отсутствии поступления данных от счетчика не может быть больше 15:00 и меньше 00:30");
                        return;
                    }

                    //Добавляю команду к сообщению
                    comandInCOM += "; INCOMHOLDTIME=" + ":" + incomholdtimeMskTxtBx.Text;
                }

                getNbModemPort();

                serialPort.Open();

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
                    Flasher.exceptionDialog("Не удалось получить подтверждение от модема, что данные записаны, попробуйте снова");
                } else {
                    Flasher.successfullyDialog("Параметры инициализации связи успешно записаны", "Запись параметров");
                }

                serialPort.Close();

                refreshInfo.PerformClick();

            } catch (Exception ex) {

                if (serialPort.IsOpen) {
                    serialPort.Close();
                }

                Flasher.exceptionDialog(ex.Message);
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
            writeConnectingParameters();
        }

        /// <summary>
        /// Действие при нажатии на кнопку запись параметров в вкладке: Настройка пользовательских серверов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setServersSettingsBtn_Click(object sender, EventArgs e) {
            writeCustomServersProperties();
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

            } else if (IPv4RdBtn_1.Checked) {
                ipDomenNameTxtBx_1.Text = oldValueIPv4_1;

            } else {
                ipDomenNameTxtBx_1.Text = oldValueIPv6_1;
            }
        }

        private void domenNameRdBtnGroup_2_CheckedChanged(object sender, EventArgs e) {
            if (domenNameRdBtn_2.Checked) {
                ipDomenNameTxtBx_2.Text = oldValueDomenName_2;

            } else if (IPv4RdBtn_2.Checked) {
                ipDomenNameTxtBx_2.Text = oldValueIPv4_2;

            } else {
                ipDomenNameTxtBx_2.Text = oldValueIPv6_2;
            }
        }

        private void domenNameRdBtnGroup_3_CheckedChanged(object sender, EventArgs e) {
            if (domenNameRdBtn_3.Checked) {
                ipDomenNameTxtBx_3.Text = oldValueDomenName_3;

            } else if (IPv4RdBtn_3.Checked) {
                ipDomenNameTxtBx_3.Text = oldValueIPv4_3;

            } else {
                ipDomenNameTxtBx_3.Text = oldValueIPv6_3;
            }
        }
    }
}
