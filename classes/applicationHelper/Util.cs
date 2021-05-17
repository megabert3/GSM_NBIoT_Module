using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.applicationHelper {

    /// <summary>
    /// Утилитарный класс созданный чтобы помещать сюда часто используемые методы
    /// </summary>
    public class Util {

        /// <summary>
        /// Конвертирует строковые байты в компьтерные
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] StringByteToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

    }
}
