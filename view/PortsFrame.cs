using GSM_NBIoT_Module.classes;
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
    public partial class PortsFrame : Form {

        CP2105_Connector cp = CP2105_Connector.GetCP2105_ConnectorInstance();

        public PortsFrame() {
            InitializeComponent();
        }

        private void acceptBtn_Click(object sender, EventArgs e) {

            int std;
            int enh;

            try {
                std = Convert.ToInt32(standartTxtBx.Text);
                enh = Convert.ToInt32(enhancedPortTxtBx.Text);

            } catch (FormatException) {
                Flasher.exceptionDialog("Значение полей должно быть численным");
                return;
            }

            if (std < 0 || enh < 0) {
                Flasher.exceptionDialog("Номер порта не может быть отрицательным");
                return;
            }

            if (std == enh) {
                Flasher.exceptionDialog("Номер standart порта не может быть равен номеру enhanced порта");
                return;
            }

            cp.setStandartPort(std);
            cp.setEnhabcedPort(enh);

            Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
