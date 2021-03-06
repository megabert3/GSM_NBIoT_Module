﻿using GSM_NBIoT_Module.classes.applicationHelper;
using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using GSM_NBIoT_Module.classes.controllerOnBoard.Configuration;
using GSM_NBIoT_Module.view;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
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

        //Конфигурация для модема
        private ConfigurationFW configuration;

        //Пути к прошивкам комплектующих
        private string pathToFirmware_BC92;
        private string pathToFirmware_STM32L412CB;

        public GSM3_Board(string pathToFirmware_BC92, string pathToFirmware_STM32L412CB, ConfigurationFW configuration) {

            name = "GSM3";
            this.pathToFirmware_BC92 = pathToFirmware_BC92;
            this.pathToFirmware_STM32L412CB = pathToFirmware_STM32L412CB;
            this.configuration = configuration;
        }        

        public override void Reflash() {
            Flasher.addMessageInMainLogWithoutTime("==========================================================================================");
            Flasher.addMessageInMainLog("ЗАПИСЫВАЕМЫЕ ДАННЫЕ");
            Flasher.addMessageInMainLogWithoutTime("");
            Flasher.addMessageInMainLog("Название конфигурации: " + configuration.getName());
            Flasher.addMessageInMainLog("Прошивка модуля Quectel: " + configuration.getfwForQuectelName());
            Flasher.addMessageInMainLog("Прошивка микроконтроллера: " + configuration.getFwForMKName());

            Flasher.addMessageInMainLog("Версия программы: " + Properties.Settings.Default.programName + " " + Properties.Settings.Default.version);

            Flasher.addMessageInMainLogWithoutTime("");

            Flasher.addMessageInMainLogWithoutTime("==========================================================================================");

            Flasher.addMessageInMainLog("НАЧАЛО ПЕРЕПРОШИВКИ МОДЕМА" + Environment.NewLine);

            Flasher.addMessageInMainLog("Поиск портов модема GSM3" + Environment.NewLine);

            //Проверка подключен ли только один модем к компьютеру
            cp2105.amountDevicesConnect();

            //Ищу порты устройства
            try {
                cp2105.FindDevicePorts();
            } catch (DeviceNotFoundException) {

                bool answer = Flasher.YesOrNoDialog("Не удалось найти порты модема, задать порты в ручную?", "Порты модема");

                if (answer) { 

                    DialogResult result = Flasher.setupPorts();

                    if (result == DialogResult.Cancel) {
                        throw new DeviceNotFoundException("Не удалось найти модем в списке подключенных устройств");
                    }

                } else {
                    throw;
                }
            }

            int enhanced = cp2105.getEnhancedPort();
            int standard = cp2105.getStandardPort();

            Flasher.addMessageInMainLog("Enhanced COM: " + enhanced);
            Flasher.addMessageInMainLog("Standard COM: " + standard + Environment.NewLine);
            Flasher.setValuePogressBarFlashingStatic(50);

            cp2105.WriteGPIOStageAndSetFlags(enhanced, true, true, 1000, true);
            cp2105.WriteGPIOStageAndSetFlags(standard, true, true, true, 1000, true);

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

            Flasher.addMessageInMainLog("Состояние ножек Enhanced порта");
            Flasher.addMessageInMainLog("GPIO_0 = " + enh_0 + " (BOOT_MCU)");
            Flasher.addMessageInMainLog("GPIO_1 = " + enh_1 + " (RST_MCU)" + Environment.NewLine);
            Flasher.setValuePogressBarFlashingStatic(70);

            //Считываю состояние ножек Standard порта
            StateGPIO_OnStandardPort sta = cp2105.GetStageGPIOStandardPort();

            int sta_0;
            int sta_1;
            int sta_2;

            if (sta.stageGPIO_0) sta_0 = 1;
            else sta_0 = 0;

            if (sta.stageGPIO_1) sta_1 = 1;
            else sta_1 = 0;

            if (sta.stageGPIO_2) sta_2 = 1;
            else sta_2 = 0;

            Flasher.addMessageInMainLog("Состояние ножек Standard порта");
            Flasher.addMessageInMainLog("GPIO_0 = " + sta_0 + " (RST_BC92)");
            Flasher.addMessageInMainLog("GPIO_1 = " + sta_1);
            Flasher.addMessageInMainLog("GPIO_2 = " + sta_2 + " (PSM_EINT)" + Environment.NewLine);
            Flasher.setValuePogressBarFlashingStatic(90);

            //Если путь для прошивки модуля quectel не пустой, то перепрошиваю модуль
            if (!String.IsNullOrEmpty(pathToFirmware_BC92)) {

                Flasher.addMessageInMainLog("Подготовка к перепрошивке модуля Quectel" + Environment.NewLine);

                bc92.setConfiguration(configuration);

                bc92.preparingModuleForFirmware(enhanced, standard);

                bc92.reflashModule(pathToFirmware_BC92);
            }

            Flasher.setValuePogressBarFlashingStatic(500);

            if (!String.IsNullOrEmpty(pathToFirmware_STM32L412CB)) {

                Flasher.addMessageInMainLog("Подготовка к перепрошивке микроконтроллера" + Environment.NewLine);

                //Устанавливаю необходимую конфигурацию
                stm32L412cb.setConfiguration(configuration);

                //Глушу модуль BC92
                Flasher.addMessageInMainLog("Отключение модуля Quectel");
                cp2105.WriteGPIOStageAndSetFlags(standard, false, true, true, 100, true);
                Flasher.addMessageInMainLog("Standard порт GPIO_0 = 0 (RST_BC92)");
                Flasher.addMessageInMainLog("Standard порт GPIO_1 = 1");
                Flasher.addMessageInMainLog("Standard порт GPIO_2 = 1 (PSM_EINT)" + Environment.NewLine);

                Flasher.setValuePogressBarFlashingStatic(510);

                //Включаю контроллер
                Flasher.addMessageInMainLog("Включение микроконтроллера");
                cp2105.WriteGPIOStageAndSetFlags(enhanced, true, true, 100, true);
                Flasher.addMessageInMainLog("Enhanced порт GPIO_0 = 1 (BOOT_MCU)");
                Flasher.addMessageInMainLog("Enhanced порт GPIO_1 = 1 (RST_MCU)" + Environment.NewLine);

                Flasher.setValuePogressBarFlashingStatic(520);

                //Даю команду контроллеру при следующем включении войти в бут
                Flasher.addMessageInMainLog("Передача команды микроконтроллеру перейти в boot режим");
                cp2105.WriteGPIOStageAndSetFlags(enhanced, false, true, 100, true);
                Flasher.addMessageInMainLog("Enhanced порт GPIO_0 = 0 (BOOT_MCU)");
                Flasher.addMessageInMainLog("Enhanced порт GPIO_1 = 1 (RST_MCU)" + Environment.NewLine);

                Flasher.setValuePogressBarFlashingStatic(530);

                //Выключаю контроллер
                Flasher.addMessageInMainLog("Перезагрузка микроконтроллера");
                cp2105.WriteGPIOStageAndSetFlags(enhanced, false, false, 100, true);
                Flasher.addMessageInMainLog("Enhanced порт GPIO_0 = 0 (BOOT_MCU)");
                Flasher.addMessageInMainLog("Enhanced порт GPIO_1 = 0 (RST_MCU)" + Environment.NewLine);

                Flasher.setValuePogressBarFlashingStatic(540);

                //Включаю контроллер
                cp2105.WriteGPIOStageAndSetFlags(enhanced, false, true, 100, true);
                Flasher.addMessageInMainLog("Enhanced порт GPIO_0 = 0 (BOOT_MCU)");
                Flasher.addMessageInMainLog("Enhanced порт GPIO_1 = 1 (RST_MCU)" + Environment.NewLine);
                Flasher.setValuePogressBarFlashingStatic(550);

                //Перепрошивка контроллера =================================================
                //Открываю COM порт
                try {

                    Flasher.addMessageInMainLogWithoutTime("==========================================================================================");
                    Flasher.addMessageInMainLog("ПЕРЕПРОШИВКА МИКРОКОНТРОЛЛЕРА" + Environment.NewLine);
                    Flasher.addMessageInMainLog("Открываю порт");
                    stm32L412cb.OpenSerialPort(enhanced, 115200, Parity.Even, 8, StopBits.One);

                    Stopwatch stm32FirmwareWriteStart = new Stopwatch();
                    stm32FirmwareWriteStart.Start();

                    try {
                        stm32L412cb.INIT();
                    } catch (COMException) {

                        Flasher.addMessageInMainLog("Не удалось получить ответ от микроконтроллера");
                        Flasher.addMessageInMainLog("Повторная попытка");
                        Flasher.addMessageInMainLog("Перзагрузка контроллера");
                        cp2105.WriteGPIOStageAndSetFlags(enhanced, true, true, 100, true);
                        cp2105.WriteGPIOStageAndSetFlags(enhanced, true, false, 100, true);
                        cp2105.WriteGPIOStageAndSetFlags(enhanced, true, true, 100, true);

                        //Даю команду контроллеру при следующем включении войти в бут
                        Flasher.addMessageInMainLog("Передача команды микроконтроллеру перейти в boot режим");
                        cp2105.WriteGPIOStageAndSetFlags(enhanced, false, true, 100, true);

                        //Выключаю контроллер
                        Flasher.addMessageInMainLog("Перезагрузка микроконтроллера");
                        cp2105.WriteGPIOStageAndSetFlags(enhanced, false, false, 100, true);

                        //Включаю контроллер
                        cp2105.WriteGPIOStageAndSetFlags(enhanced, false, true, 100, true);

                    } catch (MKCommandException) {}

                    Flasher.setValuePogressBarFlashingStatic(555);

                    Flasher.addMessageInMainLog("Очистка памяти микроконтроллера" + Environment.NewLine);
                    stm32L412cb.ERASE();
                    Flasher.setValuePogressBarFlashingStatic(570);

                    Flasher.addMessageInMainLog("Начало записи прошивки в микроконтроллер" + Environment.NewLine);
                    stm32L412cb.WRITE(pathToFirmware_STM32L412CB);

                    //Выгружаю всё, что было в буфере при прошивке контроллера
                    Flasher.addProgressFlashMKLogInMainLog();

                    stm32L412cb.ClosePort();
                    Thread.Sleep(300);

                    Flasher.addMessageInMainLog("Запуск записанной прошивки, перезагрузка микроконтроллера" + Environment.NewLine);

                    cp2105.WriteGPIOStageAndSetFlags(enhanced, true, true, 100, true);
                    cp2105.WriteGPIOStageAndSetFlags(enhanced, true, false, 100, true);
                    cp2105.WriteGPIOStageAndSetFlags(enhanced, true, true, 100, true);

                    //stm32L412cb.GO();
                    Flasher.setValuePogressBarFlashingStatic(990);

                    stm32FirmwareWriteStart.Stop();

                    Flasher.addMessageInMainLog("Время перепрошивки микроконтроллера " + Flasher.parseMlsInMMssMls(stm32FirmwareWriteStart.ElapsedMilliseconds) + Environment.NewLine);

                } catch (Exception) {
                    Flasher.addProgressFlashMKLogInMainLog();

                    throw;

                } finally {
                    stm32L412cb.ClosePort();
                    Thread.Sleep(100);
                }
            }

            DateTime dtStartModemWork = DateTime.Now;

            Flasher.addMessageInMainLog("Установка значений GPIO CP2105 Standard и Enhanced портов в исходное состояние");
            cp2105.WriteGPIOStageAndSetFlags(enhanced, true, true, 1000, true);
            Flasher.addMessageInMainLog("Enhanced порт GPIO_0 = 1 (BOOT_MCU)");
            Flasher.addMessageInMainLog("Enhanced порт GPIO_1 = 1 (RST_MCU)" + Environment.NewLine);

            cp2105.WriteGPIOStageAndSetFlags(standard, true, true, true, 300, true);
            Flasher.addMessageInMainLog("Standard порт GPIO_0 = 1 (RST_BC92)");
            Flasher.addMessageInMainLog("Standard порт GPIO_1 = 1");
            Flasher.addMessageInMainLog("Standard порт GPIO_2 = 1 (PSM_EINT)" + Environment.NewLine);

            Flasher.setValuePogressBarFlashingStatic(995);

            Flasher.addMessageInMainLogWithoutTime("==========================================================================================");
            Flasher.addMessageInMainLog("ПЕРЕПРОШИВКА МОДЕМА УСПЕШНО ЗАВЕРШЕНА" + Environment.NewLine);
            Flasher.addMessageInMainLog("Общее время перепрошивки модема " + Flasher.parseMlsInMMssMls(Flasher.firmwareWriteStart.ElapsedMilliseconds) + Environment.NewLine);
            Flasher.setValuePogressBarFlashingStatic(1000);

            Flasher.addMessageInMainLogWithoutTime("==========================================================================================");
            Flasher.addMessageInMainLog("ЗАПИСАННЫЕ ДАННЫЕ");

            if (!String.IsNullOrEmpty(bc92.getOldFrimware()) && !bc92.getOldFrimware().Equals(configuration.getfwForQuectelName())) {
                Flasher.addMessageInMainLog("Прошлая прошивка модуля Quectel: " + bc92.getOldFrimware());
            }

            Flasher.addMessageInMainLog("Прошивка модуля Quectel: " + configuration.getfwForQuectelName());
            Flasher.addMessageInMainLog("Прошивка микроконтроллера: " + configuration.getFwForMKName());
            Flasher.addMessageInMainLog("Название конфигурации: " + configuration.getName() + Environment.NewLine);

            Flasher.addMessageInMainLog("Дата и время выполнения прошивки: " + DateTime.Now + Environment.NewLine);

            if (!String.IsNullOrEmpty(Flasher.confCommandsAndAnswerQuectel.ToString())) {
                Flasher.addMessageInMainLog("Ответ модуля на конфигурационные команды:");

                string[] separator = { Environment.NewLine };
                string[] answArr = Flasher.confCommandsAndAnswerQuectel.ToString().Split(separator, StringSplitOptions.RemoveEmptyEntries);

                foreach (string str in answArr) {
                    Flasher.addMessageInMainLog(str);
                }
            }

            Flasher.confCommandsAndAnswerQuectel.ToString();

        }

        public STM32L412CB_Controller getStm32L412cb() {
            return stm32L412cb;
        }
    }
}