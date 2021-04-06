using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.applicationHelper {
    /// <summary>
    /// Класс для работы с форматом IPv6 адреса 
    /// </summary>
    public class IPv6Parser {

        /// <summary>
        /// Проверяет валидность IPv6 адреса
        /// </summary>
        /// <param name="ipv6addresStr"></param>
        public static void checValidValue(string ipv6addresStr) {

            string oc = @"([0-9a-fA-F]{1,4}(?($)(?!:)|:)){8}$";
            string oc2 = @"(::([0-9a-fA-f]{1,4}(?($)(?!:)|:)){1,7})$";
            string oc3 = @"((([0-9a-fA-f]{1,4}):){1,7}:)$";
            string oc4 = @"(([0-9a-fA-F]{1,4}::([0-9a-fA-F]{1,4}(?($)(?!:)|:)){1,6})|(([0-9a-fA-F]{1,4}:){1,2}:([0-9a-fA-F]{1,4}(?($)(?!:)|:)){1,4})|(([0-9a-fA-F]{1,4}:){1,3}:([0-9a-fA-F]{1,4}(?($)(?!:)|:)){1,3})|(([0-9a-fA-F]{1,4}:){1,4}:([0-9a-fA-F]{1,4}(?($)(?!:)|:)){1,2})|(([0-9a-fA-F]{1,4}:){1,5}:([0-9a-fA-F]{1,4}(?($)(?!:)|:)){1,1})|(([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}))";
            string compip = "^(" + oc + "|" + oc2 + "|" + oc3 + "|" + oc4 + ")$";

            if (!Regex.IsMatch(ipv6addresStr, compip)) {
                throw new FormatException("Неверный формат записи IPv6, диапазон каждого значения в одном поле (XXXX) должен быть от 0..F (HEX)" +
                    "\nДопускается сокращённый вид записи" +
                    "\nПримеры записи:" +
                    "\n2001:0DB0:0000:123A:0000:0000:0000:0030" +
                    "\n2001:DB0:0:123A:0:0:0:30" + 
                    "\n2001:DB0:0:123A::30");
            }
        }

        /// <summary>
        /// Возврашщает массив байт из текстового формата
        /// </summary>
        /// <param name="ipv6addresStr"></param>
        /// <returns></returns>
        public static byte[] getIPv6AddresByteArray(string ipv6addresStr) {

            checValidValue(ipv6addresStr);

            string[] ipv6Split = ipv6addresStr.Trim().Split(':');

            //Если сокращенная запись в начале или конце поля, то убираю лишнюю пустую ячейку у сплита
            if (ipv6Split[0] == "") {
                string[] localArr = new string[ipv6Split.Length - 1];
                Array.Copy(ipv6Split, 1 , localArr, 0, ipv6Split.Length - 1);
                ipv6Split = localArr;

            } else if (ipv6Split[ipv6Split.Length - 1] == "") {
                string[] localArr = new string[ipv6Split.Length - 1];
                Array.Copy(ipv6Split, 0, localArr, 0, ipv6Split.Length - 1);
                ipv6Split = localArr;
            }

            //123A:10:20:40::
            string fullIPadress = "";

            for (int i = 0; i < ipv6Split.Length; i++) {
                              
                //Если попадается сокращённая запись, то востанавливаю до полной
                if (String.IsNullOrEmpty(ipv6Split[i])) {

                    //Количество хекстетов для добавления
                    int n = 8 - (ipv6Split.Length - 1);
                    
                    //Добавляю сокращённые хекстеты
                    for (int j = 0; j < n; j++) {
                        fullIPadress += "0000:";
                    }

                } else {
                    fullIPadress += ipv6Split[i] + ":";
                }
            }

            fullIPadress = fullIPadress.Substring(0, fullIPadress.Length - 1);

            List<byte> listByteIPv6 = new List<byte>(16);

            foreach (string cell in fullIPadress.Split(':')) {

                string cell_2_Byte = cell.Trim();

                //В зависимости от длины формата ячейки с адресом заполняю массив байтами
                if (cell_2_Byte.Length <= 2) {
                    listByteIPv6.Add(0);
                    listByteIPv6.Add(Convert.ToByte(cell_2_Byte, 16));

                } else {

                    if (cell_2_Byte.Length == 3) {
                        listByteIPv6.Add(Convert.ToByte(cell_2_Byte.Substring(0, 1), 16));
                        listByteIPv6.Add(Convert.ToByte(cell_2_Byte.Substring(1), 16));

                    } else {
                        listByteIPv6.Add(Convert.ToByte(cell_2_Byte.Substring(0, 2), 16));
                        listByteIPv6.Add(Convert.ToByte(cell_2_Byte.Substring(2), 16));
                    }
                }
            }

            return listByteIPv6.ToArray();
        }
    }
}
