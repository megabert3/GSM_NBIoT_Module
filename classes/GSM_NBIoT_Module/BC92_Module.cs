using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using GSM_NBIoT_Module.classes.controllerOnBoard.Configuration;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// Модуль Quectel BC92 комбинированный, осуществялет выход в сеть
    /// https://www.quectel.com/product/bc92.htm
    /// </summary>
    public class BC92_Module : CommunicationModule {

        public BC92_Module() {
            base.name = "BC92";
        }

        CP2105_Connector cP2105_Connector = CP2105_Connector.GetCP2105_ConnectorInstance();

        private static SerialPort serialPort;

        private ConfigurationFW configuration;

        private string oldFrimware = "";

        // Время ожидания ответа от модуля Quectel в секундах
        private long timeOutAnswer = 5000;

        //Полученные данные с модуля
        private string dataInCOM_Port = "";

        //Если получена строка завершающаяя передачу данных ("ОК", "ERROR")
        private bool answer = false;

        //Версия прошивки модуля Quectel
        private string verFirmware;

        public override void reflashModule(string pathToFirmware) {

            //================================== Проверяю актуальность путей ==================================================

            //Директория программы для перепрошивки модуля
            string PathTo_QMulti_DL_CMD_V1_8 = Directory.GetCurrentDirectory() + "\\QMulti_DL_CMD_V1.8";

            //Файл програмы для перепрошивки модуля
            string PathTo_QMulti_DL_CMD_V1_8_EXE = Directory.GetCurrentDirectory() + "\\QMulti_DL_CMD_V1.8\\QMulti_DL_CMD_V1.8.exe";

            if (!Directory.Exists(PathTo_QMulti_DL_CMD_V1_8)) throw new DirectoryNotFoundException("Не удалось найти папку с программой QMulti_DL_CMD_V1.8," +
                " проверьте целостность программы или переустановите её и попробуйте снова");
            if (!File.Exists(PathTo_QMulti_DL_CMD_V1_8_EXE)) throw new FileNotFoundException("Не удалось найти файл QMulti_DL_CMD_V1.8.exe" +
                " проверьте целостность программы или переустановите её и попробуйте снова");

            //Загружаемая прошивка модуля
            string loadFirmware = Path.GetFileNameWithoutExtension(pathToFirmware);

            //Текущая прошивка модуля
            try {
                oldFrimware = VerFirmware;
                Flasher.addMessageInMainLog("Текущая прошивка модуля: " + oldFrimware);

                //Если версия загружаемой прошивки такая же, как уже записанная, то выхожу
                if (verFirmware.Equals(loadFirmware)) {
                    Flasher.addMessageInMainLog("В модуль уже записана необходимая прошивка");

                    Flasher.addMessageInMainLog("\n==========================================================================================");
                    Flasher.addMessageInMainLog("КОНФИГУРАЦИЯ МОДУЛЯ QUECTEL" + Environment.NewLine);

                    //Посылаю конфигурационные команды
                    sendATCommands(configuration.getQuectelCommandList());
                    return;
                }

            } catch (TimeoutException ex) {
                Flasher.addMessageInMainLog("Не удалось получить версию прошивки");
            } catch (InvalidOperationException ex) {
                Flasher.addMessageInMainLog("Не удалось получить версию прошивки");
            }

            Flasher.setValuePogressBarFlashingStatic(180);
            //=============================== Запускаю приложение для прошивки модуля ===============================================
            using (Process QMulti_DL_CMD_V1_8_Process = new Process()) {

                //Порт для перепрошивки модема
                int port = cP2105_Connector.getEnhabcedPort();

                // Скорось прошивки
                const int band = 921600;

                ProcessStartInfo QMulti_DL_CMD_V1_8_Process_Info = new ProcessStartInfo();

                //Скрываю окно приложения
                QMulti_DL_CMD_V1_8_Process_Info.CreateNoWindow = true;

                //Директория с приложением
                QMulti_DL_CMD_V1_8_Process_Info.WorkingDirectory = PathTo_QMulti_DL_CMD_V1_8;

                //Само приложение
                QMulti_DL_CMD_V1_8_Process_Info.FileName = PathTo_QMulti_DL_CMD_V1_8_EXE;
                //Параметры для командной строки
                QMulti_DL_CMD_V1_8_Process_Info.Arguments = port + " " + band + " " + pathToFirmware;

                QMulti_DL_CMD_V1_8_Process_Info.UseShellExecute = false;
                QMulti_DL_CMD_V1_8_Process_Info.RedirectStandardOutput = true;
                QMulti_DL_CMD_V1_8_Process_Info.RedirectStandardError = true;

                QMulti_DL_CMD_V1_8_Process.StartInfo = QMulti_DL_CMD_V1_8_Process_Info;

                QMulti_DL_CMD_V1_8_Process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => {
                    Flasher.addMessageInMainLog(e.Data + Environment.NewLine);
                    int progressBarValue = Flasher.getValueProgressBarFlashingStatic();

                    if (progressBarValue <= 450) {
                        Flasher.setValuePogressBarFlashingStatic(progressBarValue + 1);
                    }
                });

                Flasher.addMessageInMainLog("\n==========================================================================================");
                Flasher.addMessageInMainLog("ПРОШИВКА МОДУЛЯ QUECTEL" + Environment.NewLine);

                Stopwatch quectelFirmwareWriteStart = new Stopwatch();
                quectelFirmwareWriteStart.Start();

                QMulti_DL_CMD_V1_8_Process.Start();

                QMulti_DL_CMD_V1_8_Process.BeginErrorReadLine();

                string output = QMulti_DL_CMD_V1_8_Process.StandardOutput.ReadToEnd();

                QMulti_DL_CMD_V1_8_Process.WaitForExit();

                quectelFirmwareWriteStart.Stop();

                if (output.Contains("Total upgrade time")) {
                    Flasher.addMessageInMainLog("Время прошивки модуля " + Flasher.parseMlsInMMssMls(quectelFirmwareWriteStart.ElapsedMilliseconds) + Environment.NewLine);
                } else {
                    throw new FileLoadException("Не удалось загрузить прошивку в модуль, перезагрузите модем и попробуйте снова");
                }

                if (Flasher.getValueProgressBarFlashingStatic() < 450) Flasher.setValuePogressBarFlashingStatic(450);

                //--------------------------------------------------------- Конфигурация модуля Quectel
                //Ставлю в начальное положение ножки CP2105
                cP2105_Connector.WriteGPIOStageAndSetFlags(cP2105_Connector.getStandardPort(), true, true, true, 500);
                Flasher.setValuePogressBarFlashingStatic(460);

                //Делаю ресет модуля BC92
                cP2105_Connector.WriteGPIOStageAndSetFlags(cP2105_Connector.getStandardPort(), false, true, true, 1000);
                Flasher.setValuePogressBarFlashingStatic(465);

                //Поднимаю модуль BC92 и не даю уснуть
                cP2105_Connector.WriteGPIOStageAndSetFlags(cP2105_Connector.getStandardPort(), true, true, true, 1000);
                Flasher.setValuePogressBarFlashingStatic(470);

                cP2105_Connector.WriteGPIOStageAndSetFlags(cP2105_Connector.getStandardPort(), true, true, false, 1000);
                Flasher.setValuePogressBarFlashingStatic(475);

                Flasher.addMessageInMainLog("\n==========================================================================================");
                Flasher.addMessageInMainLog("КОНФИГУРАЦИЯ МОДУЛЯ QUECTEL" + Environment.NewLine);
                //Посылаю конфигурационные команды
                sendATCommands(configuration.getQuectelCommandList());
            }
        }

        /// <summary>
        /// Послать АТ команды для конфигурации модуля Quectel 
        /// Выкидывает TimeOutException, ATCommandException, Ошибки с COM протами
        /// </summary>
        /// <param name="commands"> Команды для отправки/конфигурации модуля</param>
        public void sendATCommands(List<string> commands) {

            //Получаю COM порт для общения с модулем.
            string port = "COM" + this.cP2105_Connector.getStandardPort();

            using (serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One)) {

                serialPort.Open();

                if (serialPort.IsOpen) {

                    foreach (string command in commands) {

                        serialPort.DiscardInBuffer();
                        serialPort.DiscardOutBuffer();

                        Thread.Sleep(30);

                        Flasher.addMessageInMainLog("");
                        Flasher.addMessageInMainLog("Отправка команды: " + command);

                        serialPort.WriteLine(command + "\r\n");

                        dataInCOM_Port = "";
                        answer = false;

                        long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOutAnswer;

                        while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                            //Если есть данные для считывания обнуляю таймаут
                            if (serialPort.BytesToRead != 0) {
                                endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOutAnswer;
                            }

                            //Считываю данные из ком порта
                            dataInCOM_Port += serialPort.ReadExisting();

                            //Если данные содержат упешный ответ приступаю к следующей команде
                            if (dataInCOM_Port.Contains("OK")) {
                                answer = true;
                                break;
                            }

                            if (dataInCOM_Port.Contains("ERROR")) {
                                throw new ATCommandException("Не удалось записать команду: " + command + ". Ответ модуля Quectel ERROR");
                            }
                        }

                        if (!answer) {
                            serialPort.Close();
                            throw new TimeoutException("Не удалось получить ответ от модуля Quectel");
                        }

                        string[] answArr = dataInCOM_Port.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                        Flasher.addMessageInMainLog("Ответ:");

                        for (int i = 1; i < answArr.Length; i++) {
                            Flasher.addMessageInMainLog(answArr[i]);
                        }

                        Flasher.addMessageInMainLog(Environment.NewLine);

                        if (Flasher.getValueProgressBarFlashingStatic() < 500) {
                            Flasher.setValuePogressBarFlashingStatic(Flasher.getValueProgressBarFlashingStatic() + 2);
                        }
                    }

                } else {
                    serialPort.Close();
                    throw new ATCommandException("Порт " + port + " занят, освободите порт и попробуйте снова");
                }

                serialPort.Close();
            }
        }

        /// <summary>
        /// Отправляет команду на получение данныx модуля и возвращает версию прошивки
        /// </summary>
        /// <returns>Возвращает версию прошивки модуля Quectel</returns>
        private string getVersionFrimware() {

            string port = "COM" + this.cP2105_Connector.getStandardPort();

            using (serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One)) {

                serialPort.Open();

                if (serialPort.IsOpen) {

                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();

                    Thread.Sleep(30);

                    serialPort.WriteLine("ATI" + "\r\n");

                    string portData = "";
                    answer = false;

                    long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOutAnswer;

                    while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                        //Если есть данные для считывания обнуляю таймаут
                        if (serialPort.BytesToRead != 0) {
                            endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOutAnswer;
                        }

                        portData += serialPort.ReadExisting();

                        if (portData.Contains("OK")) {
                            answer = true;
                            break;
                        }
                    }

                    if (!answer) throw new TimeoutException("Не удалось получить ответ от модуля Quectel");

                    string[] arrPortData = portData.Split('\r');

                    foreach (string line in arrPortData) {

                        if (line.Contains("Revision")) {
                            int startIndex = line.IndexOf(':') + 1;

                            return line.Substring(startIndex).Trim();
                        }
                    }

                } else {

                    serialPort.Close();
                    throw new ATCommandException("Порт " + port + " занят, освободите порт и попробуйте снова");
                }

                serialPort.Close();
                return null;
            }
        }

        /// <summary>
        /// Выставляет таймаут ответа модуля
        /// </summary>
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

        /// <summary>
        /// Получает версию прошивки
        /// </summary>
        public string VerFirmware {
            get {
                verFirmware = getVersionFrimware();
                if (verFirmware != null) return verFirmware;
                else throw new InvalidOperationException("Не удалось получить версию прошивки модуля");
            }
        }

        public void setConfiguration(ConfigurationFW configuration) {
            this.configuration = configuration;
        }

        public string getOldFrimware() {
            return oldFrimware;
        }
    }
}
