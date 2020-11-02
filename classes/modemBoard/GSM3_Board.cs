using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// Плата модема GSM3 осуществлет передачу информации по каналам GSM, NBIoT
    /// </summary>
    public class GSM3_Board : Board {

        //Комплектующие
        private BC92_Module bc92 = new BC92_Module();
        private STM32L412CB_Controller stm32L412cb = new STM32L412CB_Controller();
        private CP2105_Connector cp2105 = CP2105_Connector.GetCP2105_ConnectorInstance();

        public GSM3_Board(string pathToFirmware_BC92, string pathToFirmware_STM32L412CB) {

            base.name = "GSM3";
            this.pathToFirmware_BC92 = pathToFirmware_BC92;
            this.pathToFirmware_STM32L412CB = pathToFirmware_STM32L412CB;
        }

        //Пути к прошивкам компонентов
        private string pathToFirmware_BC92;
        private string pathToFirmware_STM32L412CB;

        public override void Reflash() {

            //Ищу порты устройства
            cp2105.FindDevicePorts();

            int enhadced = cp2105.getEnhabcedPort();
            int standart = cp2105.getStandartPort();

            cp2105.GetStageGPIOEnhabcedPort();
            cp2105.GetStageGPIOStandartPort();

            //Заглушаю контроллер GPIO_1 = 0;
            cp2105.WriteGPIOStageAndSetFlags(enhadced, true, false, 100);

            //Делаю ресет модуля BC92 и не даю уснуть
            cp2105.WriteGPIOStageAndSetFlags(standart, false, true, false, 6000);
            cp2105.WriteGPIOStageAndSetFlags(standart, true, true, false, 6000);

            bc92.reflashModule(pathToFirmware_BC92);

            //Глушу модуль BC92
            cp2105.WriteGPIOStageAndSetFlags(standart, false, true, true, 100);

            //Включаю контроллер
            cp2105.WriteGPIOStageAndSetFlags(enhadced, true, true, 100);

            //Даю команду контроллеру при следующем включении войти в бут
            cp2105.WriteGPIOStageAndSetFlags(enhadced, false, true, 100);

            //Выключаю контроллер
            cp2105.WriteGPIOStageAndSetFlags(enhadced, false, false, 100);

            //Включаю контроллер
            cp2105.WriteGPIOStageAndSetFlags(enhadced, false, true, 100);

            //Перепрошивка контроллера =================================================
            //Открываю COM порт
            stm32L412cb.OpenSerialPort(enhadced, 115200, Parity.Even, 8, StopBits.One);

            try {
                stm32L412cb.INIT();

                stm32L412cb.ERASE();

                stm32L412cb.WRITE(pathToFirmware_STM32L412CB);

                stm32L412cb.GO();

            } catch(Exception e) {

                throw;

            } finally {
                stm32L412cb.ClosePort();
            }


        }
    }
}