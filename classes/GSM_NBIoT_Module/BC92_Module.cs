using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// Модуль Quectel BC92 комбинированный, осуществялет выход в сеть
    /// https://www.quectel.com/product/bc92.htm
    /// </summary>
    public class BC92_Module : CommunicationModule {

        public BC92_Module() {
            base.name = "BC92";
        }

        CP2105_Connector CP2105_Connector = CP2105_Connector.GetCP2105_ConnectorInstance();

        private static SerialPort serialPort;

        private long timeOutAnswer = 7;

        private string dataInCOM_Port = "";
        private bool answer = false;

        private string verFirmware;

        public void sendATCommands(List<string> commands) {

            string port = "COM" + this.CP2105_Connector.getEnhabcedPort();

            using (serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One)) {

                serialPort.Open();

                if (serialPort.IsOpen) {

                    foreach (string command in commands) {

                        serialPort.WriteLine(command.ToUpper() + "\r");

                        dataInCOM_Port = "";
                        answer = false;

                        Thread.Sleep(500);

                        for (int i = 0; i < timeOutAnswer; i++) {
                            dataInCOM_Port = serialPort.ReadExisting();

                            if (dataInCOM_Port.Contains("OK")) {
                                answer = true;
                                break;
                            }

                            if (dataInCOM_Port.Contains("ERROR")) {
                                throw new ATCommandException("Не удалось записать следующую комманду\n:" + dataInCOM_Port);
                            }

                            Thread.Sleep(1000);
                        }

                        if (!answer) throw new TimeoutException("Не удалось получить ответ от модема");
                    }
                } else {
                    serialPort.Close();
                    throw new ATCommandException("Порт занят освободите порт и попробуйте снова");
                }

                serialPort.Close();
            }
        }

        private string getVersionFrimware() {

            string port = "COM" + this.CP2105_Connector.getEnhabcedPort();

            using (serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One)) {

                serialPort.Open();

                if (serialPort.IsOpen) {

                    serialPort.WriteLine("ATI" + "\r");

                    string portData = "";
                    answer = false;

                    Thread.Sleep(500);

                    for (int i = 0; i < timeOutAnswer; i++) {

                        portData = serialPort.ReadExisting();

                        if (portData.Contains("OK")) {
                            answer = true;
                            break;
                        }

                        if (portData.Contains("ERROR")) {
                            throw new ATCommandException("Не удалось записать следующую комманду\n:" + portData);
                        }

                        Thread.Sleep(1000);
                    }

                    if (!answer) throw new TimeoutException("Не удалось получить ответ от модема");

                    

                } else {
                    serialPort.Close();
                    throw new ATCommandException("Порт занят освободите порт и попробуйте снова");
                }

                serialPort.Close();
            }
        }

        public long TimeOutAnswer {
            get {
                return timeOutAnswer;
            }

            set {
                if (value >= 0) {
                    this.timeOutAnswer = value;
                } else throw new ArgumentException("Значение не должно быть отрицательным");
            }
        }
    }
}
