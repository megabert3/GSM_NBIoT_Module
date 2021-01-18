using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.applicationHelper.exceptions {
    public class CP_Error : Exception {

        public CP_Error() { }

        public CP_Error(string mess) 
            : base(mess) { }
    }
}
