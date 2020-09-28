using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// USB_UART конвертор осуществляет общение с модулем и контроллером на плате модема
    /// https://www.silabs.com/interface/usb-bridges/classic/device.cp2105
    /// </summary>
    class CP2105_Connector : Connector {

        public CP2105_Connector() {
            base.name = "CP2105";
        }

        public override void SendData() {
            throw new NotImplementedException();
        }
    }
}
