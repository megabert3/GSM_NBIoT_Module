﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.applicationHelper.exceptions {
    class ATCommandException : Exception {

        public ATCommandException() {

        }

        public ATCommandException(string mess) : base(mess) {

        }
    }
}