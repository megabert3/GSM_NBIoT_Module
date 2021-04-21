using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.modemConfig {

    /// <summary>
    /// Данный класс отвечает за сохранение скрипта с текстом, который необходим для быстрой конфигурации модема
    /// </summary>
    [Serializable]
    public class ModemConfigScript {

        private string scriptData;

        public ModemConfigScript(string scriptData) {
            this.scriptData = scriptData;
        }

        public string getScriptData() {
            return scriptData;
        }
    }
}
