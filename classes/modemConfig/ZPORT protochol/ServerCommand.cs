using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.modemConfig.ZPORT_protochol {

    /// <summary>
    /// Команда ZPort'a SERVER проверка вводимых параметров и запись параметров в модем
    /// </summary>
    public class ServerCommand {

        private static string errorMessPortValid = "Значение порта входящего соединения должно быть целочисленным и быть в диапазоне от 1 до 65535";

        public static string checkValidPortParameter(string port) {

            int portValue;

            try {
                portValue = Convert.ToInt32(port);
            }catch (Exception) {
                throw new FormatException(errorMessPortValid);
            }

            if (portValue < 1 || portValue > 65535) throw new FormatException(errorMessPortValid);

            return port.Trim();
        }
    }
}
