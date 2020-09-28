using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes {

    /// <summary>
    /// Контроллер STM32L412CB выполнеяет функцию обработки полученных данных с модуля
    ///https://www.st.com/en/microcontrollers-microprocessors/stm32l412cb.html
    /// </summary>
    class STM32L412CB_Controller : Controller{

        public STM32L412CB_Controller() {
            base.name = "STM32L412CB";
        }
    }
}
