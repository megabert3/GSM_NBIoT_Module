using GSM_NBIoT_Module.classes.applicationHelper;
using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// USB_UART конвертор осуществляет общение с модулем и контроллером на плате модема
    /// https://www.silabs.com/interface/usb-bridges/classic/device.cp2105
    /// </summary>
    public class CP2105_Connector : Connector {

        private static CP2105_Connector CP2105_ConnectorInstanse;

        private CP2105_Connector() {
            base.name = "CP2105";
        }

        public static CP2105_Connector GetCP2105_ConnectorInstance() {

            if (CP2105_ConnectorInstanse == null) {
                CP2105_ConnectorInstanse = new CP2105_Connector();
            }

            return CP2105_ConnectorInstanse;
        }

        //Состояние ножек CP2105 на определённом COM порте
        private bool stageGPIO_0;
        private bool stageGPIO_1;
        private bool stageGPIO_2;

        //Порты
        private int enhabcedPort;
        private int standartPort;

        //======================= Маршалинг основных функций CP210xRuntime.dll ==========================
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CreateFile(
        [MarshalAs(UnmanagedType.LPTStr)] string filename,
        [MarshalAs(UnmanagedType.U4)] FileAccess access,
        [MarshalAs(UnmanagedType.U4)] FileShare share,
        IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
        IntPtr templateFile);

        /// <summary>
        /// Считывание состояния ножек
        /// </summary>
        /// <param name="cyHandle">Указатель на область</param>
        /// <param name="lpLatch">Переменная в которую записывается состояние</param>
        /// <returns></returns>
        [DllImport("CP210xRuntime.dll", SetLastError = true)]
        public static extern Int32 CP210xRT_ReadLatch(
            IntPtr cyHandle,
            [param: MarshalAs(UnmanagedType.U4), Out()] out uint lpLatch);

        /// <summary>
        /// Записать состояние ножек
        /// </summary>
        /// <param name="cyHandle">Указатель на область</param>
        /// <param name="mask">В какие пины необходимо произвести запись</param>
        /// <param name="latch">Значение пинов, которое нужно получить(Установка состояния пинов)</param>
        /// <returns></returns>
        [DllImport("CP210xRuntime.dll", SetLastError = true)]
        public static extern Int32 CP210xRT_WriteLatch(IntPtr cyHandle, UInt16 mask, UInt16 latch);
        //================================================================================================

        /// <summary>
        /// Метод необходмый для установки связи с COM-портом
        /// </summary>
        /// <param name="fileName"> Необходимо передать номер COM порта в формате "COM##"</param>
        /// <returns></returns>
        private static IntPtr CreateFile(string fileName) {
            return CreateFile(
                fileName,
                FileAccess.ReadWrite,
                FileShare.None,
                IntPtr.Zero,
                FileMode.Open,
                FileAttributes.Normal,
                IntPtr.Zero);
        }

        /// <summary>
        /// Считывает состояние ножек CP2105 и устанавливает их значение в переменные класса.
        /// Выкидывает исключение DeviseError
        /// </summary>
        /// <param name="COM_portNo"> Номер COM порта к которому подключено устройсво</param>
        public void ReadGPIOStageAndSetFlags(int COM_portNo) {

            //Просто поверьте, так надо
            string COM_portName = "\\\\.\\COM" + COM_portNo;

            IntPtr COM_Port = CreateFile(COM_portName);

            if (COM_Port.ToInt32() == -1)
                throw new DeviceError("Не удалось создать файл необходимый для общения по COM порту.");

            uint statusGPIO = 0;

            //Проверяю успешно ли прошла операция
            returnCodeError(CP210xRT_ReadLatch(COM_Port, out statusGPIO));

            //Возвращаемое число указывает на статус ножек
            //Возвращаемое число в бинарном виде это состояние ножек, подробнее в коде CP210xPortReadWrite.exe
            swithStageGPIO(Convert.ToInt32(statusGPIO));
        }

        public void ReadGPIOStageAndSetFlags(IntPtr COM_portPtr) {

            uint statusGPIO = 0;

            //Проверяю успешно ли прошла операция
            returnCodeError(CP210xRT_ReadLatch(COM_portPtr, out statusGPIO));

            //Возвращаемое число указывает на статус ножек
            //Возвращаемое число в бинарном виде это состояние ножек, подробнее в коде CP210xPortReadWrite.exe
            swithStageGPIO(Convert.ToInt32(statusGPIO));
        }

        /// <summary>
        /// Записывает новое состояние ножек CP2105
        /// модет вернуть ошибку DeviceError()
        /// </summary>
        /// <param name="COM_portNo">Номер ком порта</param>
        /// <param name="stageGPIO_0"></param>
        /// <param name="stageGPIO_1"></param>
        /// <param name="stageGPIO_2"></param>
        public void WriteGPIOStageAndSetFlags(int COM_portNo, Boolean stageGPIO_0, Boolean stageGPIO_1, Boolean stageGPIO_2) {
            //Просто поверьте, так надо
            string COM_portName = "\\\\.\\COM" + COM_portNo;

            IntPtr COM_Port = CreateFile(COM_portName);

            if (COM_Port.ToInt32() == -1)
                throw new DeviceError("Не удалось создать файл необходимый для общения по COM порту.");

            int stageGPIO_ForWrite = getCodeFoeSwithStageGPIO(stageGPIO_0, stageGPIO_1, stageGPIO_2);

            //Охватить все возможные пины
            const ushort mask = 15;

            returnCodeError(Convert.ToInt32(CP210xRT_WriteLatch(COM_Port, mask, (ushort) stageGPIO_ForWrite)));

            ReadGPIOStageAndSetFlags(COM_Port);
        }

        public void WriteGPIOStageAndSetFlags(int COM_portNo, Boolean stageGPIO_0, Boolean stageGPIO_1) {
            //Просто поверьте, так надо
            string COM_portName = "\\\\.\\COM" + COM_portNo;

            IntPtr COM_Port = CreateFile(COM_portName);

            if (COM_Port.ToInt32() == -1)
                throw new DeviceError("Не удалось создать файл необходимый для общения по COM порту.");

            int stageGPIO_ForWrite = getCodeFoeSwithStageGPIO(stageGPIO_0, stageGPIO_1, false);

            //Охватить все возможные пины
            const ushort mask = 15;

            returnCodeError(Convert.ToInt32(CP210xRT_WriteLatch(COM_Port, mask, (ushort)stageGPIO_ForWrite)));

            ReadGPIOStageAndSetFlags(COM_Port);
        }

        /// <summary>
        /// Возвращаемое число указывает на статус ножек. 
        /// Возвращаемое число в бинарном виде это состояние ножек, подробнее в коде CP210xPortReadWrite.exe
        /// </summary>
        /// <param name="statusGPIO"></param>
        private void swithStageGPIO(int statusGPIO) {
            switch (statusGPIO) {
                case 0: {
                        stageGPIO_0 = false;
                        stageGPIO_1 = false;
                        stageGPIO_2 = false;
                    }
                    break;
                case 1: {
                        stageGPIO_0 = true;
                        stageGPIO_1 = false;
                        stageGPIO_2 = false;
                    }
                    break;
                case 2: {
                        stageGPIO_0 = false;
                        stageGPIO_1 = true;
                        stageGPIO_2 = false;
                    }
                    break;
                case 3: {
                        stageGPIO_0 = true;
                        stageGPIO_1 = true;
                        stageGPIO_2 = false;
                    }
                    break;
                case 4: {
                        stageGPIO_0 = false;
                        stageGPIO_1 = false;
                        stageGPIO_2 = true;
                    }
                    break;
                case 5: {
                        stageGPIO_0 = true;
                        stageGPIO_1 = false;
                        stageGPIO_2 = true;
                    }
                    break;
                case 6: {
                        stageGPIO_0 = false;
                        stageGPIO_1 = true;
                        stageGPIO_2 = true;
                    }
                    break;
                case 7: {
                        stageGPIO_0 = true;
                        stageGPIO_1 = true;
                        stageGPIO_2 = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// В зависимости от переданнаваемого состояния ножек возвращает значение которое необходимо передать в CP2105
        /// </summary>
        /// <param name="stageGPIO_0"></param>
        /// <param name="stageGPIO_1"></param>
        /// <param name="stageGPIO_2"></param>
        /// <returns></returns>
        private int getCodeFoeSwithStageGPIO(Boolean stageGPIO_0, Boolean stageGPIO_1, Boolean stageGPIO_2) {
            if (!stageGPIO_0 & !stageGPIO_1 & !stageGPIO_2) return 0;
            else if (stageGPIO_0 & !stageGPIO_1 & !stageGPIO_2) return 1;
            else if (!stageGPIO_0 & stageGPIO_1 & !stageGPIO_2) return 2;
            else if (stageGPIO_0 & stageGPIO_1 & !stageGPIO_2) return 3;
            else if (!stageGPIO_0 & !stageGPIO_1 & stageGPIO_2) return 4;
            else if (stageGPIO_0 & !stageGPIO_1 & stageGPIO_2) return 5;
            else if (!stageGPIO_0 & stageGPIO_1 & stageGPIO_2) return 6;
            else if (stageGPIO_0 & stageGPIO_1 & stageGPIO_2) return 7;

            else throw new IndexOutOfRangeException("Идекс больше кол-ва ножек GPIO");
        }

        /// <summary>
        /// Ошибки возникающие при считывании состояния ножек c CP2105
        /// </summary>
        /// <param name="code">Код ошибки</param>
        private void returnCodeError(int code) {
            switch (code) {
                case 0: return;
                case 255: throw new DeviceError("CP2105_DEVICE_NOT_FOUND");
                case 1: throw new DeviceError("CP2105_INVALID_HANDLE");
                case 2: throw new DeviceError("CP2105_INVALID_PARAMETER");
                case 3: throw new DeviceError("CP2105_DEVICE_IO_FAILED");
                case 4: throw new DeviceError("CP2105_FUNCTION_NOT_SUPPORTED");
                case 5: throw new DeviceError("CP2105_GLOBAL_DATA_ERROR");
                case 7: throw new DeviceError("CP2105_FILE_ERROR");
                case 8: throw new DeviceError("CP2105_COMMAND_FAILED");
                case 9: throw new DeviceError("CP2105_INVALID_ACCESS_TYPE");
            }
        }

        /// <summary>
        /// Автоматический поиск портов устройства
        /// Выкидывает исключение DeviceNotFoundException()
        /// </summary>
        private void findDevicePorts() {

            string query = "SELECT * FROM Win32_SerialPort";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            enhabcedPort = 0;
            standartPort = 0;

            bool findEnha = false;
            bool findSta = false;

            //Имя устройств
            const string searhEnhancedPort = "Silicon Labs Dual CP210x USB to UART Bridge: Enhanced COM Port";
            const string searhStandartPort = "Silicon Labs Dual CP210x USB to UART Bridge: Standard COM Port";

             foreach (ManagementObject service in searcher.Get()) {

                string portDescription = (string) service["Description"];

                if (!findEnha) {
                    if (searhEnhancedPort.Equals(portDescription)) {
                        string port = (string) service["Name"];
                        int startIndex = port.IndexOf('(');
                        int lastIndex = port.IndexOf(')');
                        int leght = lastIndex - (startIndex + 4);

                        enhabcedPort = Convert.ToInt32(port.Substring(startIndex + 4, leght));

                        findEnha = true;
                        continue;
                    }
                }

                if (!findSta) {
                    if (searhStandartPort.Equals(portDescription)) {
                        string port = (string)service["Name"];
                        int startIndex = port.IndexOf('(');
                        int lastIndex = port.IndexOf(')');
                        int leght = lastIndex - (startIndex + 4);

                        standartPort = Convert.ToInt32(port.Substring(startIndex + 4, leght));

                        findSta = true;
                    }
                }
            }

            if (enhabcedPort == 0 || standartPort == 0) 
                throw new DeviceNotFoundException("Не удалось найти модем в списке подключенных устройств");

        }

        public override void SendData() {
            throw new NotImplementedException();
        }


        //================== getters and setters =======================
        public bool getStageGPIO_0() {
            return stageGPIO_0;
        }

        public bool getStageGPIO_1() {
            return stageGPIO_1;
        }

        public bool getStageGPIO_2() {
            return stageGPIO_2;
        }

        public int getEnhabcedPort() {
            findDevicePorts();

            return enhabcedPort;
        }
        public int getStandartPort() {
            findDevicePorts();

            return standartPort;
        }
    }
}
