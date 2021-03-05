﻿using GSM_NBIoT_Module.classes;
using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSM_NBIoT_Module.view {
    public partial class ModemConfig : Form {
        public ModemConfig() {
            InitializeComponent();
        }

        private SerialPort serialPort = new SerialPort();
        private int timeOut = 3000;

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

            //Установка слушателя маскам
            domenNameRdBtn_1.CheckedChanged += domenNameRdBtns_CheckedChanged;
            domenNameRdBtn_2.CheckedChanged += domenNameRdBtns_CheckedChanged;
            domenNameRdBtn_3.CheckedChanged += domenNameRdBtns_CheckedChanged;

            CultureInfo cultureInfo = new CultureInfo("en", false);
            ipDomenNameTxtBx_1.Culture = cultureInfo;
            ipDomenNameTxtBx_2.Culture = cultureInfo;
            ipDomenNameTxtBx_3.Culture = cultureInfo;

            ipDomenNameTxtBx_1.InsertKeyMode = InsertKeyMode.Overwrite;
            ipDomenNameTxtBx_2.InsertKeyMode = InsertKeyMode.Overwrite;
            ipDomenNameTxtBx_3.InsertKeyMode = InsertKeyMode.Overwrite;

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

            switch(i) {
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

            } catch(Exception ex) {
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
            //Подменное имя и порт сервера 1
            getIPv4AndPortUserHost(0, ref iPorDomen, ref port);
            ipDomenNameTxtBx_1.Text = iPorDomen;
            portTxtBx_1.Text = port;

            if (iPorDomen.StartsWith("\"") && iPorDomen.EndsWith("\"")) {
                domenNameRdBtn_1.Checked = true;
            } else {
                IPv4RdBtn_1.Checked = true;
            }

            //Подменное имя и порт сервера 2
            getIPv4AndPortUserHost(1, ref iPorDomen, ref port);
            ipDomenNameTxtBx_2.Text = iPorDomen;
            portTxtBx_2.Text = port;

            if (iPorDomen.StartsWith("\"") && iPorDomen.EndsWith("\"")) {
                domenNameRdBtn_2.Checked = true;
            } else {
                IPv4RdBtn_2.Checked = true;
            }

            //Подменное имя и порт сервера 3
            getIPv4AndPortUserHost(2, ref iPorDomen, ref port);
            ipDomenNameTxtBx_3.Text = iPorDomen;
            portTxtBx_3.Text = port;

            if (iPorDomen.StartsWith("\"") && iPorDomen.EndsWith("\"")) {
                domenNameRdBtn_3.Checked = true;
            } else {
                IPv4RdBtn_3.Checked = true;
            }
        }

        /// <summary>
        /// Устанавливает пользовательские параметры дополнительных серверов (Вкладка: настройка пользовательских серверов)
        /// </summary>
        private void writeCustomServersProperties() {
            try {
                getNbModemPort();
                checkGPIO_CP2105();

                serialPort.Open();

                checkCustomServerProperties(0, domenNameRdBtn_1, ipDomenNameTxtBx_1, portTxtBx_1);
                checkCustomServerProperties(1, domenNameRdBtn_2, ipDomenNameTxtBx_2, portTxtBx_2);
                checkCustomServerProperties(2, domenNameRdBtn_3, ipDomenNameTxtBx_3, portTxtBx_3);

                serialPort.Close();

            } catch (Exception ex) {
                serialPort.Close();
                Flasher.exceptionDialog(ex.Message);
            }
        }

        private void checkCustomServerProperties(int numbServerProperties, RadioButton domenOrIPv4Check, MaskedTextBox domenOrIPv4Data, MaskedTextBox portData) {

            //Распарсенные данные IP адреса для отправлки в COM порт
            string IPv4Data = "";

            //Если должно поступить на вход доменное имя
            if (domenOrIPv4Check.Checked) {

                char[] domenNameArr = domenOrIPv4Data.Text.ToCharArray();
                //С учётом кавычек
                if (domenNameArr.Length > 28) {
                    domenOrIPv4Data.Focus();
                    domenOrIPv4Data.SelectAll();
                    Flasher.exceptionDialog("Доменное имя не должно быть больше 28 символов");
                    return;
                }

                for (int i = 0; i < domenNameArr.Length; i++) {
                    if (domenNameArr[i] < 0x20 || domenNameArr[i] > 0x7F) {
                        domenOrIPv4Data.Focus();
                        domenOrIPv4Data.SelectAll();
                        Flasher.exceptionDialog("Доменное имя должно содержать только ASCII символы");
                        return;
                    }
                }

                //Если должен поступить на вход IP адрес
            } else {

                //Проверка значений
                string[] ipv4Arr = domenOrIPv4Data.Text.Split('.');
                int emptyDataCounter = 0;

                //заменяю незаполненные поля нулями
                for (int i = 0; i < ipv4Arr.Length; i++) {
                    if (String.IsNullOrEmpty(ipv4Arr[i].Trim())) {
                        ipv4Arr[i] = "0";
                        emptyDataCounter++;
                    }
                }

                //Если все значения ip адреса будут пустыми, это значит, что необходимо переписать данное поле пустым значением
                if (emptyDataCounter == 4) {

                    domenOrIPv4Data.Text = "";

                } else {

                    if (Convert.ToInt32(ipv4Arr[0]) > 255 ||
                        Convert.ToInt32(ipv4Arr[1]) > 255 ||
                        Convert.ToInt32(ipv4Arr[2]) > 255 ||
                        Convert.ToInt32(ipv4Arr[3]) > 255) {

                        domenOrIPv4Data.Focus();
                        domenOrIPv4Data.SelectAll();
                        Flasher.exceptionDialog("Неверный формат записи IPv4 адреса. Значения между точек должны быть в диапазоне 0..255");
                        return;
                    }

                    IPv4Data = ipv4Arr[0].Trim() + "." + ipv4Arr[1].Trim()+ "." + ipv4Arr[2].Trim()+ "." + ipv4Arr[3].Trim();
                }
            }

            if (!String.IsNullOrEmpty(portData.Text)) {
                //Проверка коректности ввода порта
                if (Convert.ToInt32(portData.Text) > 65535 || Convert.ToInt32(portData.Text) < 0) {
                    portData.Focus();
                    portData.SelectAll();
                    Flasher.exceptionDialog("Значение порта должно быть в диапазоне 0..65535");
                    return;
                }
            }

            //Отправка данных в порт
            string dataToPort;

            //Если доменное имя, то добавляются кавычки
            if (domenOrIPv4Check.Checked) {
                dataToPort = "USERHOST N=" + numbServerProperties +
                    " IP=\"" + domenOrIPv4Data.Text + "\"; PORT=" + portData.Text;

                //Если IP адрес, то просто передаётся он
            } else {
                dataToPort = "USERHOST N=" + numbServerProperties +
                    " IP=" + IPv4Data + "; PORT=" + portData.Text;
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
                        domenOrIPv4Data.Focus();
                        domenOrIPv4Data.SelectAll();
                        throw new MKCommandException("Не удалось записать данные");
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
                        string[] arrLine = line.Split(':');

                        char[] iPCharArr = arrLine[2].ToCharArray();

                        //Получаю IP или доменное имя
                        for (int j = 0; j < iPCharArr.Length; j++) {
                            if (iPCharArr[j] != ')') {
                                domenNameOrIP += iPCharArr[j];
                            } else {
                                break;
                            }
                        }

                        char[] arrCharPort = arrLine[3].ToCharArray();
                        for (int k = 0; k < arrCharPort.Length; k++) {
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
                        string holdtime = answer[6].Substring(10, answer[6].Length - 17 - 9);
                        holdTimeMsdTxtBx.Text = parseTimeForMskTxtBox(holdtime, true);
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

            foreach (string answerValue in modemAnswer.Split(':')) {

                if (String.IsNullOrEmpty(answerValue)) {
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
                //Проверка на валидность параметров-----------------------------
                //Период инициализации севнсов связи
                periodArr = periodMsdTxtBx.Text.Split(':');

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
                hours = Convert.ToInt32(serviceMsdTxtBx.Text);

                if (hours > 24 || hours < 12) {
                    serviceMsdTxtBx.Focus();
                    serviceMsdTxtBx.SelectAll();
                    Flasher.exceptionDialog("Периодичность инициации сеансов связи не может быть больше 24-х часови меньше 12-ти часов");
                    return;
                }

                //Время ожидания ответа сервера 
                periodArr = letwaitMsdTxtBx.Text.Split(':');
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
                minutes = Convert.ToInt32(periodArr[0]);
                seconds = Convert.ToInt32(periodArr[1]);

                checkTimeFormat(0, minutes, seconds, holdTimeMsdTxtBx);

                timeInSeconds = getSeconds(0, minutes, seconds);

                if (timeInSeconds > 1005 || timeInSeconds < getSeconds(0, 0, 15)) {
                    holdTimeMsdTxtBx.Focus();
                    holdTimeMsdTxtBx.SelectAll();
                    Flasher.exceptionDialog("Время удержания сеанса связи не может быть больше 16:45 и меньше 00:15");
                    return;
                }

                getNbModemPort();

                serialPort.Open();

                serialPort.WriteLine(
                    "CONNECTING PERIOD=" + periodMsdTxtBx.Text +
                    "; SERVICE=" + serviceMsdTxtBx.Text+":"+":" +
                    "; LETWAIT=" + ":"+letwaitMsdTxtBx.Text +
                    "; TRYLIMIT=" + trylimitMsdTxtBx.Text +
                    "; SESSLIMIT=" + sesslimitMsdTxtBx.Text +":" +
                    "; HOLDTIME=" + ":"+holdTimeMsdTxtBx.Text
                    );

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

                        if (line.EndsWith("OK")) {
                            successfully = true;
                            break;
                        }
                    }
                }

                if (!successfully) {
                    Flasher.exceptionDialog("Не удалось получить подтверждение от модема, что данные записаны, попробуйте снова");
                } else {
                    Flasher.successfullyDialog("Параметры инициализации связи успешно записаны", "Запись параметров");
                }

                serialPort.Close();

            } catch (Exception ex) {
                Flasher.exceptionDialog(ex.Message);

                if (serialPort.IsOpen) {
                    serialPort.Close();
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

        private void connectingAcceptBtn_Click(object sender, EventArgs e) {
            writeConnectingParameters();
        }
        
        /// <summary>
        /// Устанавливаю необходимую маску в зависимости от maskTxtBx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void domenNameRdBtns_CheckedChanged(object sender, EventArgs e) {

            if (sender == domenNameRdBtn_1) {
                if (domenNameRdBtn_1.Checked) {
                    ipDomenNameTxtBx_1.Mask = "";
                } else {
                    ipDomenNameTxtBx_1.Mask = "000.000.000.000";
                }

            } else if (sender == domenNameRdBtn_2) {
                if (domenNameRdBtn_2.Checked) {
                    ipDomenNameTxtBx_2.Mask = "";
                } else {
                    ipDomenNameTxtBx_2.Mask = "000.000.000.000";
                }

            } else if (sender == domenNameRdBtn_3) {
                if (domenNameRdBtn_3.Checked) {
                    ipDomenNameTxtBx_3.Mask = "";
                } else {
                    ipDomenNameTxtBx_3.Mask = "000.000.000.000";
                }
            }
        }

        private void maskedTxtBx_Enter(object sender, EventArgs e) {
            //((MaskedTextBox)sender).Selection
        }

        private void setServersSettingsBtn_Click(object sender, EventArgs e) {
            writeCustomServersProperties();
        }
    }
}