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

            FormClosing += Form_Closing;
        }

        public Password(Form mainForm) {
            InitializeComponent();
            this.mainForm = mainForm;

            FormClosing += Form_Closing;
        }

        private void enterPassBtn_Click(object sender, EventArgs e) {

            ConfigurationFileStorage configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

            if (configurationFileStorage.getPass().Equals(PasswordtxtBx.Text)) {

                Form configurationFrame = new ConfigurationFrame(mainForm);
                ((Flasher)mainForm).setConfigurationForm(configurationFrame);

                configurationFrame.Show();

                this.Close();

            } else {
                Flasher.exceptionDialog("Неверный пароль");
            }
        }

        private void Form_Closing(object sender, EventArgs e) {
            ((Flasher)mainForm).setPassForm(null);
        }
    }
}
