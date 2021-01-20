using GSM_NBIoT_Module.classes.terminal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GSM_NBIoT_Module.classes.terminal.MacrosesGroup;

namespace GSM_NBIoT_Module.view.terminal {
    public partial class MacrosSettings : Form {

        //Иммитация tgBtn нажата кнопка группы или нет
        private bool firstGroupBtnIsPressed = false;
        private bool secondGroupBtnIsPressed = false;
        private bool thirdGroupBtnIsPressed = false;
        private bool fourthGroupBtnIsPressed = false;
        private bool fifthGroupBtnIsPressed = false;

        //Получаю файл с макросами
        private MacrosesGroupStorage macroses = MacrosesGroupStorage.getMacrosesGroupStorageInstance();

        public MacrosSettings() {
            InitializeComponent();
        }

        private void MacrosSettings_Load(object sender, EventArgs e) {

        }

        /// <summary>
        /// Данный метод устанавливает все параметры макросов в таблицу, для отображения и редактирования
        /// </summary>
        /// <param name="i">индекс группы</param>
        private void setMacrosInfoInTable(int i) {
            MacrosesGroup macrosesGroup = macroses.getmacrosesGroupsList().ElementAt(i);

            nameGroupTxtBx.Text = macrosesGroup.Name;
            TextBox txtBox;
            CheckBox checkBox;

            foreach (KeyValuePair<int, Macros> macros in macrosesGroup.getMacrosesDic()) {

                Macros macrosInDic = macros.Value;

                //Установка отправляемых данных
                txtBox = macrosTabLotPnl.GetControlFromPosition(0, macros.Key) as TextBox;
                txtBox.Text = macros.Value.macrosValue;

                //Установка названия макроса
                txtBox = macrosTabLotPnl.GetControlFromPosition(1, macros.Key) as TextBox;
                txtBox.Text = macros.Value.macrosName;

                //Установка времени задержки в цикле для макроса (перед отправкой сообщения)
                txtBox = macrosTabLotPnl.GetControlFromPosition(2, macros.Key) as TextBox;
                txtBox.Text = macros.Value.timeCycle.ToString();

                //Установка флага циклической отправки
                checkBox = macrosTabLotPnl.GetControlFromPosition(3, macros.Key) as CheckBox;
                checkBox.Checked = macros.Value.macrosIncycle;
            }
        }

        private void groupBtn_Click(object sender, EventArgs e) {

            if (sender == firstGroupBtn) {

                if (!firstGroupBtnIsPressed) {
                    firstGroupBtnIsPressed = true;
                    secondGroupBtnIsPressed = false;
                    thirdGroupBtnIsPressed = false;
                    fourthGroupBtnIsPressed = false;
                    fifthGroupBtnIsPressed = false;

                    setMacrosInfoInTable(0);
                }

            } else if (sender == secondGroupBtn) {

                if (!secondGroupBtnIsPressed) {

                    firstGroupBtnIsPressed = false;
                    secondGroupBtnIsPressed = true;
                    thirdGroupBtnIsPressed = false;
                    fourthGroupBtnIsPressed = false;
                    fifthGroupBtnIsPressed = false;

                    setMacrosInfoInTable(1);
                }

                
            } else if (sender == thirdGroupBtn) {

                if (!thirdGroupBtnIsPressed) {

                    firstGroupBtnIsPressed = false;
                    secondGroupBtnIsPressed = false;
                    thirdGroupBtnIsPressed = true;
                    fourthGroupBtnIsPressed = false;
                    fifthGroupBtnIsPressed = false;

                    setMacrosInfoInTable(2);
                }
            } else if (sender == fourthGroupBtn) {

                if (!fourthGroupBtnIsPressed) {

                    firstGroupBtnIsPressed = false;
                    secondGroupBtnIsPressed = false;
                    thirdGroupBtnIsPressed = false;
                    fourthGroupBtnIsPressed = true;
                    fifthGroupBtnIsPressed = false;

                    setMacrosInfoInTable(3);
                }
            } else if (sender == fifthGroupBtn) {

                if (!fifthGroupBtnIsPressed) {

                    firstGroupBtnIsPressed = false;
                    secondGroupBtnIsPressed = false;
                    thirdGroupBtnIsPressed = false;
                    fourthGroupBtnIsPressed = false;
                    fifthGroupBtnIsPressed = true;

                    setMacrosInfoInTable(4);
                }
            }
        }
    }
}