using GSM_NBIoT_Module.classes;
using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using GSM_NBIoT_Module.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSM_NBIoT_Module.view {
    /// <summary>
    /// Окно для установки портов стандартного и добавочного портов.
    /// Срабатывает в том случае когда программе не удалось автоматически найти модем.
    /// </summary>
    public partial class PortsFrame : Form {

        CP2105_Connector cp = CP2105_Connector.GetCP2105_ConnectorInstance();

        private bool exc = false;

        public PortsFrame() {
            InitializeComponent();
        }

        private void PortsFrame_Load(object sender, EventArgs e) {

            standardCmBx.Items.AddRange(SerialPort.GetPortNames());
            enhancedPortCmBx.Items.AddRange(SerialPort.GetPortNames());

            standardCmBx.Text = Settings.Default.Standard;
            enhancedPortCmBx.Text = Settings.Default.Enhanced;
        }

        private void acceptBtn_Click(object sender, EventArgs e) {
            
            int std = 0;
            int enh = 0;

            string stdStr = standardCmBx.Text;
            string enhStr = enhancedPortCmBx.Text;

            if (String.IsNullOrEmpty(stdStr) || String.IsNullOrEmpty(enhStr)) {
                Flasher.exceptionDialog("Поля: Standard и Enhanced не должны быть пустыми");
                exc = true;
                return;
            }

            try {
                try {
                    std = Convert.ToInt32(stdStr.Trim().Substring(3));
                    enh = Convert.ToInt32(enhStr.Trim().Substring(3));

                } catch (ArgumentOutOfRangeException) {
                    throw new FormatException();
                }

            } catch (FormatException) {
                Flasher.exceptionDialog("Неверный формат записи COM порта. Значение полей должно иметь формат \"COMXX\", где XX это цифры");
                exc = true;
                return;
            }

            if (std < 0 || enh < 0) {
                Flasher.exceptionDialog("Номер порта не может быть отрицательным");
                exc = true;
                return;
            }

            if (std == enh) {
                Flasher.exceptionDialog("Номер standard порта не может быть равен номеру enhanced порта");
                exc = true;
                return;
            }

            cp.setStandardPort(std);
            cp.setEnhancedPort(enh);

            Settings.Default.Standard = stdStr;
            Settings.Default.Enhanced = enhStr;

            Settings.Default.Save();

            Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e) {
            Close();
        }

        private void refreshComPortsBtn_Click(object sender, EventArgs e) {
            standardCmBx.Items.Clear();
            standardCmBx.Items.AddRange(SerialPort.GetPortNames());
            enhancedPortCmBx.Items.Clear();
            enhancedPortCmBx.Items.AddRange(SerialPort.GetPortNames());
        }

        private void PortsFrame_FormClosing(object sender, FormClosingEventArgs e) {
            if (exc) {
                e.Cancel = true;
                exc = false;
            }
        }
    }
}
