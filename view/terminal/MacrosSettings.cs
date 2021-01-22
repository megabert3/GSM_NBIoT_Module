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
        //Окно терминала
        Terminal terminalForm;

        //Иммитация tgBtn нажата кнопка группы или нет
        private bool firstGroupBtnIsPressed = false;
        private bool secondGroupBtnIsPressed = false;
        private bool thirdGroupBtnIsPressed = false;
        private bool fourthGroupBtnIsPressed = false;
        private bool fifthGroupBtnIsPressed = false;

        private List<MacrosesGroup> localMacrosesGroup;

        public MacrosSettings() {
            InitializeComponent();
        }

        public MacrosSettings(Form terminalForm) {
            InitializeComponent();
            this.terminalForm = terminalForm as Terminal;
        }

        private void MacrosSettings_Load(object sender, EventArgs e) {

            //Инициализирую начальную версию
            localMacrosesGroup = new List<MacrosesGroup>();

            foreach (MacrosesGroup macrosesGroup in MacrosesGroupStorage.getMacrosesGroupStorageInstance().getMacrosesGroupsList()) {
                localMacrosesGroup.Add(macrosesGroup.Clone() as MacrosesGroup);
            }

            //В зависимости от выбранной вкладки устанавливаю таблицу с макросами
            switch (terminalForm.getMacrosPanelIndex()) {
                case 0:
                    firstGroupBtn.PerformClick();
                    break;
                case 1:
                    secondGroupBtn.PerformClick();
                    break;
                case 2:
                    thirdGroupBtn.PerformClick();
                    break;
                case 3:
                    fourthGroupBtn.PerformClick();
                    break;
                case 4:
                    fifthGroupBtn.PerformClick();
                    break;
            }
        }

        private TextBox txtBox;
        private CheckBox checkBox;
        /// <summary>
        /// Данный метод устанавливает все параметры макросов в таблицу, для отображения и редактирования
        /// </summary>
        /// <param name="i">индекс группы</param>
        private void setMacrosInfoInTable(int i) {
            MacrosesGroup macrosesGroup = localMacrosesGroup.ElementAt(i);

            nameGroupTxtBx.Text = macrosesGroup.Name;
            
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

        //Отвечает за индекс группы которая была до того как пользователь решил сменить группу редактирования макросов
        private int oldGroupIndex;
        private void groupBtn_Click(object sender, EventArgs e) {

            if (sender == firstGroupBtn) {

                if (!firstGroupBtnIsPressed) {

                    saveOldGroupValues();
                    oldGroupIndex = 1;

                    firstGroupBtnIsPressed = true;
                    secondGroupBtnIsPressed = false;
                    thirdGroupBtnIsPressed = false;
                    fourthGroupBtnIsPressed = false;
                    fifthGroupBtnIsPressed = false;

                    setMacrosInfoInTable(0);
                }

            } else if (sender == secondGroupBtn) {

                if (!secondGroupBtnIsPressed) {

                    saveOldGroupValues();
                    oldGroupIndex = 2;

                    firstGroupBtnIsPressed = false;
                    secondGroupBtnIsPressed = true;
                    thirdGroupBtnIsPressed = false;
                    fourthGroupBtnIsPressed = false;
                    fifthGroupBtnIsPressed = false;

                    setMacrosInfoInTable(1);
                }

                
            } else if (sender == thirdGroupBtn) {

                if (!thirdGroupBtnIsPressed) {

                    saveOldGroupValues();
                    oldGroupIndex = 3;

                    firstGroupBtnIsPressed = false;
                    secondGroupBtnIsPressed = false;
                    thirdGroupBtnIsPressed = true;
                    fourthGroupBtnIsPressed = false;
                    fifthGroupBtnIsPressed = false;

                    setMacrosInfoInTable(2);
                }
            } else if (sender == fourthGroupBtn) {

                if (!fourthGroupBtnIsPressed) {

                    saveOldGroupValues();
                    oldGroupIndex = 4;

                    firstGroupBtnIsPressed = false;
                    secondGroupBtnIsPressed = false;
                    thirdGroupBtnIsPressed = false;
                    fourthGroupBtnIsPressed = true;
                    fifthGroupBtnIsPressed = false;

                    setMacrosInfoInTable(3);
                }
            } else if (sender == fifthGroupBtn) {

                if (!fifthGroupBtnIsPressed) {

                    saveOldGroupValues();
                    oldGroupIndex = 5;

                    firstGroupBtnIsPressed = false;
                    secondGroupBtnIsPressed = false;
                    thirdGroupBtnIsPressed = false;
                    fourthGroupBtnIsPressed = false;
                    fifthGroupBtnIsPressed = true;

                    setMacrosInfoInTable(4);
                }
            }
        }

        /// <summary>
        /// Сохраняет локальные изменения в макроса при переключении группы макросов пользователем
        /// </summary>
        private void saveOldGroupValues() {
            if (oldGroupIndex != 0) {

                Dictionary<int, Macros> macrosDic = localMacrosesGroup.ElementAt(oldGroupIndex - 1).getMacrosesDic();

                for (int i = 1; i <= 20; i++) {

                    Macros macros = new Macros();

                    //Установка отправляемых данных
                    txtBox = macrosTabLotPnl.GetControlFromPosition(0, i) as TextBox;
                    macros.macrosValue = txtBox.Text;

                    //Установка названия макроса
                    txtBox = macrosTabLotPnl.GetControlFromPosition(1, i) as TextBox;
                    macros.macrosName = txtBox.Text;

                    //Установка времени задержки в цикле для макроса (перед отправкой сообщения)
                    txtBox = macrosTabLotPnl.GetControlFromPosition(2, i) as TextBox;
                    macros.timeCycle = Convert.ToInt32(txtBox.Text);

                    //Установка флага циклической отправки
                    checkBox = macrosTabLotPnl.GetControlFromPosition(3, i) as CheckBox;
                    macros.macrosIncycle = checkBox.Checked;

                    macrosDic[i] = macros;
                }
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e) {
            Close();
        }

        private void SaveBtn_Click(object sender, EventArgs e) {
            MacrosesGroupStorage macrosesGroup = MacrosesGroupStorage.getMacrosesGroupStorageInstance();
            macrosesGroup.setMacrosesGroupsList(localMacrosesGroup);
            MacrosesGroupStorage.serializeMacrosesGroupStorage();
            terminalForm.refreshMacrosBtns();
            Close();
        }
    }
}