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

        private BC92_Module bc92;
        private STM32L412CB_Controller stm32L412cb;
        private CP2105_Connector cp2105;

        public GSM3_Board() {
            base.name = "GSM3";

            bc92 = new BC92_Module();
            stm32L412cb = new STM32L412CB_Controller();
            cp2105 = CP2105_Connector.GetCP2105_ConnectorInstance();
        }

        public override void Reflash() {
            //Ищу порты устройства
            cp2105.findDevicePorts();

            int enhadced = cp2105.getEnhabcedPort();
            int standart = cp2105.getStandartPort();
            
            //Заглушаю контроллер GPIO_1 = 0;
            cp2105.WriteGPIOStageAndSetFlags(enhadced, true, false, 100);

            //Делаю ресет модуля BC92 и не даю уснуть
            cp2105.WriteGPIOStageAndSetFlags(standart, false, true, false, 6000);
            cp2105.WriteGPIOStageAndSetFlags(standart, true, true, false, 6000);

            //Поменять ссылку
            bc92.reflashModule(@"C:\Users\a.halimov\Desktop\FW_TaiPit\NBIoT\FW_BC92\BC92_rel5\BC92RBR01A05.lod");
        }
    }
}