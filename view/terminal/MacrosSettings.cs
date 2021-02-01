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
        private Terminal terminalForm;

        //Иммитация tgBtn нажата кнопка группы или нет
        private bool firstGroupBtnIsPressed = false;
        private bool secondGroupBtnIsPressed = false;
        private bool thirdGroupBtnIsPressed = false;
        private bool fourthGroupBtnIsPressed = false;
        private bool fifthGroupBtnIsPressed = false;

        //Изменения в макросах до сохранения
        private List<MacrosesGroup> localMacrosesGroup;

        //Цвет кнопки по умолчанию
        private Color defaultBtnColor;
        private Color selectedColorBtn = Color.PaleGreen;

        //Были ли изменения пользователем (предложение сохранить)
        private bool txtBoxTextIsChanges = false;

        public MacrosSettings() {
            InitializeComponent();
        }

        public MacrosSettings(Form terminalForm) {
            InitializeComponent();
            this.terminalForm = terminalForm as Terminal;
            defaultBtnColor = saveBtn.BackColor;
        }

        private void MacrosSettings_Load(object sender, EventArgs e) {

            //Инициализирую начальную версию
            localMacrosesGroup = new List<MacrosesGroup>();

            MacrosesGroupStorage macrosStorage = MacrosesGroupStorage.getMacrosesGroupStorageInstance();

            foreach (MacrosesGroup macrosesGroup in macrosStorage.getMacrosesGroupsList()) {
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

            //Установка слушателей текс боксам и чек боксам таблицы
            foreach (Control crl in macrosTabLotPnl.Controls) {

                if (crl is TextBox) {
                    crl.TextChanged += textBoxDataAndName_TextChaged;

                } else if (crl is CheckBox) {
                    (crl as CheckBox).CheckedChanged += checkBoxRepeat_CheckedChanged;

                }
            }

            //Установка слушателей кнопкам отправки в таблицу
            for (int i = 1; i <= 20; i++) {
                Button btn = macrosTabLotPnl.GetControlFromPosition(4, i) as Button;
                btn.Click += sendDataBtn_Click;
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
        /// <summary>
        /// Сохраняет прошлые изменения в значениях макроса на определённой странице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupBtn_Click(object sender, EventArgs e) {
            try {
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

                setColorSelectedBtn(sender as Button);

            } catch (FormatException ex) {
                Flasher.exceptionDialog(ex.Message);
            }
        }

        /// <summary>
        /// Расскрашивает кнопку, соответствующую выбранной группе макросов
        /// </summary>
        /// <param name="selectedBtn"></param>
        private void setColorSelectedBtn(Button selectedBtn) {

            foreach (Control btn in groupsBtntabLtPnl.Controls) {

                if (btn == selectedBtn) {
                    btn.BackColor = selectedColorBtn;
                } else {
                    btn.BackColor = defaultBtnColor;
                }
            }
        }

        /// <summary>
        /// Сохраняет локальные изменения в макроса при переключении группы макросов пользователем
        /// </summary>
        private void saveOldGroupValues() {
            if (oldGroupIndex != 0) {
                int k;

                localMacrosesGroup.ElementAt(oldGroupIndex - 1).Name = nameGroupTxtBx.Text;

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

                    k = -1;
                    if (int.TryParse(txtBox.Text, out k)) {
                        macros.timeCycle = k;

                    } else {
                        txtBox.Focus();
                        txtBox.SelectAll();
                        throw new FormatException("Неверный формат числа. Значение должно быть целочисленным");
                    }

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
            try {
                saveOldGroupValues();
            } catch (FormatException ex) {
                Flasher.exceptionDialog(ex.Message);
                return;
            }

            MacrosesGroupStorage macrosesGroup = MacrosesGroupStorage.getMacrosesGroupStorageInstance();
            macrosesGroup.setMacrosesGroupsList(localMacrosesGroup);
            MacrosesGroupStorage.serializeMacrosesGroupStorage();
            terminalForm.refreshMacrosBtns();
            terminalForm.refreshToolTipsForMacros();
            txtBoxTextIsChanges = false;
            Close();
        }

        /// <summary>
        /// Действие при изменении текста в текст бокс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDataAndName_TextChaged(object sender, EventArgs e) {
            txtBoxTextIsChanges = true;

            TextBox textBox = sender as TextBox;

            if (textBox.Name.Contains("time")) {

                int value = -1;

                if (!int.TryParse(textBox.Text, out value)) {
                    Flasher.exceptionDialog("Значение должно быть натуральным числом");
                    textBox.Focus();
                    textBox.SelectAll();
                }
            }
        }

        private void checkBoxRepeat_CheckedChanged(object sender, EventArgs e) {
            txtBoxTextIsChanges = true;
        }

        private void MacrosSettings_FormClosing(object sender, FormClosingEventArgs e) {

            if (txtBoxTextIsChanges) {
                bool answer = Flasher.YesOrNoDialog("Сохранить изменения в макросах?", "Сохранение изменений");

                if (answer) {
                    saveBtn.PerformClick();
                }
            }
        }

        private void sendDataBtn_Click(object sender, EventArgs e) {
            int btnIndex = Convert.ToInt32((sender as Button).Name.Substring(1));
            TextBox txtBox = macrosTabLotPnl.GetControlFromPosition(0, btnIndex) as TextBox;
            terminalForm.sendCommandInCOMPort(txtBox.Text);
        }
    }
}