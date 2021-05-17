using GSM_NBIoT_Module.classes.applicationHelper;
using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.modemConfig.ZPORT_protochol {

    /// <summary>
    /// Команда ZPort'a SERVER проверка вводимых параметров и запись параметров в модем
    /// </summary>
    public class ServerCommand {

        //Таймаут ожидания ответа из COM порта
        private static int timeOut = 1500;

        private static string errorMessPortValid = "Значение порта входящего соединения должно быть целочисленным и быть в диапазоне от 1 до 65535";

        /// <summary>
        /// Проверяет парамеры задания порта входящего соединения
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string checkValidPortParameter(string port) {

            int portValue;

            try {
                portValue = Convert.ToInt32(port);
            } catch (Exception) {
                throw new FormatException(errorMessPortValid);
            }

            if (portValue < 1 || portValue > 65535) throw new FormatException(errorMessPortValid);

            return port.Trim();
        }

        /// <summary>
        /// Подготавливает данные для отправки в COM для параметра CMDKEY
        /// </summary>
        /// <param name="CMDKEYvalue"></param>
        /// <returns></returns>
        public static byte[] CMDKEYParamByte(byte[] CMDKEYvalue) {
            List<byte> dataToCOM = new List<byte>();
            /* * – первый символ (начало литерала) и последний символ (конец 
                   литерала) должны быть ASCII ‘”’ (двойные кавычки) ;
                   внутри литерала могут быть любые символы (октеты), однако 
                   символы ‘”’, ‘\’ и непечатные символы (диапазон 00hex - 1Fhex) 
                   должны экранироваться символом ‘\’, т.е. заменяться на пары символов 
               \” , \\  и \0 - \O соответственно */

            if (CMDKEYvalue.Length < 4 || CMDKEYvalue.Length > 12)
                throw new FormatException("Длинна ключа должна быть не меньше 4 и не больше 12");

            foreach (byte b in CMDKEYvalue) {
                if (b >= 00 && b <= 31 ||
                    b == 55 ||
                    b == 92) {

                    dataToCOM.AddRange(new byte[] { 92, b });

                } else {
                    dataToCOM.Add(b);
                }
            }

            return dataToCOM.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port">Слушающий порт</param>
        /// <param name="CMDKEY">сообщает или задаёт ключ перевода входящего соединения из прозрачного режима в режим обмена по протоколу MOST</param>
        /// <param name="IP">сообщает или задаёт приоритет типа IP-адреса сервера</param>
        public static void writeServerParams(string port, byte[] CMDKEY, string IP, SerialPort serialPort) {
            /* 12:34:58.732 << SERVER PORT=15; CMDKEY="abc:"; IP=IPV6;
               12:34:58.742 >> Zc) SERVER PORT=13; CMDKEY="89\=\:"; IP=IPV6;
               12:34:58.782 >> Za) SERVER (PORT:13) (CMDKEY:"89\=\:") (IP:IPV6) OK
            */

            List<byte> dataToCOM = new List<byte>(80);

            dataToCOM.AddRange(Encoding.ASCII.GetBytes("SERVER PORT=" + port + "; CMDKEY=\""));
            dataToCOM.AddRange(CMDKEY);
            dataToCOM.AddRange(Encoding.ASCII.GetBytes("\"; IP=" + IP));
            dataToCOM.AddRange(new byte[] { 13, 10 });

            if (!serialPort.IsOpen) serialPort.Open();

            serialPort.Write(dataToCOM.ToArray(), 0, dataToCOM.Count);

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line;

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    line = serialPort.ReadLine();

                    if (line.Contains("Za) SERVER") && line.EndsWith("OK")) {
                        serialPort.Close();
                        return;

                    } else if (line.Contains("Za) SERVER") && line.Contains("ERROR")) {
                        serialPort.Close();
                        throw new MKCommandException("Ответ микроконтроллера на команду \"SERVER\"\n" + line);
                    }
                }
            }

            serialPort.Close();
            throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
        }

        public static Dictionary<string, string> readServerCommnadParams(SerialPort serialPort) {

            if (!serialPort.IsOpen) serialPort.Open();

            serialPort.WriteLine("SERVER");

            long endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;
            string line = "";

            List<byte> dataOutCOM = new List<byte>(80);

            //Пока не вышло время по таймауту
            while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < endReadTime) {

                //Если данные пришли в порт
                while (serialPort.BytesToRead != 0) {

                    //Обновляю таймаут
                    endReadTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + timeOut;

                    byte dataByte = (byte)serialPort.ReadByte();

                    if (dataByte == 13) {

                        /*
                        14:42:50.513 >> Zc) SERVER
                        14:42:50.544 >> Za) SERVER (PORT:15) (CMDKEY:"abcd") (IP:IPV6) (CONTEXT:BASE) OK
                        */
                        line = Encoding.ASCII.GetString(dataOutCOM.ToArray());

                        if (!line.EndsWith("OK")) continue;

                        string[] lineArr = line.Split('(');

                        string port = lineArr[1].Substring(lineArr[1].IndexOf(':') + 1, lineArr[1].IndexOf(')') - (lineArr[1].IndexOf(':') + 1));

                        string ip = lineArr[3].Substring(lineArr[3].IndexOf(':') + 1, lineArr[3].IndexOf(')') - (lineArr[3].IndexOf(':') + 1));
                        string contex = lineArr[4].Substring(lineArr[4].IndexOf(':') + 1, lineArr[4].IndexOf(')') - (lineArr[4].IndexOf(':') + 1));

                        serialPort.Close();

                        return new Dictionary<string, string> {
                            {"PORT", port },
                            {"CMDKEY", getCMDKEYBytesStr(dataOutCOM)},
                            {"IP", ip},
                            { "CONTEXT", contex }
                        };

                    } else if (line.Contains("Za) SERVER") && line.Contains("ERROR")) {
                        serialPort.Close();
                        throw new MKCommandException("Ответ микроконтроллера на команду \"SERVER\"\n" + line);
                    }

                    dataOutCOM.Add(dataByte);
                }
            }

            serialPort.Close();
            throw new TimeoutException("Превышено время ожидания ответа от микроконтроллера");
        }

        /// <summary>
        /// Возвращает значение параметра CMDKEY команды SERVER
        /// </summary>
        /// <param name="serverCommandData"></param>
        /// <returns></returns>
        private static string getCMDKEYBytesStr(List<byte> serverCommandData) {
            //Вычленяю CMDKEY
            int left = 0;
            int right = 0;

            //Нахожу первую кавычку сообщения
            for (int i = 0; i < serverCommandData.Count; i++) {
                if (serverCommandData.ElementAt(i) == 34) {
                    left = i + 1;
                    break;
                }
            }

            //Нахожу последнюю кавычку сообщения
            for (int j = serverCommandData.Count - 1; j > 0; j--) {
                if (serverCommandData.ElementAt(j) == 34) {
                    right = j;
                    break;
                }
            }

            byte[] CMDKEY_dataBytes = new byte[right - left];

            //Получаю необходимые байты
            Array.Copy(serverCommandData.ToArray(), left, CMDKEY_dataBytes, 0, right - left);

            List<byte> CMDKEYwithoutEscaping = new List<byte>();

            //Убираю все экранирующие знаки "/"
            for (int i = 0; i < CMDKEY_dataBytes.Length; i++) {

                try {
                    if (CMDKEY_dataBytes[i] == 47 && CMDKEY_dataBytes[i + 1] != 47) {
                        continue;
                    }
                } catch (IndexOutOfRangeException) { }

                CMDKEYwithoutEscaping.Add(CMDKEY_dataBytes[i]);
            }

            return string.Join(" ", CMDKEYwithoutEscaping.Select(i => i.ToString("X2"))); ;
        }
    }
}
