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
        public Password() {
            InitializeComponent();
        }

        private void enterPassBtn_Click(object sender, EventArgs e) {

            ConfigurationFileStorage configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

            if (configurationFileStorage.getPass().Equals(PasswordtxtBx.Text)) {
                new ConfigurationFrame().Show();
                this.Close();

            } else {
                Flasher.exceptionDialog("Неверный пароль");
            }
        }
    }
}
