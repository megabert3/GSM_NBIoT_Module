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

        private void refreshInfo_Click(object sender, EventArgs e) {
            try {
                getNbModemPort();
                checkGPIO_CP2105();

                serialPort.Open();

                verFWTxtBx.Text = getVerFW();
                verProtTxtBx.Text = getVerZport();

                string iPorDomen = "";
                string port = "";

                getIPv4AndPortUserHost(0, ref iPorDomen, ref port);
                ipDomenNameTxtBx_1.Text = iPorDomen;
                portTxtBx_1.Text = port;

                if (iPorDomen.StartsWith("\"") && iPorDomen.EndsWith("\"")) {
                    domenNameRdBtn_1.Checked = true;
                } else {
                    IPv4RdBtn_1.Checked = true;
                }

                getIPv4AndPortUserHost(1, ref iPorDomen, ref port);
                ipDomenNameTxtBx_2.Text = iPorDomen;
                portTxtBx_2.Text = port;

                if (iPorDomen.StartsWith("\"") && iPorDomen.EndsWith("\"")) {
                    domenNameRdBtn_2.Checked = true;
                } else {
                    IPv4RdBtn_2.Checked = true;
                }

                getIPv4AndPortUserHost(2, ref iPorDomen, ref port);
                ipDomenNameTxtBx_3.Text = iPorDomen;
                portTxtBx_3.Text = port;

                if (iPorDomen.StartsWith("\"") && iPorDomen.EndsWith("\"")) {
                    domenNameRdBtn_3.Checked = true;
                } else {
                    IPv4RdBtn_3.Checked = true;
                }

                if (serialPort.IsOpen) {
                    serialPort.Close();
                }
                
            } catch(Exception ex) {
                Flasher.exceptionDialog(ex.Message);
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
    }
}
