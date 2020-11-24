using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// Абстрактный класс для описания платы модема
    /// </summary>
    public abstract class Board {

        protected string name;

        /// <summary>
        /// Перепрошивает плату модема
        /// </summary>
        public abstract void Reflash();
    }
}
