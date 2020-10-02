using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// Модуль Quectel BC92 комбинированный осуществялет выход в сеть
    /// https://www.quectel.com/product/bc92.htm
    /// </summary>
    class BC92_Module : CommunicationModule {

        public BC92_Module() {
            base.name = "BC92";
        }

        CP2105_Connector CP2105_Connector = CP2105_Connector.GetCP2105_ConnectorInstance();

        public string sendATCommand(string command) {

            int port = this.CP2105_Connector.getStandartPort();
        }

    }
}
