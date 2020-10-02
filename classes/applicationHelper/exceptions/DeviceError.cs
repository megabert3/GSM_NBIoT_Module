using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.applicationHelper {
    class DeviceError : Exception {
        public DeviceError() { }
        public DeviceError(string mess)
            : base(mess) { }
        
    }
}
