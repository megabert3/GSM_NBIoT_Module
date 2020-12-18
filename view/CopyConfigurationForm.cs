using GSM_NBIoT_Module.classes.controllerOnBoard.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSM_NBIoT_Module.view {
    public partial class CoppyConfForm : Form {

        private ConfigurationFW configuration;

        ConfigurationFrame configurationFrame;

        public CoppyConfForm() {
            InitializeComponent();
        }

        public CoppyConfForm(ConfigurationFW configurationFW, ConfigurationFrame configurationFrame) {
            InitializeComponent();

            this.configuration = configurationFW;

            this.configurationFrame = configurationFrame;
        }

        private void acceptNameBtn_Click(object sender, EventArgs e) {
            
            string nameCopyConfiguration = nameTxtBx.Text;

            if (String.IsNullOrEmpty(nameCopyConfiguration)) {
                Flasher.exceptionDialog("Название конфигурации не может быть пустым");
                return;
            }

            ConfigurationFW cloneConfigurationFW = (ConfigurationFW) configuration.Clone();

            ConfigurationFileStorage configurations = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

            if (configurations.getConfigurationFile(nameCopyConfiguration) != null) {

                string dialogMess = "Конфигурация с именем " + "\"" + nameCopyConfiguration + "\"" + " уже существует в списке конфигураций," +
                        "заменить её копируемой?";

                bool answer = Flasher.YesOrNoDialog(dialogMess, "Замена конфигурации");

                if (answer) {
                    //Конфигурация, которую необходимо заменить
                    ConfigurationFW conf = configurations.getConfigurationFile(nameCopyConfiguration);

                    //Индекс конфигурации в списке
                    int i = configurations.getAllConfigurationFiles().IndexOf(conf);

                    //Замена конфигурации
                    configurations.getAllConfigurationFiles().Insert(i, configuration);

                }

            } else {
                ConfigurationFW copyConf = (ConfigurationFW) configuration.Clone();

                copyConf.setName(nameCopyConfiguration);

                configurations.addConfigurateFileInStorage(copyConf);
            }

            //Сериализую изменения
            ConfigurationFileStorage.serializeConfigurationFileStorage();
            configurationFrame.refreshListView();

            //Обновляю комбобокс с конфигурациями основного окна
            Flasher.refreshConfigurationCmBox();

            Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e) {
            Close();
        }

        private void CoppyConfForm_KeyDown(object sender, KeyEventArgs e) {

            if (e.KeyCode == Keys.Escape) {
                Close();
            }
        }
    }
}
