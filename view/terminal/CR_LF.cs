using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSM_NBIoT_Module.view.terminal {
    public partial class CR_LF : Form {

        Terminal terminalForm;

        public CR_LF(Terminal terminalForm) {
            InitializeComponent();

            this.terminalForm = terminalForm;
        }

        private void Form1_Load(object sender, EventArgs e) {
            CRTxtBx.Text = terminalForm.CR_Byte.ToString();
            LFTxtBx.Text = terminalForm.LF_Byte.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            try {

                terminalForm.CR_Byte = Convert.ToInt32(CRTxtBx.Text);
                terminalForm.LF_Byte = Convert.ToInt32(LFTxtBx.Text);

                if (terminalForm.CR_Byte < 0 || terminalForm.LF_Byte < 0) throw new FormatException();

            } catch (FormatException) {
                Flasher.exceptionDialog("Да введи ты нормальные значения! 0..255");
                return;
            }

            Close();
        }
    }
}
