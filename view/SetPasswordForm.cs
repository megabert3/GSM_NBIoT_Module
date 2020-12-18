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
    public partial class SetPasswordForm : Form {
        public SetPasswordForm() {
            InitializeComponent();
        }

        private void setPassBtn_Click(object sender, EventArgs e) {
            string repeatPass;
            string newPass;

            ConfigurationFileStorage configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

            //Проверяю установлен ли пароль до этого
            if (String.IsNullOrEmpty(configurationFileStorage.getPass())) {
                newPass = newPassTxtBox.Text;
                repeatPass = repeatNewPassTxtBox.Text;

                if (newPass.Equals(repeatPass)) {
                    configurationFileStorage.setPassword(newPass);

                    ConfigurationFileStorage.serializeConfigurationFileStorage();

                    newPassTxtBox.Text = "";
                    repeatNewPassTxtBox.Text = "";

                    Flasher.successfullyDialog("Пароль установлен", "Пароль");
                } else {
                    Flasher.exceptionDialog("Введённые пароли не совпадают");
                }

                //Если пароль установлен
            } else {

                //Проверяю, что старый пароль введён верно
                if (configurationFileStorage.getPass().Equals(oldPassTxtBox.Text)) {

                    newPass = newPassTxtBox.Text;
                    repeatPass = repeatNewPassTxtBox.Text;

                    if (newPass.Equals(repeatPass)) {
                        configurationFileStorage.setPassword(newPass);

                        ConfigurationFileStorage.serializeConfigurationFileStorage();

                        oldPassTxtBox.Text = "";
                        newPassTxtBox.Text = "";
                        repeatNewPassTxtBox.Text = "";

                        Flasher.successfullyDialog("Пароль установлен", "Пароль");
                    } else {
                        Flasher.exceptionDialog("Введённые пароли не совпадают");
                    }

                } else {
                    Flasher.exceptionDialog("Неверный пароль");
                }
            }
        }

        private void closeBtn_Click(object sender, EventArgs e) {
            Close();
        }

        private void SetPasswordForm_KeyDown(object sender, KeyEventArgs e) {

            if (e.KeyCode == Keys.Escape) Close();
        }
    }
}
