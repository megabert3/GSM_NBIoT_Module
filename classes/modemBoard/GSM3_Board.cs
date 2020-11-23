using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using static GSM_NBIoT_Module.classes.CP2105_Connector;

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

            Flasher.addMessageInMainLog("Поиск портов модема GSM3");

            //Ищу порты устройства
            cp2105.FindDevicePorts();

            int enhadced = cp2105.getEnhabcedPort();
            int standart = cp2105.getStandartPort();

            Flasher.addMessageInMainLog("Enhadced COM: " + enhadced);
            Flasher.addMessageInMainLog("Standart COM: " + standart + Environment.NewLine);
            Flasher.setValuePogressBarFlashingStatic(50);

            cp2105.WriteGPIOStageAndSetFlags(enhadced, true, true, 1000);
            cp2105.WriteGPIOStageAndSetFlags(standart, true, true, true, 1000);

            Flasher.addMessageInMainLog("Считываю состояние ножек GPIO портов");

            //Считываю состояние ножек Enchadced порта
            StateGPIO_OnEnhabcedPort enhad = cp2105.GetStageGPIOEnhabcedPort();

            Flasher.addMessageInMainLog("Состояние ножек добавочного порта");
            Flasher.addMessageInMainLog("GPIO_0 " + enhad.stageGPIO_0);
            Flasher.addMessageInMainLog("GPIO_1 " + enhad.stageGPIO_1 + Environment.NewLine);
            Flasher.setValuePogressBarFlashingStatic(70);

            //Считываю состояние ножек Standart порта
            StateGPIO_OnStandartPort sta = cp2105.GetStageGPIOStandartPort();

            Flasher.addMessageInMainLog("Состояние ножек стандартного порта");
            Flasher.addMessageInMainLog("GPIO_0 " + sta.stageGPIO_0);
            Flasher.addMessageInMainLog("GPIO_1 " + sta.stageGPIO_1);
            Flasher.addMessageInMainLog("GPIO_2 " + sta.stageGPIO_2 + Environment.NewLine);
            Flasher.setValuePogressBarFlashingStatic(90);

            //Если путь для прошивки модуля quectel не пустой, то перепрошиваю модуль
            if (!String.IsNullOrEmpty(pathToFirmware_BC92)) {

                Flasher.addMessageInMainLog("Отключаю контроллер");
                //Заглушаю контроллер GPIO_1 = 0;
                cp2105.WriteGPIOStageAndSetFlags(enhadced, true, false, 100);
                Flasher.addMessageInMainLog("Добавочный порт GPIO_1 = false" + Environment.NewLine);
                Flasher.setValuePogressBarFlashingStatic(100);

                Flasher.addMessageInMainLog("Перезагружаю модуль");
                //Делаю ресет модуля BC92 и не даю уснуть
                cp2105.WriteGPIOStageAndSetFlags(standart, false, true, true, 1000);
                Flasher.addMessageInMainLog("Стандартный порт GPIO_0 = false, GPIO_1 = true, GPIO_2 = true" + Environment.NewLine);
                Flasher.setValuePogressBarFlashingStatic(110);

                cp2105.WriteGPIOStageAndSetFlags(standart, true, true, true, 3000);
                Flasher.addMessageInMainLog("Стандартный порт GPIO_0 = true, GPIO_1 = true, GPIO_2 = true" + Environment.NewLine);
                Flasher.setValuePogressBarFlashingStatic(120);

                Flasher.addMessageInMainLog("Не даю модулю уснуть");
                cp2105.WriteGPIOStageAndSetFlags(standart, true, true, false, 1000);
                Flasher.addMessageInMainLog("Стандартный порт GPIO_0 = true, GPIO_1 = true, GPIO_2 = false" + Environment.NewLine);
                Flasher.setValuePogressBarFlashingStatic(150);

                bc92.reflashModule(pathToFirmware_BC92);
            }


            if (!String.IsNullOrEmpty(pathToFirmware_STM32L412CB)) {

                //Глушу модуль BC92
                Flasher.addMessageInMainLog("Отключаю модуль");
                cp2105.WriteGPIOStageAndSetFlags(standart, false, true, true, 100);
                Flasher.addMessageInMainLog("Станартный порт GPIO_0 = false, GPIO_1 = true, GPIO_2 = true" + Environment.NewLine);
                Flasher.setValuePogressBarFlashingStatic(510);

                //Включаю контроллер
                Flasher.addMessageInMainLog("Включаю контроллер");
                cp2105.WriteGPIOStageAndSetFlags(enhadced, true, true, 100);
                Flasher.addMessageInMainLog("Добавочный порт GPIO_0 = true, GPIO_1 = true" + Environment.NewLine);
                Flasher.setValuePogressBarFlashingStatic(520);

                //Даю команду контроллеру при следующем включении войти в бут
                Flasher.addMessageInMainLog("Даю команду перейти в boot режим");
                cp2105.WriteGPIOStageAndSetFlags(enhadced, false, true, 100);
                Flasher.addMessageInMainLog("Добавочный порт GPIO_0 = false, GPIO_1 = true" + Environment.NewLine);
                Flasher.setValuePogressBarFlashingStatic(530);

                //Выключаю контроллер
                Flasher.addMessageInMainLog("Перезагружаю контроллер");
                cp2105.WriteGPIOStageAndSetFlags(enhadced, false, false, 100);
                Flasher.addMessageInMainLog("Добавочный порт GPIO_0 = false, GPIO_1 = false");
                Flasher.setValuePogressBarFlashingStatic(540);

                //Включаю контроллер
                cp2105.WriteGPIOStageAndSetFlags(enhadced, false, true, 100);
                Flasher.addMessageInMainLog("Добавочный порт GPIO_0 = false, GPIO_1 = true" + Environment.NewLine);
                Flasher.setValuePogressBarFlashingStatic(550);

                //Перепрошивка контроллера =================================================
                //Открываю COM порт
                Flasher.addMessageInMainLog("\n==========================================================================================");
                Flasher.addMessageInMainLog("ПЕРЕПРОШИВКА КОНТРОЛЛЕРА" + Environment.NewLine);
                Flasher.addMessageInMainLog("Открываю порт");
                stm32L412cb.OpenSerialPort(standart, 115200, Parity.Even, 8, StopBits.One);

                Stopwatch stm32FirmwareWriteStart = new Stopwatch();
                stm32FirmwareWriteStart.Start();

                try {
                    try {
                        stm32L412cb.INIT();
                    } catch (MKCommandException) { }

                    Flasher.setValuePogressBarFlashingStatic(555);

                    Flasher.addMessageInMainLog("Удаляю прошлую прошивку" + Environment.NewLine);
                    stm32L412cb.ERASE();
                    Flasher.setValuePogressBarFlashingStatic(570);

                    Flasher.addMessageInMainLog("Начинаю запись" + Environment.NewLine);
                    stm32L412cb.WRITE(pathToFirmware_STM32L412CB);

                    //Выгружаю всё, что было в буфере при прошивке контроллера
                    Flasher.addProgressFlashMKLogInMainLog();

                    Flasher.addMessageInMainLog("Запуск записанной прошивки" + Environment.NewLine);
                    stm32L412cb.GO();
                    Flasher.setValuePogressBarFlashingStatic(990);

                    stm32FirmwareWriteStart.Stop();

                    Flasher.addMessageInMainLog("Время перепрошивки контроллера " + Flasher.parseMlsInMMssMls(stm32FirmwareWriteStart.ElapsedMilliseconds) + Environment.NewLine);

                    stm32L412cb.ClosePort();

                    Thread.Sleep(100);

                } catch (Exception) {
                    Flasher.addProgressFlashMKLogInMainLog();

                    throw;

                } finally {
                    stm32L412cb.ClosePort();
                }

                Flasher.addMessageInMainLog("Устанавливаю значение GPIO стандартного и добавочного портов в исходное состояние");
                cp2105.WriteGPIOStageAndSetFlags(enhadced, true, true, 100);
                Flasher.addMessageInMainLog("Добавочный порт GPIO_0 = true, GPIO_1 = true");
                Flasher.setValuePogressBarFlashingStatic(995);

                cp2105.WriteGPIOStageAndSetFlags(standart, true, true, true, 100);
                Flasher.addMessageInMainLog("Стандартный порт GPIO_0 = true, GPIO_1 = true, GPIO_2 = true" + Environment.NewLine);


                Flasher.addMessageInMainLog("\n==========================================================================================");
                Flasher.addMessageInMainLog("ПЕРЕРОШИВКА МОДЕМА УСПЕШНО ЗАВЕРШЕНА");
                Flasher.setValuePogressBarFlashingStatic(1000);
            }
        }

        public STM32L412CB_Controller getStm32L412cb() {
            return stm32L412cb;
        }
    }
}