using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using GSM_NBIoT_Module.classes.controllerOnBoard.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// Контроллер STM32L412CB выполнеяет функцию обработки полученных данных с модуля
    ///https://www.st.com/en/microcontrollers-microprocessors/stm32l412cb.html
    ///
    /// Запись реализована с помощью команд BootLoader'a подробнее см. в файлах программы.
    /// </summary>
    public class STM32L412CB_Controller : Controller {

        public STM32L412CB_Controller() {
            base.name = "STM32L412CB";
        }

        //Обект конфигурации для прошивки контроллера
        private ConfigurationFW configuration;

        //Полная загружаемая прошивка, сформированная в буферы по 256 байт (key<address>, value<buffer(256)>)
        private SortedList<uint, List<byte>> firmwareData;

        //Необходимо ли сделать полную верификацию прошивки?
        private bool fullVerification = true;

        //Получаю объект связи с контроллером
        private CP2105_Connector CP2105_Connector = CP2105_Connector.GetCP2105_ConnectorInstance();

        private SerialPort serialPort = new SerialPort();
        private int timeOut = 50000;

        //Версия бутлоадера контроллера
        private string verBootLoader = null;

        //Сколько раз была включена команда на защиту от записи/чтения
        private int protectedReadWriteEnabled = -1;
        //Сколько раз была отключена команда на защиту от записи/чтения
        private int protectedReadWriteDisabled = -1;

        //Получение ID чипа
        //PID stands for product ID. Byte 1 is the MSB and byte 2 the LSB of the ID.
        private byte MSB = 0x04;
        private byte LSB;

        //Ответы контроллера
        private const byte ACK = 0x79; //ОК
        private const byte NACK = 0x1F; // НЕ OK

        //Стартовый адрес записи прошивки в контроллер
        private const uint startAddressForWrite = 0x08000000;

        //Адрес начала конфигурации прошивики
        private const uint configurationAdress = 0x08000200;

        /// <summary>
        /// Список разрешенных команд bootloadera микроконтроллера
        /// </summary>
        public struct Commands {
            public bool GET_CMD;                    //Get the version and the allowed commands supported by the current version of the boot loader
            public bool GET_VER_ROPS_CMD;           //Get the BL version and the Read Protection status of the NVM
            public bool GET_ID_CMD;                 //Get the chip ID
            public bool READ_CMD;                   //Read up to 256 bytes of memory starting from an address specified by the user
            public bool GO_CMD;                     //Jump to an address specified by the user to execute (a loaded) code
            public bool WRITE_CMD;                  //Write maximum 256 bytes to the RAM or the NVM starting from an address specified by the user
            public bool ERASE_CMD;                  //Erase from one to all the NVM sectors
            public bool ERASE_EXT_CMD;              //Erase from one to all the NVM sectors
            public bool WRITE_PROTECT_CMD;          //Enable the write protection in a permanent way for some sectors
            public bool WRITE_UNPROTECT_CMD;        //Disable the write protection in a permanent way for all NVM sectors
            public bool READOUT_PROTECT_CMD;        //Enable the readout protection in a permanent way
            public bool READOUT_UNPROTECT_CMD;      //Disable the readout protection in a temporary way
        }

        /// <summary>
        /// Открывает COM порт с параметрами
        /// </summary>
        /// <param name="NmbPort">Номер порта</param>
        /// <param name="bandRate">Скорость передачи</param>
        /// <param name="parity">Четность</param>
        /// <param name="dataBits">Число стоповых битов</param>
        public void OpenSerialPort(int NmbPort, int bandRate, Parity parity, int dataBits, StopBits stopBit) {
            serialPort.PortName = "COM" + NmbPort;
            serialPort.BaudRate = bandRate;
            serialPort.Parity = parity;
            serialPort.DataBits = dataBits;
            serialPort.StopBits = stopBit;

            serialPort.Open();
        }

        /// <summary>
        /// Проверяет открыт ли текущий порт
        /// </summary>
        /// <returns></returns>
        public bool PortIsOpen() {
            return serialPort.IsOpen;
        }

        /// <summary>
        /// Задаёт таймаут для чтения из COM порта
        /// </summary>
        /// <param name="timeOut"></param>
        public void setTimeOut(int timeOut) {
            this.timeOut = timeOut;
        }

        /// <summary>
        /// Закрывает COM порт 
        /// </summary>
        public void ClosePort() {
            serialPort.Close();
            
            if (serialPort.IsOpen) {
                throw new COMException("Не удалось закрыть COM порт " + serialPort.PortName);
            }
        }

        /// <summary>
        /// Получает список доступных команд для bootloadera микроконтроллера
        /// </summary>
        /// <returns></returns>
        public Commands GET() {
            Commands commands = new Commands();

            List<byte> answer = sendDataInCOM(false, 0x00, 0xFF);

            if (answer.Contains(0x00)) {
                commands.GET_CMD = true;
            }

            if (answer.Contains(0x01)) {
                commands.GET_VER_ROPS_CMD = true;
            }

            if (answer.Contains(0x02)) {
                commands.GET_ID_CMD = true;
            }

            if (answer.Contains(0x11)) {
                commands.READ_CMD = true;
            }

            if (answer.Contains(0x21)) {
                commands.GO_CMD = true;
            }

            if (answer.Contains(0x31)) {
                commands.WRITE_CMD = true;
            }

            if (answer.Contains(0x44)) {
                commands.ERASE_CMD = true;
            }

            if (answer.Contains(0x63)) {
                commands.WRITE_PROTECT_CMD = true;
            }

            if (answer.Contains(0x73)) {
                commands.WRITE_UNPROTECT_CMD = true;
            }

            if (answer.Contains(0x82)) {
                commands.READOUT_PROTECT_CMD = true;
            }

            if (answer.Contains(0x92)) {
                commands.READOUT_UNPROTECT_CMD = true;
            }

            return commands;
        }


        /// <summary>
        /// Получает версию бутлоадера и количество включений и выключений защиты от записи и чтения прошивки контроллера
        /// Скорее всего не работает и сделана для совместимости
        /// </summary>
        public void GET_VER_ROPS() {
            if (!PortIsOpen()) throw new COMException("COM порт закрыт, откройте COM прот и попробуйте снова");

            List<byte> answer = sendDataInCOM(false, 0x01, 0xFE);

            this.verBootLoader = answer.ElementAt(1).ToString();

            protectedReadWriteEnabled = answer.ElementAt(2);

            protectedReadWriteDisabled = answer.ElementAt(3);
        }

        public void GET_ID() {
            if (!PortIsOpen()) throw new COMException("COM порт закрыт, откройте COM прот и попробуйте снова");

            List<byte> answer = sendDataInCOM(false, 0x02, 0xFD);

            LSB = answer.ElementAt(3);
        }

        /// <summary>
        /// Снимает защиту от чтения флэш памяти 
        /// ВНИМАНИЕ! При снятии зашиты от чтения вся флэш память отчищается
        /// </summary>
        public void READOUT_UNPROTECT() {
            if (!PortIsOpen()) throw new COMException("COM порт закрыт, откройте COM прот и попробуйте снова");

            sendDataInCOM(true, 0x92, 0x6D);
        }

        /// <summary>
        /// Устанавливает защиту от чтения 
        /// ВНИМАНИЕ! Перед тем как установить защиту необходимо учесть то, что чтобы её в дальнейшем снять удалятся все данные с флеш памяти контроллера
        /// </summary>
        public void READOUT_PROTECT() {
            if (!PortIsOpen()) throw new COMException("COM порт закрыт, откройте COM прот и попробуйте снова");

            sendDataInCOM(true, 0x82, 0x7D);
        }

        /// <summary>
        /// Защита от записи
        /// </summary>
        public void WRITE_PROTECT() {
            if (!PortIsOpen()) throw new COMException("COM порт закрыт, откройте COM прот и попробуйте снова");

            sendDataInCOM(true, 0x63, 0x9C);
        }

        /// <summary>
        /// Отключение защиты от записи
        /// </summary>
        public void WRITE_UNPROTECT() {
            if (!PortIsOpen()) throw new COMException("COM порт закрыт, откройте COM прот и попробуйте снова");

            sendDataInCOM(true, 0x73, 0x8C);
        }

        /// <summary>
        /// Отчистка памяти контроллера (удаление старой прошивки)
        /// </summary>
        public override void ERASE() {
            if (!PortIsOpen()) throw new COMException("COM порт закрыт, откройте COM прот и попробуйте снова");

            //Отправляю контроллеру заброс на команду отчистки
            sendDataInCOM(true, 0x44, 0xBB);

            //Удаляю всё из всех секторов
            sendDataInCOM(true, 0xFF, 0xFF, 0x00);
        }

        /// <summary>
        /// Инациализация скорости для обмена данными с контроллером
        /// </summary>
        public void INIT() {
            if (!PortIsOpen()) throw new COMException("COM порт закрыт, откройте COM прот и попробуйте снова");

            sendDataInCOM(false, 0x7F);
        }

        /// <summary>
        /// Выход из состояния boot'a и переход к выполнению программы прошивки
        /// </summary>
        public void GO() {
            if (!PortIsOpen()) throw new COMException("COM порт закрыт, откройте COM прот и попробуйте снова");

            sendDataInCOM(true, 0x21, 0xDE);
            sendDataInCOM(true, 0x08, 0x00, 0x00, 0x00, 0x08);
        }


        //Структура, необходимая для возврата данных из распарсенной строчки HEX файла, формат строки HEX файла см. в интернете
        struct DataInHEX {
            //Адрес данных в строке
            public uint address;
            //Размер данных
            public byte amountDataByte;
            //Сами данные
            public byte[] data;
        }

        //Старая реализация записи прошивки (без конфигурации, !!!НЕ УДАЛЯТЬ!!! сохранено для информации)
  /*      public override void WRITE(string pathToHex) {

            firmwareData = new SortedList<uint, List<byte>>();

            //Строка из HEX файла
            string line;

            //Адрес записи данных накопившихся в буфере
            uint writeAddress = 0x08000000;

            //Текущий адрес считываемой линии
            uint currentAddress = 0x08000000;

            //Предполакаемый адрес линии, который должен быть
            uint addressOffset = 0x08000000;

            //Структура обработки полученной строки
            DataInHEX dataInHEX;

            List<byte> buffer = new List<byte>(258);

            //Окрываю файл прошивки и считываю с него данные
            using (StreamReader hexFileSream = new StreamReader(pathToHex)) {
                
                while(true) {

                    if (buffer.Count == 256) {

                        WriteBufferToMK_AndVerify(writeAddress, buffer);

                        writeAddress += 256;

                        buffer = new List<byte>(258);
                    }
                                        
                    line = hexFileSream.ReadLine();

                    //Если конец файла, то выхожу
                    if (line == null) {
                        //Если в буфере остались незаписанные данные flush
                        if (buffer.Count != 0) WriteBufferToMK_AndVerify(writeAddress, buffer);

                        //Если нужна полная верификация
                        if (fullVerification) {
                            Flasher.addMessInLogBuffer("\n==========================================================================================");
                            Flasher.addMessInLogBuffer("Полная проверка записанной прошивки" + Environment.NewLine);

                            fullVerificationFirmwareInMK();

                            Flasher.addMessInLogBuffer("\n==========================================================================================");
                            Flasher.addMessInLogBuffer("Верификация прошивки прошла успешно" + Environment.NewLine);
                        }

                        break;
                    }

                    //Убираю знак ":"
                    line = line.Substring(1);

                    //Получаю из стринг строки массив байт
                    byte[] dataLineToArr = StringToByteArray(line);

                    //Если строка на запись
                    if (dataLineToArr[3] == 0x00) {

                        dataInHEX = ParseByteLineHEX(dataLineToArr);

                        currentAddress = dataInHEX.address;

                        //Если следующий адрес равен тому, который должен быть (Проверка на разрыв)
                        if (currentAddress == addressOffset) {

                            //Задаю новый адрес смещения в зависимости от кол-ва принятых байт
                            addressOffset = dataInHEX.address + dataInHEX.amountDataByte;

                            //Если место в буфере есть, то записываю данные
                            if (buffer.Count + dataInHEX.amountDataByte <= 256) {

                                buffer.AddRange(dataInHEX.data);

                            //Если в буфере нет места
                            } else {

                                //Создаю индекс для передачи
                                byte i = 0;

                                //Записываю часть, которая влезет в буфер
                                while (buffer.Count <= 256) {
                                    buffer.Add(dataInHEX.data[i]);
                                    i++;
                                }

                                //Выгружаю данный буфер в МК
                                WriteBufferToMK_AndVerify(writeAddress, buffer);

                                buffer = new List<byte>(258);

                                writeAddress = currentAddress - i;

                                //Догружаю в буфер оставшиеся байты
                                for (int j = i; j < dataInHEX.data.Length; j++) {

                                    buffer.Add(dataInHEX.data[j]);
                                }
                            }
                        // Если предполагаемый адрес не равен текущему адресу (Разрыв)
                        } else {

                            //Выгружаю всё что было до этого в буфере в контроллер
                            WriteBufferToMK_AndVerify(writeAddress, buffer);

                            buffer = new List<byte>(258);
                            writeAddress = dataInHEX.address;
                            addressOffset = dataInHEX.address + dataInHEX.amountDataByte;
                            buffer.AddRange(dataInHEX.data);
                        }
                    }
                }
            }
        }*/

        public override void WRITE (string pathToHex) {
            firmwareData = new SortedList<uint, List<byte>>();

            //Строка из HEX файла
            string line;

            //Адрес записи данных накопившихся в буфере
            uint writeAddress = 0x08000000;

            //Текущий адрес считываемой линии
            uint currentAddress = 0x08000000;

            //Предполагаемый адрес линии, который должен быть (адрес смещения)
            uint addressOffset = 0x08000000;

            //Структура обработки полученной строки
            DataInHEX dataInHEX;

            //Буфер даных для записи в МК
            List<byte> buffer = new List<byte>(258);

            //Окрываю файл прошивки и считываю с него данные
            using (StreamReader hexFileSream = new StreamReader(pathToHex)) {

                while (true) {

                    //Если буфер для записи полон отправляю его к остальным данным для записи
                    if (buffer.Count == 256) {

                        firmwareData.Add(writeAddress, buffer);

                        //Адрес записи следующего буфера увеличиваю на размер сформированного буфера
                        writeAddress += 256;

                        buffer = new List<byte>(258);
                    }

                    //Считываю строку из HEX файла
                    line = hexFileSream.ReadLine();

                    //============================================= Если конец файла, то выхожу
                    if (line == null) {
                        //Если в буфере остались незаписанные данные то выгружаю их к остальным данным для записи flush
                        if (buffer.Count != 0) {
                            firmwareData.Add(writeAddress, buffer);
                        }

                        //Записываю распарсенную и сконфигурированную прошивку в МК
                        writeDataInMK();

                        //Если нужна полная верификация
                        if (fullVerification) {
                            Flasher.addMessInLogBufferWithoutTime("==========================================================================================");
                            Flasher.addMessInLogBuffer("Полная проверка записанной прошивки" + Environment.NewLine);
                            
                            fullVerificationFirmwareInMK();

                            Flasher.addMessInLogBufferWithoutTime("==========================================================================================");
                            Flasher.addMessInLogBuffer("Верификация прошивки прошла успешно" + Environment.NewLine);
                        }

                        break;
                    }

                    //Убираю знак ":"
                    line = line.Substring(1);

                    //Получаю из стринг строки массив байт (конвертиру str байты в байты)
                    byte[] dataLineToArr = StringToByteArray(line);

                    //Если строка на запись (см. формат формирования строки HEX файла)
                    if (dataLineToArr[3] == 0x00) {

                        //Получаю адрес, байты для записи и количество байт для записи
                        dataInHEX = ParseByteLineHEX(dataLineToArr);

                        //Обновляю текущий адрес считывания
                        currentAddress = dataInHEX.address;

                        //Если следующий адрес равен тому, который должен быть (Проверка на разрыв)
                        if (currentAddress == addressOffset) {

                            //Задаю новый адрес смещения в зависимости от кол-ва принятых байт
                            addressOffset = dataInHEX.address + dataInHEX.amountDataByte;

                            //Если место в буфере есть, то записываю данные
                            if (buffer.Count + dataInHEX.amountDataByte <= 256) {

                                buffer.AddRange(dataInHEX.data);

                                //Если в буфере нет места
                            } else {

                                //Создаю индекс для передачи
                                byte i = 0;

                                //Записываю часть, которая влезет в буфер
                                while (buffer.Count < 256) {
                                    buffer.Add(dataInHEX.data[i]);
                                    i++;
                                }

                                //Выгружаю данный буфер в МК
                                firmwareData.Add(writeAddress, buffer);

                                buffer = new List<byte>(258);

                                writeAddress = currentAddress - i;

                                //Догружаю в буфер оставшиеся байты
                                for (int j = i; j < dataInHEX.data.Length; j++) {

                                    buffer.Add(dataInHEX.data[j]);
                                }
                            }
                            // Если предполагаемый адрес не равен текущему адресу (Разрыв)
                        } else {

                            //Выгружаю всё что было до этого в буфере в контроллер
                            firmwareData.Add(writeAddress, buffer);

                            buffer = new List<byte>(258);
                            writeAddress = dataInHEX.address;
                            addressOffset = dataInHEX.address + dataInHEX.amountDataByte;
                            buffer.AddRange(dataInHEX.data);
                        }

                        //============================================================ Конфигурация прошивки (записывается при чтении файла прошивки)
                        //Если адрес равен адресу с которого необходимо начать запись конфигурации модема
                        if (currentAddress == configurationAdress) {

                            //Считываю номер версии и имя прошивки (данные записаные в загружаемой прошивке)
                            byte[] dataNverAndFrimfareName = dataInHEX.data;

                            //Номер версии
                            byte Nver = dataNverAndFrimfareName[0];
                            //Имя прошивки в байтах
                            byte[] FWnameByte = new byte[5];
                            //Заполняю массив именем прошивки
                            Array.Copy(dataNverAndFrimfareName, 1, FWnameByte, 0, FWnameByte.Length);

                            //Конвертирую байты в str имя прошивки
                            string FWName = Encoding.GetEncoding("ASCII").GetString(FWnameByte);

                            //Сравниваю имя загружаемой прошивки с тем, которое указано в самой прошивке
                            if (!FWName.Equals(Path.GetFileNameWithoutExtension(pathToHex))) throw new MKCommandException("Имя файла прошивки не соответствует заложенной в HEX файле");

                            //Получаю адрес следующей строки в HEX файле
                            string nextLine = hexFileSream.ReadLine();

                            //Получаю из неё адрес (пошаговые преобразования изложены выше в ходе выполнения функции)
                            dataInHEX = ParseByteLineHEX(StringToByteArray(nextLine.Substring(1)));

                            uint nextAddress = dataInHEX.address;

                            //Получаю размер пустой области в прошивке
                            int sizeForConfiguration = (int)(nextAddress - configurationAdress);

                            //Получаю байты конфигурации, которые необходимо записать в записываемую прошивку
                            byte[] configurationBytes = configuration.formationOfConfigurationData(Nver, sizeForConfiguration, dataNverAndFrimfareName);

                            //Имя и сценарий записи уже в буфере
                            //Добавляю полученные в результате конфигурации байты в буфер
                            if (buffer.Count + configurationBytes.Length <= 256) {

                                buffer.AddRange(configurationBytes);

                                //Если в буфере нет места
                            } else {

                                //Создаю индекс для передачи
                                byte i = 0;

                                //Записываю часть, которая влезет в буфер
                                while (buffer.Count < 256) {
                                    buffer.Add(configurationBytes[i]);
                                    i++;
                                }

                                //Выгружаю данный буфер в МК
                                firmwareData.Add(writeAddress, buffer);

                                buffer = new List<byte>(258);

                                writeAddress = currentAddress - i;

                                //Догружаю в буфер оставшиеся байты
                                for (int j = i; j < configurationBytes.Length; j++) {

                                    buffer.Add(configurationBytes[j]);
                                }
                            }

                            //Преверяю наличие разрыва между записанными конфигурационными байтами и следующей строчкой записи
                            if ((configurationAdress + configurationBytes.Length + dataNverAndFrimfareName.Length) != dataInHEX.address) {

                                firmwareData.Add(writeAddress, buffer);

                                buffer = new List<byte>(258);
                                writeAddress = dataInHEX.address;
                                addressOffset = dataInHEX.address + dataInHEX.amountDataByte;
                                buffer.AddRange(dataInHEX.data);

                                continue;
                            }

                            //Выгружаю байты уже считанной строки, которую считал для того, чтобы узнать размер области для записи конфигурации
                            if (buffer.Count + dataInHEX.data.Length <= 256) {

                                buffer.AddRange(dataInHEX.data);

                                //Если в буфере нет места
                            } else {

                                //Создаю индекс для передачи
                                byte i = 0;

                                //Записываю часть, которая влезет в буфер
                                while (buffer.Count < 256) {
                                    buffer.Add(dataInHEX.data[i]);
                                    i++;
                                }

                                //Выгружаю данный буфер в МК
                                firmwareData.Add(writeAddress, buffer);

                                buffer = new List<byte>(258);

                                writeAddress = currentAddress - i;

                                //Догружаю в буфер оставшиеся байты
                                for (int j = i; j < dataInHEX.data.Length; j++) {

                                    buffer.Add(dataInHEX.data[j]);
                                }
                            }

                            //Смещаю адрес на значение конфигурации и считанной строки
                            addressOffset = currentAddress + (uint) (configurationBytes.Length + dataNverAndFrimfareName.Length + dataInHEX.amountDataByte);

                            //Выставляю значения учитывая уже считанную линию
                            currentAddress = dataInHEX.address;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Парсит сконвертированный из HEX строки массив байт в структуру данных
        /// </summary>
        /// <param name="HEX_Line">Линия HEX формата</param>
        /// <returns>Возвращает адрес, колличество байт и сами данные (байты)</returns>
        private DataInHEX ParseByteLineHEX(byte[] HEX_Line) {

            DataInHEX dataInHEX = new DataInHEX();

            byte[] arrAddress = new byte[2];

            Array.Copy(HEX_Line, 1, arrAddress, 0, 2);

            Array.Reverse(arrAddress);

            ushort shorte = BitConverter.ToUInt16(arrAddress, 0);

            dataInHEX.address = startAddressForWrite + shorte;

            dataInHEX.amountDataByte = HEX_Line[0];

            dataInHEX.data = new byte[dataInHEX.amountDataByte];

            for (int i = 4; i < HEX_Line.Length - 1; i++) {

                dataInHEX.data[i - 4] = HEX_Line[i];
            }

            return dataInHEX;
        }

        /// <summary>
        /// Производит запись распарсенных данных из файла в микроконтроллер
        /// </summary>
        private void writeDataInMK() {
            if (firmwareData.Count != 0) {

                //Номер буфера
                int i = 1;

                foreach (KeyValuePair<uint, List<byte>> fwBuff in firmwareData) {
                    
                    uint address = fwBuff.Key;
                    List<byte> buffData = fwBuff.Value;

                    WriteBufferToMK_AndVerify(address, buffData, i);

                    i++;
                }
            }
        }


        /// <summary>
        /// Отправляет буфер данных в МК
        /// </summary>
        /// <param name="address">Адрес куда записать</param>
        /// <param name="buffer">Сами данные (что записывать)</param>
        /// <param name="i">Номер буфера</param>
        private void WriteBufferToMK_AndVerify(uint address, List<byte> buffer, int k) {

            //Даю контроллеру запрос на запись данных
            sendDataInCOM(true, 0x31, 0xCE);

            byte[] addressArr = BitConverter.GetBytes(address);

            Array.Reverse(addressArr);

            byte xorSummAddress = getXORsumm(addressArr);

            //Массив адреса и котрольной суммы
            byte[] addressAndxOR = new byte[addressArr.Length + 1];

            Array.Copy(addressArr, addressAndxOR, addressArr.Length);

            //Добавляю XOR сумму
            addressAndxOR[addressArr.Length] = xorSummAddress;

            Flasher.addMessInLogBuffer("Запрос на запись буфера данных №" + k + " в адрес: " + Convert.ToString(address, 16) + " (hex)");
            //Отправляю запрос на запись в адрес
            sendDataInCOM(true, addressAndxOR);

            List<byte> byteDataOfSend = new List<byte>(buffer);

            //Количество передаваемых байт
            byte amountBytes = ((byte)(buffer.Count - 1));

            byteDataOfSend.Insert(0, amountBytes);

            byte xorSummData = getXORsumm(byteDataOfSend.ToArray());

            byteDataOfSend.Add(xorSummData);

            //Записываю буфер байт в контроллер
            Flasher.addMessInLogBuffer("Запись буфера размером " + buffer.Count + " байт");
            sendDataInCOM(true, byteDataOfSend.ToArray());

            //Получаю байты, которые записались в МК
            Flasher.addMessInLogBuffer("Считывание записанных данных из микроконтроллера и проверка их на корректность");
            byte[] readData = readDataOfMK(address, (byteDataOfSend.Count - 2));

            byte[] writeData = buffer.ToArray();

            //Сверяю данные
            for (int i = 0; i < writeData.Length; i++) {

                if (writeData[i] != readData[i]) throw new MKCommandException("Прочитанные данные из микроконтроллера не совтападют с записанными");

            }

            Flasher.addMessInLogBuffer("Данные успешно записаны" + "\n");
            
            int progBarValue = Flasher.getValueProgressBarFlashingStatic();
            if (progBarValue <= 850) {
                Flasher.setValuePogressBarFlashingStatic(progBarValue + 1);
            }
        }

        /// <summary>
        /// Считывает байты прошивки из контроллера
        /// </summary>
        /// <param name="address">адрес с которого необходимо считать</param>
        /// <param name="amoutByte">количество байт, которые необходимо считать</param>
        private byte[] readDataOfMK(uint address, int amoutByte) {            

            byte[] addressArr = BitConverter.GetBytes(address);

            Array.Reverse(addressArr);

            byte xorSummAddress = getXORsumm(addressArr);

            //Массив адреса и котрольной суммы
            byte[] addressAndxOR = new byte[addressArr.Length + 1];

            Array.Copy(addressArr, addressAndxOR, addressArr.Length);

            //Добавляю XOR сумму
            addressAndxOR[addressArr.Length] = xorSummAddress;

            //Запрашиваю у МК чтение
            sendDataInCOM(true, 0x11, 0xEE);
            //Передаю адрес чтения
            sendDataInCOM(true, addressAndxOR);

            //Запрашиваю возврат байтов находящихся в МК
            return takeDataOfCOM(amoutByte, ((byte) (amoutByte - 1)), ((byte) ~(amoutByte - 1)));
        }

        /// <summary>
        /// Производить полную проверку записанной прошивки в микроконтроллере
        /// </summary>
        private void fullVerificationFirmwareInMK() {

            if (firmwareData.Count != 0) {

                foreach (KeyValuePair<uint, List<byte>> fwBuff in firmwareData) {

                    uint address = fwBuff.Key;
                    List<byte> buffData = fwBuff.Value;

                    Flasher.addMessInLogBuffer("Считывание данных по адресу: " + Convert.ToString(address, 16) + " (hex)");
                    byte[] readBytes = readDataOfMK(address, buffData.Count);                    

                    Flasher.addMessInLogBuffer("Сравнение полученных данных из микроконтроллера, с данными прошивки " + Convert.ToString(address, 16) + " (hex)");
                    for (int i = 0; i < buffData.Count; i++) {
                        if (readBytes[i] != buffData.ElementAt(i)) throw new MKCommandException("Прочитанные данные из микроконтроллера не совтападют с записанными");
                    }

                    Flasher.addMessInLogBuffer("Данные записанны верно" + Environment.NewLine);

                    int progBarValue = Flasher.getValueProgressBarFlashingStatic();
                    if (progBarValue <= 990) {
                        Flasher.setValuePogressBarFlashingStatic(progBarValue + 1);
                    }
                }
            }
        }

        /// <summary>
        /// Конвертирует стринговые байты в числовые
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// Отправялет байты в COM порт
        /// </summary>
        /// <param name="onlyAck">Если нужен только ответ AСK</param>
        /// <param name="bytes">Массив байтов, которые необходимо отправить</param>
        /// <returns></returns>
        private List<byte> sendDataInCOM(bool onlyAck, params byte[] bytes) {

            if (!PortIsOpen()) throw new COMException("COM порт закрыт, откройте порт");
            
            //Данные для отправки в COM порт
            byte[] dataInPort = bytes;
            //Данные из COM порта
            List<byte> dataOutPort = new List<byte>();

            serialPort.Write(dataInPort, 0, dataInPort.Length);

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    int data = serialPort.ReadByte();

                    if (data == NACK) throw new MKCommandException("Не удалось выполнить команду bootloader'a, ответ микроконтроллера NACK");

                    //Если ответ один только ACK
                    if (onlyAck) {
                        if (data == ACK) return dataOutPort;
                    }

                    dataOutPort.Add((byte)data);
                }

                if (dataOutPort.Count() > 1) {
                    if (dataOutPort.ElementAt(dataOutPort.Count - 1) == ACK) {
                        return dataOutPort;
                    }
                }
            }

            throw new COMException("Не удалось получить ответ от микроконтроллера, превышено время ожидания ответа");
        }

        /// <summary>
        /// Отправляет байты и возвращает полный ответ, необходим для считывания байтов с памяти МК
        /// </summary>
        /// <param name="amountByte">Количесто ожидаеммых байтов</param>
        /// <param name="bytes">Количество ожидаемых байтов и байт суммы после операции NOT</param>
        /// <returns></returns>
        private byte[] takeDataOfCOM(int amountByte, params byte[] bytes) {

            if (!PortIsOpen()) throw new COMException("COM порт закрыт, откройте порт");

            //Данные для отправки в COM порт
            byte[] dataInPort = bytes;

            //Данные из COM порта
            List<byte> dataOutPort = new List<byte>(amountByte + 1);

            bool firstByte = false;

            serialPort.Write(dataInPort, 0, dataInPort.Length);

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                while (serialPort.BytesToRead != 0) {

                    int data = serialPort.ReadByte();

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    if (!firstByte) {
                        if (data == NACK) throw new MKCommandException("Не удалось выполнить команду bootloader'a, ответ микроконтроллера NACK");
                        firstByte = true;
                    }

                    dataOutPort.Add((byte)data);

                    if (dataOutPort.Count == amountByte + 1) {

                        byte[] readDataOfMK = new byte[amountByte];

                        Array.Copy(dataOutPort.ToArray(), 1, readDataOfMK, 0, amountByte);

                        return readDataOfMK;
                    }
                }
            }

            throw new COMException("Не удалось получить ответ от микроконтроллера, превышено время ожидания ответа");
        }

        /// <summary>
        /// Вычисляет значение контрольной суммы
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte getXORsumm(params byte[] bytes) {

            if (bytes.Length <= 0) throw new ArithmeticException("Невозможно расчитать контролльную сумму из одного числа");

            byte XORsumm = 0;

            byte a;
            byte b;

            for (int i = 0; i < bytes.Length - 1; i++) {

                if (i != 0) {
                    XORsumm = (byte)(XORsumm ^ bytes[i + 1]);
                } else {
                    a = bytes[i];
                    b = bytes[i + 1];

                    XORsumm = (byte)(a ^ b);
                }
            }

            return XORsumm;
        }

        /// <summary>
        /// Устанавливает конфигурационный файл для прошивки
        /// </summary>
        /// <param name="configuration"></param>
        public void setConfiguration(ConfigurationFW configuration) {
            this.configuration = configuration;
        }
    }
}
