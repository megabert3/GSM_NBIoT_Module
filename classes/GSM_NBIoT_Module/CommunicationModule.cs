using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// Абстрактный класс модуля связи (GSM, NBIoT...) используемого на плате модема (Class.Board)
    /// </summary>
    public abstract class CommunicationModule {

        //Модель модуля
        protected string name;

        /// <summary>
        /// Перепрошивает модуль Quectel
        /// </summary>
        /// <param name="pathToFirmware">Путь к прошивке модуля</param>
        public abstract void reflashModule(string pathToFirmware);
    }
}