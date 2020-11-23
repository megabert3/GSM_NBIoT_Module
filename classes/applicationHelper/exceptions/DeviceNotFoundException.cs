using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.applicationHelper.exceptions {

    /// <summary>
    /// Данный класс сообщает об ошибках связанных с поиском модема в списке устройств.
    /// </summary>
    class DeviceNotFoundException : Exception {
        public DeviceNotFoundException() {

        }

        public DeviceNotFoundException(string mess) : base(mess)  {

        }
    }
}
