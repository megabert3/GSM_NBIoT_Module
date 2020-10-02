using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// Плата модема GSM3 осуществлет передачу информации по каналам GSM, NBIoT
    /// </summary>
    class GSM3_Board : Board {

        private CommunicationModule bc92;
        private Controller stm32L412cb;
        private Connector cp2105;

        public GSM3_Board() {
            base.name = "GSM3";

            bc92 = new BC92_Module();
            stm32L412cb = new STM32L412CB_Controller();
            cp2105 = CP2105_Connector.GetCP2105_ConnectorInstance();
        }
    }
}