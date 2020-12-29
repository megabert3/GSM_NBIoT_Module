using GSM_NBIoT_Module.classes.controllerOnBoard.Configuration;
using GSM_NBIoT_Module.view;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GSM_NBIoT_Module {
    public partial class ConfigurationFrame : Form {

        //Ссылка на объект основного окна
        private Form mainForm;

        //Обект с имеющимися конфигурациями
        private ConfigurationFileStorage configurationFileStorage;

        //Подсказка для вывода текста при наведении на строку где прописываются команды для Quectel
        private ToolTip confFormToolTip = new ToolTip();

        //Имя редактируемой или добавляемой конфигурации
        public static string editebleOrAddedConfName = "";

        public ConfigurationFrame() {
            InitializeComponent();
            this.KeyDown += key_Down;
        }

        public ConfigurationFrame(Form mainForm) {
            InitializeComponent();

            KeyDown += key_Down;

            this.mainForm = mainForm;
            FormClosing += Form_Closing;
        }

        //Инициализация окна
        private void ConfigurationFrame_Load(object sender, EventArgs e) {

            editebleOrAddedConfName = Flasher.configurationCmBoxStatic.Text;
            refreshListView();

            //Блокирую все кнопки кроме кнопки "Добавить"
            if (configurationDataGridView.Rows.Count == 0) {

                editConfigurationBtn.Enabled = false;
                copyBtn.Enabled = false;
                deleteConfigurationBtn.Enabled = false;
            }

            confFormToolTip.InitialDelay = 500;
            confFormToolTip.AutoPopDelay = 6000;
            confFormToolTip.ReshowDelay = 500;

            confFormToolTip.ShowAlways = true;

            //Подсказка для кнопки установить пароль
            confFormToolTip.SetToolTip(setPasswordBtn, "Установка пароля доступа к конфигурационному файлу");
            //Подсказка для кнопки добавить
            confFormToolTip.SetToolTip(addConfigurationBtn, "Создать новую конфигурацию для прошивки модема");
            //Подсказка для кнопки редактировать
            confFormToolTip.SetToolTip(editConfigurationBtn, "Редактировать выделенную в таблице конфигурацию");
            //Подсказка для кнопки удалить
            confFormToolTip.SetToolTip(deleteConfigurationBtn, "Удалить выделенную в таблице конфигурацию");

            //настраиваю выравнивание текста для колонок в таблице с конфигурациями
            configurationDataGridView.Columns["terget_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["protocol_id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["index"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["MCL_Mode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["port"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["domenNameOrIP"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["frimwareForMK"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["frimwareForQuectel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            configurationDataGridView.Columns["terget_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["protocol_id"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["index"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["MCL_Mode"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["port"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["domenNameOrIP"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["frimwareForMK"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            configurationDataGridView.Columns["frimwareForQuectel"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        /// <summary>
        /// Проверяет заполненные поля и добавляет конфигурацию к имеющимся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addConfigurationBtn_Click(object sender, EventArgs e) {

            new AddEditConfigurationForm(this, new ConfigurationFW(), "Создание новой конфигурации", true).ShowDialog();
        }

        /// <summary>
        /// Обновляет таблицу элементов согласно конфигурационному файлу
        /// </summary>
        public void refreshListView() {

            configurationDataGridView.Rows.Clear();

            configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

            //Дабовляю строки
            for (int i = 0; i < configurationFileStorage.getAllConfigurationFiles().Count; i++) {
                configurationDataGridView.Rows.Add(new DataGridViewRow());
            }

            //Заполняю строки значениями конфигурации
            for (int i = 0; i < configurationFileStorage.getAllConfigurationFiles().Count; i++) {

                DataGridViewRow row = configurationDataGridView.Rows[i];

                //Имя конфигурации
                row.Cells[0].Value = configurationFileStorage.getAllConfigurationFiles()[i].getName();

                //Выделяю ранее редактируемую строку
                if (row.Cells[0].Value.ToString().Equals(editebleOrAddedConfName)) {
                    configurationDataGridView.Rows[i].Selected = true;
                }

                //Инициализация target_ID
                row.Cells[1].Value = configurationFileStorage.getAllConfigurationFiles()[i].getTarget_ID();

                //protocol_id
                row.Cells[2].Value = configurationFileStorage.getAllConfigurationFiles()[i].getProtocol_ID();

                //Индекс
                row.Cells[3].Value = configurationFileStorage.getAllConfigurationFiles()[i].getIndex();

                //MCL mode
                if (configurationFileStorage.getAllConfigurationFiles()[i].isEGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit()) {
                    row.Cells[4].Value = "Да";
                } else {
                    row.Cells[4].Value = "Нет";
                }

                //Порт
                row.Cells[5].Value = configurationFileStorage.getAllConfigurationFiles()[i].getPort();

                //Доменное имя
                row.Cells[6].Value = configurationFileStorage.getAllConfigurationFiles()[i].getDomenName();

                //Получаю комбо бокс строки
                DataGridViewComboBoxCell configCommandsQuectelComboBoxCell = (DataGridViewComboBoxCell)row.Cells[7];

                //Добавляю в него все команды для модуля Quectel
                configCommandsQuectelComboBoxCell.Items.AddRange(configurationFileStorage.getAllConfigurationFiles()[i].getQuectelCommandList().ToArray());

                //Выставляю первую команду как отображаемую
                if (configCommandsQuectelComboBoxCell.Items.Count > 0) {

                    row.Cells[7].Value = configCommandsQuectelComboBoxCell.Items[0];
                }

                //Название прошивки для МК
                row.Cells[8].Value = configurationFileStorage.getAllConfigurationFiles()[i].getFwForMKName();

                //Название прошивки для модуля Quectel
                row.Cells[9].Value = configurationFileStorage.getAllConfigurationFiles()[i].getfwForQuectelName();
            }
        }

        /// <summary>
        /// Действие при нажатии удалить конфигурацию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteConfigurationBtn_Click(object sender, EventArgs e) {
            try {

                if (configurationDataGridView.Rows.Count > 0) {

                    string conficurationName = configurationDataGridView.SelectedRows[0].Cells[0].Value.ToString();

                    DialogResult res = MessageBox.Show(
                                        "Удалить конфигурацию: " + conficurationName + "?",
                                        "Удаление конфигурации",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button1);

                    if (res == DialogResult.Yes) {
                        configurationFileStorage.removeConfigurateFileInStorage(conficurationName);

                        ConfigurationFileStorage.serializeConfigurationFileStorage();

                        refreshListView();

                        Flasher.refreshConfigurationCmBox();
                    }
                }

            } catch (ArgumentOutOfRangeException) { }
        }

        /// <summary>
        /// Действие при нажатии Enter и delete
        /// Удаляет или добавляет конфигурацию по нажатию клавиши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void key_Down(object sender, KeyEventArgs e) {

            if (e.KeyCode == Keys.Delete) {

                if (configurationDataGridView.Rows.Count > 0) {
                    deleteConfigurationBtn.PerformClick();
                }
            }
        }

        private void Form_Closing(object sender, EventArgs e) {
            ((Flasher)mainForm).setConfigurationForm(null);
        }

        /// <summary>
        /// Действие при нажатии на кнопку редактировать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editConfigurationBtn_Click(object sender, EventArgs e) {

            if (configurationDataGridView.Rows.Count > 0) {

                configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

                ConfigurationFW configuration = configurationFileStorage.getConfigurationFile(configurationDataGridView.SelectedRows[0].Cells[0].Value.ToString());

                new AddEditConfigurationForm(this, configuration, "Редактирование конфигурации", false).ShowDialog();
            }
        }

        public Button getEditConfigurationBtn() {
            return editConfigurationBtn;
        }

        public Button getdeleteConfigurationBtn() {
            return deleteConfigurationBtn;
        }


        /// <summary>
        /// Действие при нажатии двойным кликом по строчке таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {

            DataGridViewRow selectedRow = configurationDataGridView.SelectedRows[0];

            //Если выбрана строчка добавления
            if (selectedRow.Cells[0].Value == null) {
                addConfigurationBtn.PerformClick();
                return;
            }

            configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

            ConfigurationFW configuration = configurationFileStorage.getConfigurationFile(selectedRow.Cells[0].Value.ToString());

            if (configurationDataGridView.Rows.Count > 0) {

                new AddEditConfigurationForm(this, configuration, "Редактирование конфигурации", false).ShowDialog();

            }
        }

        private void setPasswordBtn_Click(object sender, EventArgs e) {

            new SetPasswordForm().ShowDialog();
        }

        private void configurationDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e) {

            if (configurationDataGridView.Rows.Count == 0) {
                editConfigurationBtn.Enabled = false;
                copyBtn.Enabled = false;
                deleteConfigurationBtn.Enabled = false;
            }
        }

        private void configurationDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {

            if (configurationDataGridView.Rows.Count != 0) {
                editConfigurationBtn.Enabled = true;
                copyBtn.Enabled = true;
                deleteConfigurationBtn.Enabled = true;
            }
        }

        private void copyBtn_Click(object sender, EventArgs e) {

            ConfigurationFW selectedConf = ConfigurationFileStorage.GetConfigurationFileStorageInstanse().getConfigurationFile(configurationDataGridView.SelectedRows[0].Cells[0].Value.ToString());

            new CoppyConfForm(selectedConf, this).ShowDialog();
        }

        //Отслеживание последних пользовательских координат мыши по таблице
        int lastRow = -1;
        int lastColl = -1;
        private void configurationDataGridView_CellClick(object sender, DataGridViewCellEventArgs e) {

            //Если выброна строчка добавления, то отключаю кнопки редактировать и удалить
            if (configurationDataGridView.SelectedRows[0].Cells[0].Value == null) {

                editConfigurationBtn.Enabled = false;
                deleteConfigurationBtn.Enabled = false;
            } else {

                editConfigurationBtn.Enabled = true;
                deleteConfigurationBtn.Enabled = true;
            }

            //Раскрытие комбобокса по нажатию мыши
            int indexCmBxColl = configurationDataGridView.Columns["configCommandsQuectel"].Index;

            if (e.ColumnIndex == indexCmBxColl) {

                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)configurationDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (e.RowIndex != lastRow) {

                    if (cell.Items.Count > 0) {
                        configurationDataGridView.BeginEdit(true);
                        SendKeys.Send("{F4}");
                    }

                } else if (e.RowIndex == lastRow && e.ColumnIndex !=  lastColl) {
                                    
                    if (cell.Items.Count > 0) {
                        configurationDataGridView.BeginEdit(true);
                        SendKeys.Send("{F4}");
                    }
                }
            }

            //Смена значения в комбобокса в мэин окне
            if (e.RowIndex != lastRow) {

                if (configurationDataGridView.SelectedRows[0].Cells[0].Value != null)

                Flasher.configurationCmBoxStatic.SelectedItem = configurationDataGridView.SelectedRows[0].Cells[0].Value.ToString();
            }

            lastRow = e.RowIndex;
            lastColl = e.ColumnIndex;
        }

        public void setEditebleOrAddedConfName(String confName) {
            editebleOrAddedConfName = confName;
        }

        public DataGridView getConfigurationDataGridView() {
            return configurationDataGridView;
        }
    }
}