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
    public partial class Terminal : Form {

        CP2105_Connector cP2105 = CP2105_Connector.GetCP2105_ConnectorInstance();

        //COM порт для подключения
        SerialPort serialPort = new SerialPort();
        //Имя ком порта, примерт: COM14
        string COM_PortName;
        //Скорость передачи данных
        int bandRate;
        //Число битов данных в байте
        int dataBits;
        //Чётность
        Parity parity;
        //Стоповые биты
        StopBits stopBits;

        //Если ли сейчас подключение к ком порту
        private bool connect = false;


        List<RadioButton> portBandRadBtnList;
        List<RadioButton> dataBitsRadBtnList;

        public Terminal() {
            InitializeComponent();
        }

        private void Terminal_Load(object sender, EventArgs e) {

            //Устанавливаю слушатель
            foreach (Control radBtn in groupBox2.Controls) {
                radBtn.Click += selectBandRate_Click;
            }

            //Бит данных RadioBtns лист
            dataBitsRadBtnList = new List<RadioButton>() {
                dataBit5rdBtn,
                dataBit6rdBtn,
                dataBit7rdBtn,
                dataBit8rdBtn
            };

            foreach (RadioButton rdBtn in dataBitsRadBtnList) {
                rdBtn.Click += selectDataBist_Click;
            }

            //Бит четности
            parityNoneRdBtn.Click += selectParity_Click;
            parityOddRdBtn.Click += selectParity_Click;
            parityEvenRdBtn.Click += selectParity_Click;
            parityMarkRdBtn.Click += selectParity_Click;
            paritySpaceRdBtn.Click += selectParity_Click;

            stopBit1RdBtn.Click += selectStopBit_Click;
            stopBit1_5RdBtn.Click += selectStopBit_Click;
            stopBit2RdBtn.Click += selectStopBit_Click;

            //Инициализация списка COM портов
            comPortsListCmbBox.Items.AddRange(SerialPort.GetPortNames());

            if (comPortsListCmbBox.Items.Count > 0) comPortsListCmbBox.SelectedIndex = 0;

            //Скорость
            bandRate9600rdBtn.PerformClick();
            //data bit
            dataBit8rdBtn.PerformClick();
            //Parity
            parityNoneRdBtn.PerformClick();
            //StopBit
            stopBit1RdBtn.PerformClick();

            customBandRateTxtBx.Enabled = false;
        }

        /// <summary>
        /// Действие при нажатии кнопки подключится к COM порту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connOrDisCOMBtn_Click(object sender, EventArgs e) {

            //Если нет подключения к ком порту
            if (!connect) {

                if (!String.IsNullOrEmpty(comPortsListCmbBox.Text.Trim())) {

                    try {
                        serialPort.PortName = comPortsListCmbBox.Text.Trim();
                    } catch (ArgumentException) {
                        Flasher.exceptionDialog("Неверный формат имени COM порта");
                        return;
                    }

                    //Если задана пользовательская скорость передачи данных
                    if (customBandRateRdBtn.Checked) {

                        if (String.IsNullOrEmpty(customBandRateTxtBx.Text.Trim())) {
                            Flasher.exceptionDialog("Укажите скорость обмена данными по COM порту");
                            return;

                        } else {

                            try {
                                int bandRateCustom = Convert.ToInt32(customBandRateTxtBx.Text.Trim());

                                try {
                                    serialPort.BaudRate = bandRateCustom;
                                } catch (ArgumentOutOfRangeException) {
                                    Flasher.exceptionDialog("Значение \"скорость передачи данных по COM порту\" находится вне диапазона допустимых значенй");
                                    return;
                                }

                            } catch (FormatException) {
                                Flasher.exceptionDialog("Значение скорости обмена данными по COM порту должно быть численным");
                                return;
                            }
                        }
                    }

                    try {
                        serialPort.Open();
                    } catch (Exception ex) {
                        Flasher.exceptionDialog("Возникла ошибка при открытии COM порта:\n" + ex.Message);
                        return;
                    }

                    connect = true;
                    connectComPortPrgBr.Value = 100;
                    connOrDisCOMBtn.Text = "Disconnect";

                    //Если COM порт уже откры
                } else {

                    try {
                        serialPort.Close();
                    } catch (Exception ex) {
                        Flasher.exceptionDialog("Возникла ошибка при закрытии COM порта:\n" + ex.Message);
                        return;
                    }

                    connect = false;
                    connectComPortPrgBr.Value = 0;
                    connOrDisCOMBtn.Text = "Connect";
                }
            }
        }

        /// <summary>
        /// Обработка действия пользователя при изменения параметра скорости порта портва
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectBandRate_Click(object sender, EventArgs e) {

            //-------------------------------------------------- Изменение скорости передачи данных
            foreach (Control radBtn in groupBox2.Controls) {

                if (radBtn is RadioButton) {
                    if (sender == customBandRateRdBtn) {
                        try {
                            serialPort.Close();
                        } catch (System.IO.IOException ex) {
                            Flasher.exceptionDialog("Возникла ошибка при закрытии COM порта:\n" + ex.Message);
                            return;
                        }

                        customBandRateTxtBx.Enabled = true;
                        connect = false;
                        connectComPortPrgBr.Value = 0;
                        connOrDisCOMBtn.Text = "Connect";

                    } else {

                        int band = Convert.ToInt32(((RadioButton)sender).Text);
                        bandRate = band;
                        serialPort.BaudRate = band;
                        customBandRateTxtBx.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Действие при изменении количества бит
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectDataBist_Click(object sender, EventArgs e) {

            foreach (RadioButton radBtn in dataBitsRadBtnList) {

                if (sender == radBtn) {
                    
                    serialPort.DataBits = Convert.ToInt32(((RadioButton)sender).Text);
                    dataBits = Convert.ToInt32(((RadioButton)sender).Text);
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
    }
}
