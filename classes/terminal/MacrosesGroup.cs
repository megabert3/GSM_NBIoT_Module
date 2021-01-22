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
    public class MacrosesGroup : ICloneable {

        //Название группы
        private string name = "";

        //Хранилице макросов в группе
        private Dictionary<int, Macros> macrosesDic = new Dictionary<int, Macros>();

        /// <summary>
        /// Класс макроса для быстрой отправки сообщения в COM порт
        /// </summary>
        /// <param name="name"></param>
        public MacrosesGroup(string name) {
            this.name = name;

            //Количество кнопок макросов в окне терминала 20
            for (int i = 1; i <= 20; i++) {

                macrosesDic.Add(i, new Macros(i.ToString(), (i + 10).ToString(), false, 1000));
            }
        }

        public string Name {
            get { return name; } set { name = value; }
        }

        /// <summary>
        /// Класс макроса для быстрой отправки сообщения в COM порт
        /// </summary>        
        [Serializable]
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

            /// <exception cref="ArgumentException()">
            /// Отрицательное значение
            /// </exception>
            public int TimeCycle {
                get {
                    return timeCycle;
                }
                set {
                    if (timeCycle < 0)
                        throw new ArgumentException("Значение времени не может быть отрицательным");
                }
            }
        }

        public Dictionary<int, Macros> getMacrosesDic() {
            return macrosesDic;
        }

        public object Clone() {
            return MemberwiseClone(); ;
        }
    }
}
