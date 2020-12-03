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

        //Пути к прошивкам комплектующих
        private string pathToFirmware_BC92;
        private string pathToFirmware_STM32L412CB;

        public override void Reflash() {

            Flasher.addMessageInMainLog("Поиск портов модема GSM3" + Environment.NewLine);

            //Проверка подключен ли только один модем к компьютеру
            cp2105.amountDevicesConnect();

            //Ищу порты устройства
            cp2105.FindDevicePorts();

            int enhadced = cp2105.getEnhabcedPort();
            int standart = cp2105.getStandartPort();

            Flasher.addMessageInMainLog("Enhadced COM: " + enhadced);
            Flasher.addMessageInMainLog("Standart COM: " + standart + Environment.NewLine);
            Flasher.setValuePogressBarFlashingStatic(50);

            cp2105.WriteGPIOStageAndSetFlags(enhadced, true, true, 1000);
            cp2105.WriteGPIOStageAndSetFlags(standart, true, true, true, 1000);

            Flasher.addMessageInMainLog("Считывание состояния ножек GPIO CP2105");

            //Считываю состояние ножек Enchadced порта
            StateGPIO_OnEnhabcedPort enhad = cp2105.GetStageGPIOEnhabcedPort();

            //Переменные для более корректного отображения состояния ножек
            int enh_0;
            int enh_1;

            if (enhad.stageGPIO_0) enh_0 = 1;
            else enh_0 = 0;

            if (enhad.stageGPIO_1) enh_1 = 1;
            else enh_1 = 0;

            if (enhad.stageGPIO_1) 
            Flasher.addMessageInMainLog("Состояние ножек Enhadced порта");
            Flasher.addMessageInMainLog("GPIO_0 = " + enh_0);
            Flasher.addMessageInMainLog("GPIO_1 = " + enh_1 + Environment.NewLine);
            Flasher.setValuePogressBarFlashingStatic(70);

            //Считываю состояние ножек Standart порта
            StateGPIO_OnStandartPort sta = cp2105.GetStageGPIOStandartPort();

            int sta_0;
            int sta_1;
            int sta_2;

            if (sta.stageGPIO_0) sta_0 = 1;
            else sta_0 = 0;

            if (sta.stageGPIO_1) sta_1 = 1;
            else sta_1 = 0;

            if (sta.stageGPIO_2) sta_2 = 1;
            else sta_2 = 0;

            Flasher.addMessageInMainLog("Состояние ножек Standart порта");
            Flasher.addMessageInMainLog("GPIO_0 = " + sta_0);
            Flasher.addMessageInMainLog("GPIO_1 = " + sta_1);
            Flasher.addMessageInMainLog("GPIO_2 = " + sta_2 + Environment.NewLine);
            Flasher.setValuePogressBarFlashingStatic(90);

            //Если путь для прошивки модуля quectel не пустой, то перепрошиваю модуль
            if (!String.IsNullOrEmpty(pathToFirmware_BC92)) {

                Flasher.addMessageInMainLog("Подготовка к перепрошивке модуля Quectel" + Environment.NewLine);

                Flasher.addMessageInMainLog("Отключение микроконтроллера");
                //Заглушаю контроллер GPIO_1 = 0;
                cp2105.WriteGPIOStageAndSetFlags(enhadced, true, false, 100);
                Flasher.addMessageInMainLog("Enhadced порт GPIO_0 = 1");
                Flasher.addMessageInMainLog("Enhadced порт GPIO_1 = 0" + Environment.NewLine);
                Flasher.setValuePogressBarFlashingStatic(100);

                Flasher.addMessageInMainLog("Перезагрузка модуля Quectel");
                //Делаю ресет модуля BC92 и не даю уснуть
                cp2105.WriteGPIOStageAndSetFlags(standart, false, true, true, 1000);
                Flasher.addMessageInMainLog("Standart порт GPIO_0 = 0");
                Flasher.addMessageInMainLog("Standart порт GPIO_1 = 1");
                Flasher.addMessageInMainLog("Standart порт GPIO_2 = 1" + Environment.NewLine);

                Flasher.setValuePogressBarFlashingStatic(110);

                cp2105.WriteGPIOStageAndSetFlags(standart, true, true, true, 1000);
                Flasher.addMessageInMainLog("Standart порт GPIO_0 = 1");
                Flasher.addMessageInMainLog("Standart порт GPIO_1 = 1");
                Flasher.addMessageInMainLog("Standart порт GPIO_2 = 1" + Environment.NewLine);

                Flasher.setValuePogressBarFlashingStatic(120);

                Flasher.addMessageInMainLog("Вывод модуля Quectel из режима сна");
                cp2105.WriteGPIOStageAndSetFlags(standart, true, true, false, 1000);
                Flasher.addMessageInMainLog("Standart порт GPIO_0 = 1");
                Flasher.addMessageInMainLog("Standart порт GPIO_1 = 1");
                Flasher.addMessageInMainLog("Standart порт GPIO_2 = 0" + Environment.NewLine);

                Flasher.setValuePogressBarFlashingStatic(150);

                bc92.reflashModule(pathToFirmware_BC92);
            }


            if (!String.IsNullOrEmpty(pathToFirmware_STM32L412CB)) {

                Flasher.addMessageInMainLog("Подготовка к перепрошивке микроконтроллера" + Environment.NewLine);

                //Глушу модуль BC92
                Flasher.addMessageInMainLog("Отключение модуля Quectel");
                cp2105.WriteGPIOStageAndSetFlags(standart, false, true, true, 100);
                Flasher.addMessageInMainLog("Standart порт GPIO_0 = 0");
                Flasher.addMessageInMainLog("Standart порт GPIO_1 = 1");
                Flasher.addMessageInMainLog("Standart порт GPIO_2 = 1" + Environment.NewLine);

                Flasher.setValuePogressBarFlashingStatic(510);

                //Включаю контроллер
                Flasher.addMessageInMainLog("Включение микроконтроллера");
                cp2105.WriteGPIOStageAndSetFlags(enhadced, true, true, 100);
                Flasher.addMessageInMainLog("Enhadced порт GPIO_0 = 1");
                Flasher.addMessageInMainLog("Enhadced порт GPIO_1 = 1" + Environment.NewLine);

                Flasher.setValuePogressBarFlashingStatic(520);

                //Даю команду контроллеру при следующем включении войти в бут
                Flasher.addMessageInMainLog("Передача команды микроконтроллеру перейти в boot режим");
                cp2105.WriteGPIOStageAndSetFlags(enhadced, false, true, 100);
                Flasher.addMessageInMainLog("Enhadced порт GPIO_0 = 0");
                Flasher.addMessageInMainLog("Enhadced порт GPIO_1 = 1" + Environment.NewLine);

                Flasher.setValuePogressBarFlashingStatic(530);

                //Выключаю контроллер
                Flasher.addMessageInMainLog("Перезагрузка микроконтроллера");
                cp2105.WriteGPIOStageAndSetFlags(enhadced, false, false, 100);
                Flasher.addMessageInMainLog("Enhadced порт GPIO_0 = 0");
                Flasher.addMessageInMainLog("Enhadced порт GPIO_1 = 0" + Environment.NewLine);

                Flasher.setValuePogressBarFlashingStatic(540);

                //Включаю контроллер
                cp2105.WriteGPIOStageAndSetFlags(enhadced, false, true, 100);
                Flasher.addMessageInMainLog("Enhadced порт GPIO_0 = 0");
                Flasher.addMessageInMainLog("Enhadced порт GPIO_1 = 1" + Environment.NewLine);
                Flasher.setValuePogressBarFlashingStatic(550);

                //Перепрошивка контроллера =================================================
                //Открываю COM порт
                try {

                    Flasher.addMessageInMainLog("\n==========================================================================================");
                    Flasher.addMessageInMainLog("ПЕРЕПРОШИВКА МИКРОКОНТРОЛЛЕРА" + Environment.NewLine);
                    Flasher.addMessageInMainLog("Открываю порт");
                    stm32L412cb.OpenSerialPort(standart, 115200, Parity.Even, 8, StopBits.One);

                    Stopwatch stm32FirmwareWriteStart = new Stopwatch();
                    stm32FirmwareWriteStart.Start();

                    try {
                        stm32L412cb.INIT();
                    } catch (MKCommandException) { }

                    Flasher.setValuePogressBarFlashingStatic(555);

                    Flasher.addMessageInMainLog("Отчистка памяти микроконтроллера" + Environment.NewLine);
                    stm32L412cb.ERASE();
                    Flasher.setValuePogressBarFlashingStatic(570);

                    Flasher.addMessageInMainLog("Начало записи прошивки в микрокнотроллер" + Environment.NewLine);
                    stm32L412cb.WRITE(pathToFirmware_STM32L412CB);

                    //Выгружаю всё, что было в буфере при прошивке контроллера
                    Flasher.addProgressFlashMKLogInMainLog();

                    Flasher.addMessageInMainLog("Запуск записанной прошивки, перезагрузка микроконтроллера" + Environment.NewLine);
                    
                    cp2105.WriteGPIOStageAndSetFlags(enhadced, true, true, 100);
                    cp2105.WriteGPIOStageAndSetFlags(enhadced, true, false, 100);
                    cp2105.WriteGPIOStageAndSetFlags(enhadced, true, true, 100);

                    //stm32L412cb.GO();
                    Flasher.setValuePogressBarFlashingStatic(990);

                    stm32FirmwareWriteStart.Stop();

                    Flasher.addMessageInMainLog("Время перепрошивки микроконтроллера " + Flasher.parseMlsInMMssMls(stm32FirmwareWriteStart.ElapsedMilliseconds) + Environment.NewLine);

                    stm32L412cb.ClosePort();

                    Thread.Sleep(100);

                } catch (Exception) {
                    Flasher.addProgressFlashMKLogInMainLog();

                    throw;

                } finally {
                    stm32L412cb.ClosePort();
                }

                Flasher.addMessageInMainLog("Установка значений GPIO CP2105 standart и enhadced портов в исходное состояние");
                cp2105.WriteGPIOStageAndSetFlags(enhadced, true, true, 100);
                Flasher.addMessageInMainLog("Enhadced порт GPIO_0 = 1");
                Flasher.addMessageInMainLog("Enhadced порт GPIO_1 = 1" + Environment.NewLine);
                Flasher.setValuePogressBarFlashingStatic(995);

                cp2105.WriteGPIOStageAndSetFlags(standart, true, true, true, 100);
                Flasher.addMessageInMainLog("Standart порт GPIO_0 = 1");
                Flasher.addMessageInMainLog("Standart порт GPIO_1 = 1");
                Flasher.addMessageInMainLog("Standart порт GPIO_2 = 1" + Environment.NewLine);


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