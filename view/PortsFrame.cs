using GSM_NBIoT_Module.classes;
using GSM_NBIoT_Module.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSM_NBIoT_Module.view {
    /// <summary>
    /// Окно для установки портов стандартного и добавочного портов.
    /// Срабатывает в том случае когда программе не удалось автоматически найти модем.
    /// </summary>
    public partial class PortsFrame : Form {

        CP2105_Connector cp = CP2105_Connector.GetCP2105_ConnectorInstance();

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

            int std;
            int enh;

            string stdStr = standardCmBx.Text;
            string enhStr = enhancedPortCmBx.Text;

            if (String.IsNullOrEmpty(stdStr) || String.IsNullOrEmpty(enhStr)) {
                Flasher.exceptionDialog("Поля: Standard и Enhanced не должны быть пустыми");
                return;
            }

            try {
                std = Convert.ToInt32(stdStr.Trim().Substring(3));
                enh = Convert.ToInt32(enhStr.Trim().Substring(3));

            } catch (FormatException) {
                Flasher.exceptionDialog("Неверный формат записи COM. Значение полей должно иметь формат \"COMXX\", где XX это цифры");
                return;
            }

            if (std < 0 || enh < 0) {
                Flasher.exceptionDialog("Номер порта не может быть отрицательным");
                return;
            }

            if (std == enh) {
                Flasher.exceptionDialog("Номер standard порта не может быть равен номеру enhanced порта");
                return;
            }

            cp.setStandardPort(std);
            cp.setEnhabcedPort(enh);

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
    }
}
