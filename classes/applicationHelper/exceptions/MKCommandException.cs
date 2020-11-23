using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.applicationHelper.exceptions {
    class MKCommandException : Exception {
        /// <summary>
        /// Данный класс сообщает об ошибках возникающих при работе с микроконтроллером
        /// </summary>
        public MKCommandException() {
        }

        public MKCommandException(string mess) : base(mess) {
        }
    }
}
