using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

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

        /// <summary>
        /// Время ожидания ответа от модуля Quectel
        /// </summary>
        private long timeOutAnswer = 7;

        //Полученные данные с модуля
        private string dataInCOM_Port = "";

        //Если получена строка завершающаяя передачу данных "ОК", "ERROR"
        private bool answer = false;

        //Версия прошивки модуля Quectel
        private string verFirmware;

        public void reflashModule(string pathToFirmware) {

            //================================== Проверяю актуальность путей ==================================================

            //Директория программы для перепрошивки модуля
            string PathTo_QMulti_DL_CMD_V1_8 = Directory.GetCurrentDirectory() + "\\QMulti_DL_CMD_V1.8";

            //Файл програмы для перепрошивки модуля
            string PathTo_QMulti_DL_CMD_V1_8_EXE = Directory.GetCurrentDirectory() + "\\QMulti_DL_CMD_V1.8\\QMulti_DL_CMD_V1.8.exe";

            if (!File.Exists(pathToFirmware)) throw new FileNotFoundException("Не удалось найти файл прошивки по указанному пути");

            if (!pathToFirmware.EndsWith(".lod")) throw new FormatException("Неверное расширение файла прошивки");

            //Проверяю содержит ли путь русские символы или пробелы
            Regex regex = new Regex("\\s, [А-Яа-я]");
            MatchCollection matches = regex.Matches(pathToFirmware);

            if (matches.Count > 0) throw new FormatException("Путь содержит русские символы или пробелы");

            if (!Directory.Exists(PathTo_QMulti_DL_CMD_V1_8)) throw new DirectoryNotFoundException("Не удалось найти папку с программой QMulti_DL_CMD_V1.8," +
                " проверьте целостность программы или переустановите её и попробуйте снова");
            if (!File.Exists(PathTo_QMulti_DL_CMD_V1_8_EXE)) throw new FileNotFoundException("Не удалось найти файл QMulti_DL_CMD_V1.8.exe" +
                " проверьте целостность программы или переустановите её и попробуйте снова");

            //Текущая прошивка модуля
            string verModFirmware = VerFirmware;

            //=============================== Запускаю приложение для прошивки модуля ===============================================
            using (Process QMulti_DL_CMD_V1_8_Process = new Process()) {

                //Порт дял перепрошивки модема
                int port = cP2105_Connector.getStandartPort();

                // Скорось прошивки
                const int band = 921699;

                ProcessStartInfo QMulti_DL_CMD_V1_8_Process_Info = new ProcessStartInfo();

                //Директория с приложением
                QMulti_DL_CMD_V1_8_Process_Info.WorkingDirectory = PathTo_QMulti_DL_CMD_V1_8;

                //Само приложение
                QMulti_DL_CMD_V1_8_Process_Info.FileName = PathTo_QMulti_DL_CMD_V1_8_EXE;

                QMulti_DL_CMD_V1_8_Process_Info.Arguments = port + " " + band + " " + pathToFirmware;

                QMulti_DL_CMD_V1_8_Process_Info.UseShellExecute = false;
                //QMulti_DL_CMD_V1_8_Process_Info.RedirectStandardOutput = true;
                QMulti_DL_CMD_V1_8_Process_Info.RedirectStandardError = true;

                QMulti_DL_CMD_V1_8_Process.StartInfo = QMulti_DL_CMD_V1_8_Process_Info;

                /*QMulti_DL_CMD_V1_8_Process.OutputDataReceived += new DataReceivedEventHandler((sender, e) => {

                    Console.WriteLine("OutPut " + e.Data);
                    
                });*/

                QMulti_DL_CMD_V1_8_Process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => {

                    Console.WriteLine(e.Data);

                });

                QMulti_DL_CMD_V1_8_Process.Start();

                QMulti_DL_CMD_V1_8_Process.BeginErrorReadLine();
                //QMulti_DL_CMD_V1_8_Process.BeginOutputReadLine();

                QMulti_DL_CMD_V1_8_Process.WaitForExit();
            }

            //Получаю версию прошивки, которая должна быть записана
            string idNewFirmware = Path.GetFileNameWithoutExtension(pathToFirmware);

            //Ставлю в начальное положение ножки CP2105
            cP2105_Connector.WriteGPIOStageAndSetFlags(cP2105_Connector.getStandartPort(), true, true, true, 100);

            //Делаю ресет модуля BC92
            cP2105_Connector.WriteGPIOStageAndSetFlags(cP2105_Connector.getStandartPort(), false, true, false, 6000);
            //Поднимаю модуль BC92 и не даю уснуть
            cP2105_Connector.WriteGPIOStageAndSetFlags(cP2105_Connector.getStandartPort(), true, true, false, 6000);

            //Считываю версию прошивки с модуля и сравниваю её с той, которую необходимо было залить
            if (!idNewFirmware.Equals(getVersionFrimware())) throw new FileLoadException("Не удалось загрузить прошивку в модуль");
        }

        /// <summary>
        /// Послать АТ команды для конфигурации модуля Quectel
        /// Выкидывает TimeOutException, ATCommandException, Ошибки с COM протами
        /// </summary>
        /// <param name="commands"> Команды для отправки/конфигурации модуля</param>
        public void sendATCommands(List<string> commands) {

            //Получаю COM порт для общения с модулем.
            string port = "COM" + this.cP2105_Connector.getEnhabcedPort();

            using (serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One)) {

                serialPort.Open();

                Thread.Sleep(300);

                if (serialPort.IsOpen) {

                    foreach (string command in commands) {

                        serialPort.WriteLine(command.ToUpper() + "\r");

                        dataInCOM_Port = "";
                        answer = false;

                        Thread.Sleep(500);

                        for (int i = 0; i < timeOutAnswer; i++) {
                            //Считываю данные из ком порта
                            dataInCOM_Port += serialPort.ReadExisting();

                            //Если данные содержат упешный ответ приступаю к следующей команде
                            if (dataInCOM_Port.Contains("OK")) {
                                answer = true;
                                break;
                            }

                            if (dataInCOM_Port.Contains("ERROR")) {
                                throw new ATCommandException("Не удалось записать следующую комманду\n:" + dataInCOM_Port);
                            }

                            Thread.Sleep(1000);
                        }

                        if (!answer) {
                            serialPort.Close();
                            throw new TimeoutException("Не удалось получить ответ от модема");
                        }
                    }
                } else {
                    serialPort.Close();
                    throw new ATCommandException("Порт занят освободите порт и попробуйте снова");
                }

                serialPort.Close();
            }
        }

        public void sendATCommands(params string[] commands) {

            //Получаю COM порт для общения с модулем.
            string port = "COM" + this.cP2105_Connector.getEnhabcedPort();

            using (serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One)) {

                serialPort.Open();

                Thread.Sleep(300);

                if (serialPort.IsOpen) {

                    foreach (string command in commands) {

                        serialPort.WriteLine(command.ToUpper() + "\r");

                        dataInCOM_Port = "";
                        answer = false;

                        Thread.Sleep(500);

                        for (int i = 0; i < timeOutAnswer; i++) {
                            //Считываю данные из ком порта
                            dataInCOM_Port = serialPort.ReadExisting();

                            //Если данные содержат упешный ответ приступаю к следующей команде
                            if (dataInCOM_Port.Contains("OK")) {
                                answer = true;
                                break;
                            }

                            if (dataInCOM_Port.Contains("ERROR")) {
                                throw new ATCommandException("Не удалось записать следующую комманду\n:" + dataInCOM_Port);
                            }

                            Thread.Sleep(1000);
                        }

                        if (!answer) {
                            serialPort.Close();
                            throw new TimeoutException("Не удалось получить ответ от модема");
                        }
                    }
                } else {
                    serialPort.Close();
                    throw new ATCommandException("Порт занят освободите порт и попробуйте снова");
                }

                serialPort.Close();
            }
        }

        /// <summary>
        /// Отправляет команду на получение данныx модуля
        /// </summary>
        /// <returns>Возвращает версию прошивки модуля Quectel</returns>
        private string getVersionFrimware() {

            string port = "COM" + this.cP2105_Connector.getEnhabcedPort();

            using (serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One)) {

                serialPort.Open();

                Thread.Sleep(300);

                if (serialPort.IsOpen) {

                    serialPort.WriteLine("ATI" + "\r");

                    string portData = "";
                    answer = false;

                    Thread.Sleep(500);

                    for (int i = 0; i < timeOutAnswer; i++) {

                        portData += serialPort.ReadExisting();

                        if (portData.Contains("OK")) {
                            answer = true;
                            break;
                        }

                        if (portData.Contains("ERROR")) {
                            throw new ATCommandException("Не удалось записать следующую комманду\n:" + portData);
                        }

                        Thread.Sleep(1000);
                    }

                    if (!answer) throw new TimeoutException("Не удалось получить ответ от модема");

                    string[] arrPortData = portData.Split('\r');

                    foreach (string line in arrPortData) {

                        if (line.Contains("Revision")) {
                            int startIndex = line.IndexOf(':') + 1;

                            return line.Substring(startIndex).Trim();
                        }
                    }

                } else {

                    serialPort.Close();
                    throw new ATCommandException("Порт занят освободите порт и попробуйте снова");
                }

                serialPort.Close();
                return null;
            }
        }

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

        public string VerFirmware {
            get {
                verFirmware = getVersionFrimware();
                if (verFirmware != null) return verFirmware;
                else throw new InvalidOperationException("Не удалось получить версию прошивки модуля");
            }
        }
    }
}
