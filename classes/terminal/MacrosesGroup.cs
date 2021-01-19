using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.terminal {

    /// <summary>
    /// Класс макроса для быстрой отправки сообщения в COM порт
    /// </summary>
    [Serializable]
    public class MacrosesGroup {

        //Название группы
        private string name = "";

        //Хранилице макросов в группе
        private Dictionary<int, Macros> macrosesDic = new Dictionary<int, Macros>();

        public MacrosesGroup(string name) {
            this.name = name;

            //Количество кнопок макросов в окне терминала 20
            for (int i = 1; i <= 20; i++) {

                macrosesDic.Add(1, new Macros("", "", false, 1000));
            }
        }

        public string Name { get; set; }

        /// <summary>
        /// Структура отдельного макроса (для быстрой отправки команды в COM порт)
        /// </summary>
        public struct Macros {
            public string macrosName;
            public string macrosValue;
            public bool macrosIncycle;
            public int timeCycle;

            public Macros(string macrosName, string macrosValue, bool macrosIncycle, int timeCycle) {
                this.macrosName = macrosName;
                this.macrosValue = macrosValue;
                this.macrosIncycle = macrosIncycle;
                this.timeCycle = timeCycle;
            }
        }
    }
}
