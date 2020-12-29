using GSM_NBIoT_Module.classes.applicationHelper;
using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// USB_UART конвертор осуществляет общение, а так же изменение состояниня модуля и контроллера на плате модема
    /// https://www.silabs.com/interface/usb-bridges/classic/device.cp2105
    /// </summary>
    public class CP2105_Connector : Connector {

        //Объект коннектора
        private static CP2105_Connector CP2105_ConnectorInstanse;

        private CP2105_Connector() {
            base.name = "CP2105";
        }

        /// <summary>
        /// Возвращает объект класса коннектора
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Структура состояния GPIO у добавочного порта
        /// </summary>
        public struct StateGPIO_OnEnhabcedPort {
            public bool stageGPIO_0;
            public bool stageGPIO_1;
        }

        /// <summary>
        /// Структура состояния GPIO у стандартного порта
        /// </summary>
        public struct StateGPIO_OnStandardPort {
            public bool stageGPIO_0;
            public bool stageGPIO_1;
            public bool stageGPIO_2;
        }

        //Порты
        private int enhabcedPort;
        private int standardPort;

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
        /// Метод необходмый для установки связи с COM-портом возвращает указатель на устройство
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

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);
        private static bool MyCloseHandle(IntPtr hObject) {
            return CloseHandle(hObject);
        }

        /// <summary>
        /// Считывание состояния ножек GPIO
        /// </summary>
        /// <param name="cyHandle">Указатель на область</param>
        /// <param name="lpLatch">Переменная в которую записывается состояние</param>
        /// <returns></returns>
        [DllImport("CP210xRuntime.dll", SetLastError = true)]
        public static extern int CP210xRT_ReadLatch(
            IntPtr cyHandle,
            [param: MarshalAs(UnmanagedType.U4), Out()] out uint lpLatch);
        private static int ReadLatch(IntPtr cyHandle, out uint lpLatch) {
            return CP210xRT_ReadLatch(cyHandle, out lpLatch);
        }

        /// <summary>
        /// Записать состояние ножек GPIO
        /// </summary>
        /// <param name="cyHandle">Указатель на область</param>
        /// <param name="mask">В какие пины необходимо произвести запись</param>
        /// <param name="latch">Значение пинов, которое нужно получить(Установка состояния пинов)</param>
        /// <returns></returns>
        [DllImport("CP210xRuntime.dll", SetLastError = true)]
        public static extern int CP210xRT_WriteLatch(
            IntPtr cyHandle,
            [param: MarshalAs(UnmanagedType.U2)] ushort mask,
            [param: MarshalAs(UnmanagedType.U2)] ushort latch);
        private static int WriteLatch(IntPtr cyHandle, ushort mask, ushort latch) {
            return CP210xRT_WriteLatch(cyHandle, mask, latch);
        }

        //================================================================================================

        /// <summary>
        /// Возвращает состояние ножек на добавочном порте
        /// </summary>
        /// <returns></returns>
        public StateGPIO_OnEnhabcedPort GetStageGPIOEnhabcedPort() {

            StateGPIO_OnEnhabcedPort stateGPIO_OnEnhabcedPort = new StateGPIO_OnEnhabcedPort();

            ReadGPIOStageAndSetFlags(enhabcedPort);

            stateGPIO_OnEnhabcedPort.stageGPIO_0 = stageGPIO_0;
            stateGPIO_OnEnhabcedPort.stageGPIO_1 = stageGPIO_1;

            return stateGPIO_OnEnhabcedPort;
        }

        /// <summary>
        /// Возвращает состояние ножек на стандртном порте
        /// </summary>
        /// <returns></returns>
        public StateGPIO_OnStandardPort GetStageGPIOStandardPort() {

            StateGPIO_OnStandardPort stateGPIO_OnStandardPort = new StateGPIO_OnStandardPort();

            ReadGPIOStageAndSetFlags(standardPort);

            stateGPIO_OnStandardPort.stageGPIO_0 = stageGPIO_0;
            stateGPIO_OnStandardPort.stageGPIO_1 = stageGPIO_1;
            stateGPIO_OnStandardPort.stageGPIO_2 = stageGPIO_2;

            return stateGPIO_OnStandardPort;
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
                throw new DeviceError("Не удалось открыть COM порт.");

            uint statusGPIO = 0;

            //Проверяю успешно ли прошла операция
            returnCodeError(ReadLatch(COM_Port, out statusGPIO));

            //Возвращаемое число указывает на статус ножек
            //Возвращаемое число в бинарном виде это состояние ножек, подробнее в коде CP210xPortReadWrite.exe
            swithStageGPIO((int)statusGPIO);

            MyCloseHandle(COM_Port);
        }

        /// <summary>
        /// Считывает состояние ножек CP2105 и устанавливает их значение в переменные класса.
        /// Выкидывает исключение DeviseError
        /// </summary>
        /// <param name="COM_portPtr"> Ссылка на устройство COM порта в системе</param>
        public int ReadGPIOStageAndSetFlags(IntPtr COM_portPtr) {

            uint statusGPIO = 0;

            //Проверяю успешно ли прошла операция
            returnCodeError(ReadLatch(COM_portPtr, out statusGPIO));

            //Возвращаемое число указывает на статус ножек
            //Возвращаемое число в бинарном виде это состояние ножек, подробнее в коде CP210xPortReadWrite.exe
            swithStageGPIO((int)statusGPIO);

            return (int)statusGPIO;
        }

        /// <summary>
        /// Записывает новое состояние ножек CP2105
        /// может вернуть ошибку DeviceError()
        /// </summary>
        /// <param name="COM_portNo">Номер ком порта</param>
        /// <param name="stageGPIO_0"></param>
        /// <param name="stageGPIO_1"></param>
        /// <param name="stageGPIO_2"></param>
        /// <param name="sleepMls"></param>
        public void WriteGPIOStageAndSetFlags(int COM_portNo, bool stageGPIO_0, bool stageGPIO_1, bool stageGPIO_2, int sleepMls) {

            string COM_portName = "\\\\.\\COM" + COM_portNo;

            IntPtr COM_Port = CreateFile(COM_portName);

            if (COM_Port.ToInt32() == -1)
                throw new DeviceError("Не удалось найти дескриптор необходимый для связи по COM порту. Убедитесь, что порты модема не заняты другой программой и попробуйте снова");

            int stageGPIO_ForWrite = getCodeFoeSwithStageGPIO(stageGPIO_0, stageGPIO_1, stageGPIO_2);

            //Охватить все возможные пины
            const ushort mask = 15;

            returnCodeError(Convert.ToInt32(WriteLatch(COM_Port, mask, (ushort)stageGPIO_ForWrite)));

            Thread.Sleep(sleepMls);

            int resultGPIO = ReadGPIOStageAndSetFlags(COM_Port);

            //Если состояние ног GPIO не установилось, то пробую ещё раз
            if (resultGPIO != stageGPIO_ForWrite) {

                for (int i = 1; i < 8; i++) {

                    Flasher.addMessageInMainLog("Запрос подтверждения состояния ног CP2105 №" + i);

                    Thread.Sleep(500);

                    resultGPIO = ReadGPIOStageAndSetFlags(COM_Port);

                    if (resultGPIO == stageGPIO_ForWrite) {
                        MyCloseHandle(COM_Port);
                        Flasher.addMessageInMainLog("Подтверждение получено");
                        return;
                    }
                }

                MyCloseHandle(COM_Port);

                throw new DeviceError("Не удалось выставить необходимое состояние ног CP2105, перезагрузите модем и попробуйте снова.");
            }

            MyCloseHandle(COM_Port);
        }

        public void WriteGPIOStageAndSetFlags(int COM_portNo, bool stageGPIO_0, bool stageGPIO_1, int sleepMls) {

            string COM_portName = "\\\\.\\COM" + COM_portNo;

            IntPtr COM_Port = CreateFile(COM_portName);

            if (COM_Port.ToInt32() == -1)
                throw new DeviceError("Не удалось найти дескриптор необходимый для связи по COM порту. Убедитесь, что порты модема не заняты другой программой и попробуйте снова");

            int stageGPIO_ForWrite = getCodeFoeSwithStageGPIO(stageGPIO_0, stageGPIO_1, false);

            //Охватить все возможные пины
            const ushort mask = 15;

            returnCodeError(Convert.ToInt32(WriteLatch(COM_Port, mask, (ushort)stageGPIO_ForWrite)));

            Thread.Sleep(sleepMls);

            int resultGPIO = ReadGPIOStageAndSetFlags(COM_Port);

            //Если состояние ног GPIO не установилось, то пробую ещё раз
            if (resultGPIO != stageGPIO_ForWrite) {

                for (int i = 1; i < 8; i++) {
                    Flasher.addMessageInMainLog("Запрос подтверждения состояния ног CP2105 №" + i);

                    Thread.Sleep(500);

                    resultGPIO = ReadGPIOStageAndSetFlags(COM_Port);

                    if (resultGPIO == stageGPIO_ForWrite) {
                        MyCloseHandle(COM_Port);
                        Flasher.addMessageInMainLog("Подтверждение получено");
                        return;
                    }
                }

                MyCloseHandle(COM_Port);

                throw new DeviceError("Не удалось выставить необходимое состояние ног CP2105, перезагрузите модем и попробуйте снова.");
            }

            MyCloseHandle(COM_Port);
        }

        /// <summary>
        /// Возвращаемое число указывающее на статус ножек. 
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
        /// В зависимости от переданнаваемого состояния ножек возвращает значение, которое необходимо передать в CP2105
        /// </summary>
        /// <param name="stageGPIO_0"></param>
        /// <param name="stageGPIO_1"></param>
        /// <param name="stageGPIO_2"></param>
        /// <returns></returns>
        private int getCodeFoeSwithStageGPIO(bool stageGPIO_0, bool stageGPIO_1, bool stageGPIO_2) {
            if (!stageGPIO_0 & !stageGPIO_1 & !stageGPIO_2) return 0;
            else if (stageGPIO_0 & !stageGPIO_1 & !stageGPIO_2) return 1;
            else if (!stageGPIO_0 & stageGPIO_1 & !stageGPIO_2) return 2;
            else if (stageGPIO_0 & stageGPIO_1 & !stageGPIO_2) return 3;
            else if (!stageGPIO_0 & !stageGPIO_1 & stageGPIO_2) return 4;
            else if (stageGPIO_0 & !stageGPIO_1 & stageGPIO_2) return 5;
            else if (!stageGPIO_0 & stageGPIO_1 & stageGPIO_2) return 6;
            else if (stageGPIO_0 & stageGPIO_1 & stageGPIO_2) return 7;

            else throw new IndexOutOfRangeException("Идекс больше количества ножек GPIO");
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
        public void FindDevicePorts() {

            string query = "SELECT * FROM Win32_SerialPort";
            string querySecond = "SELECT * FROM Win32_PnPEntity where ClassGuid = '{4d36e978-e325-11ce-bfc1-08002be10318}'";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            int enhabcedPort = 0;
            int standardPort = 0;

            bool findEnha = false;
            bool findSta = false;

            //Имя устройств
            const string searhEnhancedPort = "Silicon Labs Dual CP210x USB to UART Bridge: Enhanced COM Port";
            const string searhStandardPort = "Silicon Labs Dual CP210x USB to UART Bridge: Standard COM Port";

            foreach (ManagementObject service in searcher.Get()) {


                string portDescription = (string)service["Description"];

                if (!findEnha) {
                    if (searhEnhancedPort.Equals(portDescription)) {
                        string port = (string)service["Name"];
                        int startIndex = port.IndexOf('(');
                        int lastIndex = port.IndexOf(')');
                        int leght = lastIndex - (startIndex + 4);

                        enhabcedPort = Convert.ToInt32(port.Substring(startIndex + 4, leght));

                        findEnha = true;
                        continue;
                    }
                }

                if (!findSta) {
                    if (searhStandardPort.Equals(portDescription)) {
                        string port = (string)service["Name"];
                        int startIndex = port.IndexOf('(');
                        int lastIndex = port.IndexOf(')');
                        int leght = lastIndex - (startIndex + 4);

                        standardPort = Convert.ToInt32(port.Substring(startIndex + 4, leght));

                        findSta = true;
                    }
                }
            }

            if (enhabcedPort == 0 || standardPort == 0) {
                Flasher.addMessageInMainLog("Не удалось найти модем в списке подключенных устройств");
                Flasher.addMessageInMainLog("Использую другой способ");

                searcher = new ManagementObjectSearcher(querySecond);

                findEnha = false;
                findSta = false;

                foreach (ManagementObject service in searcher.Get()) {

                    string portDescription = (string)service["Description"];

                    if (!findEnha) {
                        if (searhEnhancedPort.Equals(portDescription)) {
                            string port = (string)service["Name"];
                            int startIndex = port.IndexOf('(');
                            int lastIndex = port.IndexOf(')');
                            int leght = lastIndex - (startIndex + 4);

                            enhabcedPort = Convert.ToInt32(port.Substring(startIndex + 4, leght));

                            findEnha = true;
                            continue;
                        }
                    }

                    if (!findSta) {
                        if (searhStandardPort.Equals(portDescription)) {
                            string port = (string)service["Name"];
                            int startIndex = port.IndexOf('(');
                            int lastIndex = port.IndexOf(')');
                            int leght = lastIndex - (startIndex + 4);

                            standardPort = Convert.ToInt32(port.Substring(startIndex + 4, leght));

                            findSta = true;
                        }
                    }
                }

                if (enhabcedPort == 0 || standardPort == 0) {
                    throw new DeviceNotFoundException("Не удалось найти модем в списке подключенных устройств");
                }

            } else {
                this.enhabcedPort = enhabcedPort;
                this.standardPort = standardPort;
                return;
            }

            this.enhabcedPort = enhabcedPort;
            this.standardPort = standardPort;
        }

    /// <summary>
    /// проверяет количество подключенных модемов к компьютеру,
    /// если больше одного, то выкидывает исключение
    /// </summary>
    public void amountDevicesConnect() {
        const string searhEnhancedPort = "Silicon Labs Dual CP210x USB to UART Bridge: Enhanced COM Port";
        const string searhStandardPort = "Silicon Labs Dual CP210x USB to UART Bridge: Standard COM Port";

        int countEnhabcedPort = 0;
        int countStandardPort = 0;

        string query = "SELECT * FROM Win32_SerialPort";
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

        foreach (ManagementObject service in searcher.Get()) {

            string portDescription = (string)service["Description"];

            if (searhEnhancedPort.Equals(portDescription)) {
                countEnhabcedPort++;
            }

            if (searhStandardPort.Equals(portDescription)) {
                countStandardPort++;
            }
        }

        if (countEnhabcedPort > 1 || countStandardPort > 1)
            throw new DeviceError("В списке устройств найдено больше одного модема, для корректной работы программы должно быть подключено не более одного модема");
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
        if (enhabcedPort == 0) throw new DeviceNotFoundException("Не выставленно значение Enhanced порта");
        return enhabcedPort;
    }

    public int getStandardPort() {
        if (enhabcedPort == 0) throw new DeviceNotFoundException("Не выставленно значение Standard порта");
        return standardPort;
    }

    public void setEnhabcedPort(int portNo) {
        this.enhabcedPort = portNo;
    }

    public void setStandardPort(int portNo) {
        this.standardPort = portNo;
    }
}
}