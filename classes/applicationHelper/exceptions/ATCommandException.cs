using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GSM_NBIoT_Module.classes.applicationHelper.exceptions {
    /// <summary>
    /// Данный класс сообщает об ошибке при отправке AT команд в модуль Quectel.
    /// Если модуль вернул ERROR (команда не поддерживается).
    /// </summary>
    class ATCommandException : Exception {

        public ATCommandException() {

        }

        public ATCommandException(string mess) : base(mess) {

        }
    }
}
