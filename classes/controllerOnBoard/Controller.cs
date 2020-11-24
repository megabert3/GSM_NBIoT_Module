using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// Абстрактный класс контроллера используемого на плате модема (Class.Board)
    /// </summary>
    public abstract class Controller {

        protected string name;

        /// <summary>
        /// Выполняет полную отчистку памяти микроконтроллера
        /// </summary>
        public abstract void ERASE();

        /// <summary>
        /// Выполняет запись прошивки в микроконтроллер
        /// </summary>
        /// <param name="pathToHex">Путь к файлу с прошивкой</param>
        public abstract void WRITE(string pathToHex);
    }
}
