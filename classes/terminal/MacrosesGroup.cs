using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.terminal {

    /// <summary>
    /// Группа макросов для удобного использования макросов
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

                macrosesDic.Add(i, new Macros("", "", false, 1000));
            }
        }

        public string Name {
            get { return name; } set { name = value; }
        }

        public Dictionary<int, Macros> getMacrosesDic() {
            return macrosesDic;
        }

        public object Clone() {
            MacrosesGroup macrosesGroup = new MacrosesGroup(this.name);

            for (int i = 1; i <= 20; i++) {
                Macros localMacros = this.macrosesDic[i];

                Macros cloneMacros = macrosesGroup.getMacrosesDic()[i];

                cloneMacros.MacrosName = localMacros.MacrosName;
                cloneMacros.MacrosValue = localMacros.MacrosValue;
                cloneMacros.MacrosInCycle = localMacros.MacrosInCycle;
                cloneMacros.TimeCycle = localMacros.TimeCycle;
            }

            return macrosesGroup;
        }
    }
}
