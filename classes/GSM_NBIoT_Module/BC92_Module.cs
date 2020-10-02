using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// Модуль Quectel BC92 комбинированный, осуществялет выход в сеть
    /// https://www.quectel.com/product/bc92.htm
    /// </summary>
    class BC92_Module : CommunicationModule {

        public BC92_Module() {
            base.name = "BC92";
        }

        CP2105_Connector CP2105_Connector = CP2105_Connector.GetCP2105_ConnectorInstance();

        public string sendATCommand(string command) {

            string port = "COM" + this.CP2105_Connector.getEnhabcedPort();

            using (SerialPort serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One)) {

                serialPort.Open();

                if (serialPort.IsOpen) {

                } else {
                    throw new COMPortException("Порт занят освободите порт и попробуйте снова");
                }

            }

        }

    }
}
