﻿using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// Контроллер STM32L412CB выполнеяет функцию обработки полученных данных с модуля
    ///https://www.st.com/en/microcontrollers-microprocessors/stm32l412cb.html
    /// </summary>
    public class STM32L412CB_Controller : Controller {

        public STM32L412CB_Controller() {
            base.name = "STM32L412CB";
        }

        private CP2105_Connector CP2105_Connector = CP2105_Connector.GetCP2105_ConnectorInstance();

        private SerialPort serialPort = new SerialPort();
        private int timeOut = 5000;

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

        private const uint startAddressForWrite = 0x08000000;

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
            public bool READOUT_UNPROTECT_CMD; //Disable the readout protection in a temporary way
        }

        /// <summary>
        /// Открывает COM порт с параметрами
        /// </summary>
        /// <param name="NmbPort"></param>
        /// <param name="bandRate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
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

        public void ClosePort() {
            serialPort.Close();
        }

        /// <summary>
        /// Получает список доступных команд для Контроллера
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

            List<byte> answer = sendDataInCOM(false, 0x01, 0xFE);

            this.verBootLoader = answer.ElementAt(1).ToString();

            protectedReadWriteEnabled = answer.ElementAt(2);

            protectedReadWriteDisabled = answer.ElementAt(3);

            Console.WriteLine(protectedReadWriteEnabled);
            Console.WriteLine(protectedReadWriteDisabled);
        }

        public void GET_ID() {

            List<byte> answer = sendDataInCOM(false, 0x02, 0xFD);

            LSB = answer.ElementAt(3);
        }

        /// <summary>
        /// Снимает защиту от чтения флэш памяти 
        /// ВНИМАНИЕ! При снятии зашиты от чтения вся флэш память отчищается
        /// </summary>
        public void READOUT_UNPROTECT() {
            sendDataInCOM(true, 0x92, 0x6D);
        }

        /// <summary>
        /// Устанавливает защиту от чтения 
        /// ВНИМАНИЕ! Перед тем как установить защиту необходимо учесть, то, что чтобы её в дальнейшем снять удалятся все данные с флеш памяти контроллера
        /// </summary>
        public void READOUT_PROTECT() {
            sendDataInCOM(true, 0x82, 0x7D);
        }

        /// <summary>
        /// Защита от записи
        /// </summary>
        public void WRITE_PROTECT() {
            sendDataInCOM(true, 0x63, 0x9C);
        }

        /// <summary>
        /// Отключение защиты от записи
        /// </summary>
        public void WRITE_UNPROTECT() {
            List<byte> ansver = sendDataInCOM(true, 0x73, 0x8C);

            Console.WriteLine(BitConverter.ToString(ansver.ToArray()));
        }

        /// <summary>
        /// Отчистка памяти контроллера (удаление старой прошивки)
        /// </summary>
        public void ERASE() {
            //Отправляю контроллеру заброс на команду отчистки
            List<byte> answer = sendDataInCOM(true, 0x44, 0xBB);

            Console.WriteLine(BitConverter.ToString(answer.ToArray()));

            //Удаляю всё из всех секторов
            answer = sendDataInCOM(true, 0xFF, 0xFF, 0x00);

            Console.WriteLine(BitConverter.ToString(answer.ToArray()));
        }

        public void INIT() {
            sendDataInCOM(false, 0x7F);
        }

        public void GO() {
            sendDataInCOM(true, 0x21, 0xDE);
            sendDataInCOM(true, 0x08, 0x00, 0x00, 0x00, 0x08);
        }


        //Структура, необходимая для возврата данных из распарсенной строчки HEX файла
        struct DataInHEX {
            //Адрес данных в строке
            public uint address;
            //Размер данных
            public byte amountDataByte;
            //Сами данные
            public byte[] data;
        }

        public void WRITE(string pathToHex) {
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

            List<byte> buffer = new List<byte>(256);

            //Окрываю файл прошивки и считываю с него данные
            using (StreamReader hexFileSream = new StreamReader(pathToHex)) {
                int k = 0;
                while(true) {

                    Console.WriteLine(k);

                    if (buffer.Count == 256) {

                        WriteBufferToMK(writeAddress, buffer);

                        writeAddress += 256;

                        buffer = new List<byte>(256);
                    }
                                        
                    line = hexFileSream.ReadLine();

                    //Если конец файла, то выхожу
                    if (line == null) {
                        //Если в буфере остались незаписанные данные flush
                        if (buffer.Count != 0) WriteBufferToMK(writeAddress, buffer);

                        Console.WriteLine("Конец");
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
                                WriteBufferToMK(writeAddress, buffer);

                                buffer = new List<byte>(256);

                                writeAddress = currentAddress - i;

                                //Догружаю в буфер оставшиеся байты
                                for (int j = i; j < dataInHEX.data.Length; j++) {

                                    buffer.Add(dataInHEX.data[j]);
                                }
                            }
                        // Если предполагаемый адрес не равен текущему адресу (Разрыв)
                        } else {

                            //Выгружаю всё что было до этого в буфере в контроллер
                            WriteBufferToMK(writeAddress, buffer);

                            buffer = new List<byte>(256);
                            writeAddress = dataInHEX.address;
                            addressOffset = dataInHEX.address + dataInHEX.amountDataByte;
                            buffer.AddRange(dataInHEX.data);
                        }
                    }
                    k++;
                }
            }
        }

        /// <summary>
        /// Парсит получаемую строку в структуру данны
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
       /// Отправляет буфер данных в МК
       /// </summary>
       /// <param name="address">Адрес куда записать</param>
       /// <param name="buffer">Сами данные (что записывать)</param>
        private void WriteBufferToMK(uint address, List<byte> buffer) {

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

            //Отправляю запрос на запись в адрес
            sendDataInCOM(true, addressAndxOR);

            //Количество передаваемых байт
            byte amountBytes = (byte)((byte) buffer.Count - 1);

            buffer.Insert(0, amountBytes);

            byte xorSummData = getXORsumm(buffer.ToArray());

            buffer.Add(xorSummData);

            //Отправляю запрос на запись данных
            sendDataInCOM(true, buffer.ToArray());
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
        public List<byte> sendDataInCOM(bool onlyAck, params byte[] bytes) {

            if (!PortIsOpen()) throw new COMException("COM порт закрыт, откройте порт");
            
            //Данные для отправки в COM порт
            byte[] dataInPort = bytes;
            //Данные из COM порта
            List<byte> dataOutPort = new List<byte>();

            serialPort.Write(dataInPort, 0, dataInPort.Length);

            int timeOut = this.timeOut - 100;

            Thread.Sleep(100);

            while (timeOut > 0) {

                Thread.Sleep(100);

                while (serialPort.BytesToRead != 0) {

                    int data = serialPort.ReadByte();

                    if (data == NACK) throw new MKCommandException();

                    dataOutPort.Add((byte)data);

                    //Если ответ один только ACK
                    if (onlyAck) {
                        if (data == ACK) return dataOutPort;
                    }
                }

                if (dataOutPort.Count() > 1) {
                    if (dataOutPort.ElementAt(dataOutPort.Count - 1) == ACK) {
                        return dataOutPort;
                    }
                }

                timeOut -= 100;
            }

            throw new COMException("Не удалось получить ответ от микроконтроллера");
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
    }
}