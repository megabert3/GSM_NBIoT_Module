using GSM_NBIoT_Module.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

            serialPort.WriteLine("userhost n=" + i);

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
                        string letwait = answer[3].Substring(9, answer[3].Length - 2 - 8);
                        letwaitMsdTxtBx.Text = parseTimeForMskTxtBox(letwait, true);

                        //Добавляю TRYLIMIT
                        string trylimit = answer[4].Substring(9, 1);
                        trylimitMsdTxtBx.Text = trylimit;

                        //Добавляю SESSLIMIT
                        string sesslimit = answer[5].Substring(10, answer[5].Length - 2 - 10);
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
    }
}
