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

namespace GSM_NBIoT_Module {
    public partial class Password : Form {

        Form mainForm;

        public Password() {
            InitializeComponent();
        }

        public Password(Form mainForm) {
            InitializeComponent();
            this.mainForm = mainForm;
            
        }

        private void enterPassBtn_Click(object sender, EventArgs e) {

            ConfigurationFileStorage configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

            if (configurationFileStorage.getPass().Equals(PasswordtxtBx.Text)) {
                Form configurationFrame = new ConfigurationFrame(mainForm);
                configurationFrame.Show();

                ((Flasher)mainForm).setConfigurationForm(configurationFrame);

                this.Close();

            } else {
                Flasher.exceptionDialog("Неверный пароль");
            }
        }
    }
}
