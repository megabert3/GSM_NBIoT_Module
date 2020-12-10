using GSM_NBIoT_Module.classes.controllerOnBoard.Configuration;
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
        ConfigurationFileStorage configurationFileStorage;

        //Подсказка для вывода текста при наведении на строку где прописываются команды для Quectel
        ToolTip quectelCommandTxtBoxToolTip = new ToolTip();

        public ConfigurationFrame() {
            InitializeComponent();
            this.KeyDown += key_Down;
        }

        public ConfigurationFrame(Form mainForm) {
            InitializeComponent();
            this.KeyDown += key_Down;

            this.mainForm = mainForm;
            FormClosing += Form_Closing;

            editConfigurationBtn.Enabled = false;
            deleteConfigurationBtn.Enabled = false;
        }

        //Инициализация окна
        private void ConfigurationFrame_Load(object sender, EventArgs e) {
            refreshListView();

            string mess = "Возможен ввод сразу нескольких команд, используйте в качестве разделителя символ \";\"" + "\nПримеры ввода:" + "\nAT+CGSN=0" + "\nAT+CGSN=0; AT+IPR=9600";

            quectelCommandTxtBoxToolTip.InitialDelay = 2000;
            quectelCommandTxtBoxToolTip.AutoPopDelay = 5000;
            quectelCommandTxtBoxToolTip.ReshowDelay = 500;

            quectelCommandTxtBoxToolTip.ShowAlways = true;

            quectelCommandTxtBoxToolTip.SetToolTip(quectelCommandTxtBox, mess);
        }
        
        /// <summary>
        /// Проверяет заполненные поля и добавляет конфигурацию к имеющимся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addConfigurationBtn_Click(object sender, EventArgs e) {

            //Добавляю конфигурационные команды для модуля Quectel
            List<string> quectelCommands = new List<string>();

            if (quectelCommnadsdtGrdView.Rows.Count > 0) {

                foreach (DataGridViewRow row in quectelCommnadsdtGrdView.Rows) {

                    foreach (DataGridViewCell cell in row.Cells) {

                        quectelCommands.Add(cell.Value.ToString());
                    }
                }

            }

            //Проверка на наличие команд модуля Quectel
            if (quectelCommands.Count == 0) {
                bool answer = Flasher.YesOrNoDialog("Не задано ни одной конфигурационной команды для модуля Quectel, продолжить сохранение конфигурации?", "Конфигурация Quectel");
                if (!answer) return;
            }

            string name;
            int target_ID;
            int index;
            int protocol_ID;
            bool eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit = false;
            int port;
            byte selector = 0;

            string domenName;
            byte[] domenNameByteArr;

            //Проверка, что поля заполнены
            if (String.IsNullOrEmpty(target_IDtxtBox.Text.Trim()) ||
                String.IsNullOrEmpty(protocol_idTxtBox.Text.Trim()) ||
                String.IsNullOrEmpty(indexTxtBox.Text.Trim()) ||
                String.IsNullOrEmpty(portTxtBox.Text.Trim()) ||
                String.IsNullOrEmpty(domenNameTxtBox.Text) ||
                String.IsNullOrEmpty(ConfigNameTxtBx.Text.Trim())) {

                Flasher.exceptionDialog("Поля: Target_ID, Protocol_ID, Index, Порт, Доменное имя, не должны быть пустыми");
                return;
            }

            //Имя конфигурации
            name = ConfigNameTxtBx.Text.Trim();

            //Проверка, что конфигурации с таким же именем больше не существует
            if (configurationFileStorage.getConfigurationFile(name) != null) {
                Flasher.exceptionDialog("Конфигурация с таким именем уже существует");
                return;
            }

            //Проверка, что поля заполненны цифрами
            try {
                target_ID = Convert.ToInt32(target_IDtxtBox.Text);
                protocol_ID = Convert.ToInt32(protocol_idTxtBox.Text);
                index = Convert.ToInt32(indexTxtBox.Text);
                port = Convert.ToInt32(portTxtBox.Text);
            } catch (FormatException) {
                Flasher.exceptionDialog("Значение полей: Target_ID, Protocol_ID, Index, Порт, должны быть численными");
                return;
            }

            //Проверка, что поля заполнены цифрами необходимого диапазона
            try {
                if (target_ID < 0 || target_ID > 255) throw new FormatException("Поле Target_ID должно быть в диапазоне от 0 до 255");
                if (protocol_ID < 0 || protocol_ID > 255) throw new FormatException("Поле Protocol_ID должно быть в диапазоне от 0 до 255");
                if (index < 0 || index > 255) throw new FormatException("Поле Index должно быть в диапазоне от 0 до 255");
                if (port < 0 || port > 65535) throw new FormatException("Поле Порт должно быть в диапазоне от 0 до 65535");

            } catch (FormatException ex) {
                Flasher.exceptionDialog(ex.Message);
                return;
            }

            //Проверка, что имя прошивки для микроконтроллера или для модуля Quectel установлены
            if (String.IsNullOrEmpty(pathToFW_MKtxtBx.Text) && String.IsNullOrEmpty(pathToFW_QuectelTxtBx.Text)) {
                Flasher.exceptionDialog("Необходимо заполнить имя прошивки для микроконтроллера или для модуля Quectel");
                return;
            }

            //Получаю текстовое представление адреса
            domenName = domenNameTxtBox.Text;

            //Если пользователь выбрал адрес в формате IP
            if (IPv4rdBtn.Checked) {

                //Селектор для IPv4
                selector = 0x01;

                //Добавляю точку для парсинга
                domenName = domenName.Trim() + '.';

                //Счётчик для корректного формата данных
                int byteCount = 0;

                //Блоки текстовых цифр для преобразование в байт
                string addressByteBlock = "0";

                //Массив с адресом в байтовом виде (по умолчанию все нули)
                byte[] localDomenNameByteArr = { 0, 0, 0, 0 };

                char[] domenNameArr = domenName.ToCharArray();

                for (int i = 0; i < domenName.Length; i++) {

                    //Если встречаю точку, значит нужно перевести прошлый блок байтов в цифры
                    if (domenNameArr[i] == '.') {

                        //Попытка перевести считаные текстовые байты в байт числовой
                        try {
                            int localByteBlock = Convert.ToInt32(addressByteBlock);
                            //Если полученное число в диапазоне
                            if (localByteBlock < 0 || localByteBlock > 255) throw new FormatException();

                            localDomenNameByteArr[byteCount] = (byte)localByteBlock;

                            //Если уже все 4 блока данных, то выхожу
                            if (byteCount >= 4) break;

                            byteCount++;
                            addressByteBlock = "0";

                        } catch (FormatException) {
                            Flasher.exceptionDialog("Неверный формат записи в поле \"IPv4\"");
                            return;
                        } catch (IndexOutOfRangeException) {
                            Flasher.exceptionDialog("Неверный формат записи в поле \"IPv4\"");
                            return;
                        }

                        //Если символ числовой, то добавляю его к блоку
                    } else if (domenNameArr[i] >= '0' && domenNameArr[i] <= '9') {

                        addressByteBlock += domenNameArr[i];

                    } else if (domenNameArr[i] == ' ') {
                        //Игнорирую все пробелы между цифрами
                    } else {
                        Flasher.exceptionDialog("Неверный формат записи в поле \"IPv4\"");
                        return;
                    }
                }

                //Если пользователь указал адрес по формату, то присваиваю массив байт основному
                domenNameByteArr = localDomenNameByteArr;

                //Формирую string domen name это для того, если пользователь ввёл больше точек
                domenName = localDomenNameByteArr[0].ToString() + '.' +
                    localDomenNameByteArr[1] + '.' +
                    localDomenNameByteArr[2] + '.' +
                    localDomenNameByteArr[3];

                //Если пользователь выбрал адрес в формате доменного имени
            } else {

                char[] domenNameArr = domenName.ToCharArray();
                //Проверка, что доменное имя в кавычках
                if (domenNameArr[0] != '"'|| domenNameArr[domenName.Length - 1] != '"') {
                    Flasher.exceptionDialog("Неверный формат доменного имени. Доменное имя должно иметь знак \" в начале и в конце имени");
                    return;
                }

                //С учётом кавычек
                if (domenName.Length - 2 > 28) {
                    Flasher.exceptionDialog("Доменное имя не должно быть больше 28 символов");
                    return;
                }

                for (int i = 1; i < domenName.Length - 1; i++) {

                    if (domenNameArr[i] < 0x20 || domenNameArr[i] > 0x7F) {
                        Flasher.exceptionDialog("Доменное имя должно содержать только ASCII символы");
                        return;
                    }
                }

                //Сохраняю доменное имя, но уже без кавычек
                domenNameByteArr = Encoding.ASCII.GetBytes(domenName.Substring(1, domenName.Length - 2));
            }

            //Проверяю состояние
            if (MCL_chkBox.Checked) eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit = true;

            //Создаю объект конфигурации
            ConfigurationFW configurationFW = new ConfigurationFW(name, (byte)target_ID, (byte)index, (byte)protocol_ID,
                eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit, (ushort)port, selector, domenName, domenNameByteArr, pathToFW_MKtxtBx.Text, pathToFW_QuectelTxtBx.Text, quectelCommands);

            //Добавляю созданную конфигурацию к остальным
            configurationFileStorage.addConfigurateFileInStorage(configurationFW);

            //Сериализую изменения
            ConfigurationFileStorage.serializeConfigurationFileStorage();
            refreshListView();

            //Обновляю комбобокс с конфигурациями основного окна
            Flasher.refreshConfigurationCmBox();

            //Устанавливаю дефолдные значения полям
            ConfigNameTxtBx.Text = "";
            target_IDtxtBox.Text = "";
            protocol_idTxtBox.Text = "";
            indexTxtBox.Text = "";
            MCL_chkBox.Checked = false;
            portTxtBox.Text = "8103";
            domenNameRdBtn.Checked = true;
            IPv4rdBtn.Checked = false;
            domenNameTxtBox.Text = "\"devices.226.taipit.ru\"";
            pathToFW_MKtxtBx.Text = "";
            pathToFW_QuectelTxtBx.Text = "";
            quectelCommnadsdtGrdView.Rows.Clear();
        }

        /// <summary>
        /// Обновляет таблицу элементов ListView
        /// </summary>
        public void refreshListView() {

            configurationListView.Items.Clear();

            configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

            foreach (ConfigurationFW configuration in configurationFileStorage.getAllConfigurationFiles()) {
                ListViewItem item = new ListViewItem(configuration.getName());

                item.SubItems.Add(configuration.getTarget_ID());
                item.SubItems.Add(configuration.getProtocol_ID());
                item.SubItems.Add(configuration.getIndex());

                if (configuration.isEGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit()) {
                    item.SubItems.Add("Да");
                } else {
                    item.SubItems.Add("Нет");
                }
                item.SubItems.Add(configuration.getPort());
                item.SubItems.Add(configuration.getDomenName());
                item.SubItems.Add(configuration.getFwForMKName());
                item.SubItems.Add(configuration.getfwForQuectelName());

                configurationListView.Items.Add(item);
            }
        }

        private void domenNameRdBtn_CheckedChanged(object sender, EventArgs e) {
            domenNameTxtBox.Text = "\"devices.226.taipit.ru\"";
        }

        private void IPv4rdBtn_CheckedChanged(object sender, EventArgs e) {
            domenNameTxtBox.Text = "XXX.XXX.XXX.XXX";
        }

        private void deleteConfigurationBtn_Click(object sender, EventArgs e) {
            try {
                if (configurationListView.Items.Count > 0) {

                    bool answer = false;

                    string conficurationName = configurationListView.SelectedItems[0].Text;

                    DialogResult res = MessageBox.Show(
                                        "Удалить конфигурацию под названием: " + conficurationName + "?",
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

            } catch(ArgumentOutOfRangeException) {}
        }

        private void setPassBtn_Click(object sender, EventArgs e) {

            string repeatPass;
            string newPass;

            configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

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

        private void pathToFW_MKBtn_Click(object sender, EventArgs e) {

            //Директория где хранятся прошивки для микроконтроллера
            string dirStorageForMKFW = Directory.GetCurrentDirectory() + "\\StorageMKFW";

            //Проверяю существует ли директория
            if (!Directory.Exists(dirStorageForMKFW)) {
                Directory.CreateDirectory(dirStorageForMKFW);
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.InitialDirectory = dirStorageForMKFW;
                openFileDialog.Filter = "hex files (*.hex)|*.hex|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) {

                    pathToFW_MKtxtBx.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                }
            }
        }

        private void pathToFW_QuectelBtn_Click(object sender, EventArgs e) {

            //Директория где хранятся прошивки для микроконтроллера
            string dirStorageForQuectelFW = Directory.GetCurrentDirectory() + "\\StorageQuectelFW";

            //Проверяю существует ли директория
            if (!Directory.Exists(dirStorageForQuectelFW)) {
                Directory.CreateDirectory(dirStorageForQuectelFW);
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.InitialDirectory = dirStorageForQuectelFW;
                openFileDialog.Filter = "lod files (*.lod)|*.lod|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) {

                    pathToFW_QuectelTxtBx.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Действие при нажатии Enter и delete
        /// Удаляет или добавляет конфигурацию по нажатию клавиши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void key_Down(object sender, KeyEventArgs e) {

            //Если пользователь нажал кнопку Enter
            if (e.KeyCode == Keys.Enter) {
                
                //Если выделено поле добавления команды для модуля Quectel
                if (quectelCommandTxtBox.Focused) {
                    addQuectelCommandBtn.PerformClick();
                    return;
                }

                addConfigurationBtn.PerformClick();

            } else if (e.KeyCode == Keys.Delete) {

                if (configurationListView.Focused) {
                    deleteConfigurationBtn.PerformClick();
                }

                if (quectelCommnadsdtGrdView.Focused) {
                    deleteConfCommnadQuectel.PerformClick();
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

            if (configurationListView.Items.Count > 0) {

                configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

                ConfigurationFW configuration;

                try {
                    configuration = configurationFileStorage.getConfigurationFile(configurationListView.SelectedItems[0].Text);

                    new EditConfigurationForm(this, configuration).ShowDialog();

                } catch (ArgumentOutOfRangeException) {
                    Flasher.exceptionDialog("Выберите конфигурацию для редактирования");
                }

            } else {
                Flasher.exceptionDialog("Нет конфигураций для редактирования");
            }
        }

        private void configurationListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            if (configurationListView.SelectedItems.Count != 0) {
                editConfigurationBtn.Enabled = true;
                deleteConfigurationBtn.Enabled = true;
            } else {
                editConfigurationBtn.Enabled = false;
                deleteConfigurationBtn.Enabled = false;
            }
        }

        /// <summary>
        /// Действие при двойном щелчке на конфигурации в таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationListView_MouseDoubleClick(object sender, MouseEventArgs e) {

            if (configurationListView.SelectedItems.Count != 0) {

                configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

                ConfigurationFW configuration;

                try {
                    configuration = configurationFileStorage.getConfigurationFile(configurationListView.SelectedItems[0].Text);

                    if (configuration != null) {
                        new EditConfigurationForm(this, configuration).ShowDialog();
                    } else {
                        Flasher.exceptionDialog("В хранилище не найдено выбранной конфигурации");
                    }

                } catch (ArgumentOutOfRangeException) {
                    Flasher.exceptionDialog("Выберите конфигурацию для редактирования");
                }
            }
        }

        public Button getEditConfigurationBtn () {
            return editConfigurationBtn;
        }

        public Button getdeleteConfigurationBtn () {
            return deleteConfigurationBtn;
        }

        //------------------------------------------------------------- Quectel commands config
        /// <summary>
        /// Действие при нажатии кнопки добавить из окна добавления команды для модуля Quectel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addQuectelCommandBtn_Click(object sender, EventArgs e) {

            //Если текствовое поле не пустое
            if (!String.IsNullOrEmpty(quectelCommandTxtBox.Text.Trim())) {

                //Получаю список команд
                string[] commandsQuectel = quectelCommandTxtBox.Text.Split(';');

                //Добавляю команду в ListView
                foreach (string command in commandsQuectel) {

                    //Если команда не пустая
                    if (!String.IsNullOrEmpty(command.Trim())) {

                        string upCaseCommand = command.Trim().ToUpper();

                        quectelCommnadsdtGrdView.Rows.Add(upCaseCommand);
                       

                        quectelCommandTxtBox.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Действие при выходе из режима редактирования ячейки в quectelCommnadsdtGrdView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quectelCommnadsdtGrdView_CellEndEdit(object sender, DataGridViewCellEventArgs e) {

            //прохожусь по всем строчкам
            foreach (DataGridViewRow row in quectelCommnadsdtGrdView.Rows) {

                foreach (DataGridViewCell cell in row.Cells) {

                    //Если в строке значении ячейки пустое, то удаляю строку

                    if (cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString().Trim())) {
                        quectelCommnadsdtGrdView.Rows.Remove(row);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Действие при нажатии кнопки удалить из окна добавления команды для модуля Quectel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteConfCommnadQuectel_Click(object sender, EventArgs e) {

            //Если есть конфигурация
            if (quectelCommnadsdtGrdView.Rows.Count > 0) {

                //Если есть выделенная конфигурация
                if (quectelCommnadsdtGrdView.SelectedCells.Count > 0) {

                    DataGridViewCell cell = quectelCommnadsdtGrdView.SelectedCells[0];

                    DataGridViewRow row = cell.OwningRow;

                    quectelCommnadsdtGrdView.Rows.Remove(row);
                }
            }
        }

        /// <summary>
        /// Действие при нажатии кнопки "удалить все" из окна добавления команды для модуля Quectel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteAllConfCommnadQuectel_Click(object sender, EventArgs e) {

            if (quectelCommnadsdtGrdView.Rows.Count > 0) {

                bool answer = Flasher.YesOrNoDialog("Вы уверены, что хотите удалить все конфигурационные команды для модуля Quectel?", "Удаление команд Quectel");

                if (answer) {
                    quectelCommnadsdtGrdView.Rows.Clear();
                }
            }
        }

        /// <summary>
        /// Действие при нажатии кнопки "копировать все" из окна добавления команды для модуля Quectel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyAllConfCommnadQuectel_Click(object sender, EventArgs e) {

            if (quectelCommnadsdtGrdView.Rows.Count > 0) {

                StringBuilder quectelCommands = new StringBuilder();

                foreach (DataGridViewRow row in quectelCommnadsdtGrdView.Rows) {

                    foreach (DataGridViewCell cell in row.Cells) {

                        quectelCommands.Append(cell.Value.ToString() + ";");
                    }
                }

                Clipboard.SetDataObject(quectelCommands.ToString());

                Flasher.ShowToolTip("Команды скопированы в буфер." + "\nИспользуйте Ctrl+V чтобы получить их", this, 4000);
            }
        }

        private void quectelCommandTxtBox_MouseLeave(object sender, EventArgs e) {

        }

        private void quectelCommandTxtBox_MouseHover(object sender, EventArgs e) {

        }
    }
}