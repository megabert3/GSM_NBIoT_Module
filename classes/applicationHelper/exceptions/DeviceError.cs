using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GSM_NBIoT_Module.classes.applicationHelper {
    /// <summary>
    /// Данный класс сообщает об ошибках возникающих при работе с CP2105
    /// </summary>
    class DeviceError : Exception {
        public DeviceError() { }
        public DeviceError(string mess)
            : base(mess) { }
        
    }
}
