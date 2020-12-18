using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GSM_NBIoT_Module.classes.controllerOnBoard.Configuration;

namespace GSM_NBIoT_Module
{
    public partial class EditConfigurationForm : Form
    {
        private ConfigurationFrame configurationFrame;
        private ConfigurationFW configuration;

        //Это окно вызвано кнопкой "Создать новую конфигурацию?"
        private bool newConfiguration;

        //Подсказка для вывода текста при наведении на строку где прописываются команды для Quectel
        ToolTip quectelCommandTxtBoxToolTip = new ToolTip();

        public EditConfigurationForm() {
            InitializeComponent();
        }

        public EditConfigurationForm(Form configurationFrame, ConfigurationFW configuration, string header, bool newConfiguration) {
            InitializeComponent();

            Text = header;
            this.newConfiguration = newConfiguration;

            this.configurationFrame = (ConfigurationFrame) configurationFrame;
            this.configuration = configuration;

            //Если конфигурация не пустая (вызвана не кнопкой Добавить), то инициализирую поля
            if (!newConfiguration) {
                //Инициализация полей окна для редактирования
                ConfigNameTxtBx.Text = configuration.getName();
                target_IDtxtBox.Text = configuration.getTarget_ID();
                protocol_idTxtBox.Text = configuration.getProtocol_ID();
                indexTxtBox.Text = configuration.getIndex();
                portTxtBox.Text = configuration.getPort();

                if (configuration.isEGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit()) {
                    MCL_chkBox.Checked = true;
                }

                if (configuration.getSelector() > 0) {

                    IPv4rdBtn.Checked = true;

                } else {

                    domenNameRdBtn.Checked = true;
                }

                domenNameTxtBox.Text = configuration.getDomenName();

                pathToFW_MKtxtBx.Text = configuration.getFwForMKName();
                pathToFW_QuectelTxtBx.Text = configuration.getfwForQuectelName();

                foreach (string commandsQuectel in configuration.getQuectelCommandList()) {
                    quectelCommnadsdtGrdView.Rows.Add(commandsQuectel);
                }
            }

            string mess = "Возможен ввод сразу нескольких команд, используйте в качестве разделителя символ \";\"" + "\nПримеры ввода:" + "\nAT+CGSN=0" + "\nAT+CGSN=0; AT+IPR=9600";

            quectelCommandTxtBoxToolTip.InitialDelay = 500;
            quectelCommandTxtBoxToolTip.AutoPopDelay = 6000;
            quectelCommandTxtBoxToolTip.ReshowDelay = 500;

            quectelCommandTxtBoxToolTip.ShowAlways = true;

            quectelCommandTxtBoxToolTip.SetToolTip(addQuectelCommandBtn, mess);
        }

        private void pathToFW_MKBtn_Click(object sender, EventArgs e)
        {
            //Директория где хранятся прошивки для микроконтроллера
            string dirStorageForMKFW = Directory.GetCurrentDirectory() + "\\StorageMKFW";

            //Проверяю существует ли директория
            if (!Directory.Exists(dirStorageForMKFW))
            {
                Directory.CreateDirectory(dirStorageForMKFW);
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = dirStorageForMKFW;
                openFileDialog.Filter = "hex files (*.hex)|*.hex|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    pathToFW_MKtxtBx.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                }
            }
        }

        private void pathToFW_QuectelBtn_Click(object sender, EventArgs e)
        {
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

        private void saveEditsBtn_Click(object sender, EventArgs e) {

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

            ConfigurationFileStorage configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

            //Проверка, что поля заполненны цифрами
            try {
                target_ID = Convert.ToInt32(target_IDtxtBox.Text);
                protocol_ID = Convert.ToInt32(protocol_idTxtBox.Text);
                index = Convert.ToInt32(indexTxtBox.Text);
                port = Convert.ToInt32(portTxtBox.Text);
            } catch (FormatException) {
                Flasher.exceptionDialog("Значение полей: Target_ID, Protocol_ID, Index, Порт, должны быть целочисленными");
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
                if (domenNameArr[0] != '"' || domenNameArr[domenName.Length - 1] != '"') {
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

            //Добавляю конфигурационные команды для модуля Quectel
            List<string> quectelCommands = new List<string>();

            if (quectelCommnadsdtGrdView.Rows.Count > 0) {

                foreach (DataGridViewRow row in quectelCommnadsdtGrdView.Rows) {

                    foreach (DataGridViewCell cell in row.Cells) {

                        quectelCommands.Add(cell.Value.ToString());
                    }
                }
            }

            //Если имя конфигурации не равно старому
            if (!configuration.getName().Equals(name) && !String.IsNullOrWhiteSpace(configuration.getName())) {

                //Проверяю, что в списке нет конфигурации с новым именем
                if (configurationFileStorage.getConfigurationFile(name) != null) {

                    string dialogMess = "Конфигурация с именем " + "\"" + name + "\"" + " уже существует в списке конфигураций," +
                        "заменить её текущей?";

                    bool answer = Flasher.YesOrNoDialog(dialogMess, "Замена конфигурации");

                    if (answer) {
                        configuration = configurationFileStorage.getConfigurationFile(name);
                    } else {
                        return;
                    }

                    //Если в списке нет конфигураций с таким же именем, но имя отличается от старого
                } else {

                    bool answer = Flasher.YesOrNoDialog("Сохранить данную конфигурацию как новую?", "Добавление новой конфигурации");

                    if (answer) {

                        configurationFileStorage.addConfigurateFileInStorage(new ConfigurationFW(name, (byte)target_ID, (byte)index, (byte)protocol_ID, eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit,
                            (ushort)port, selector, domenName, domenNameByteArr, pathToFW_MKtxtBx.Text, pathToFW_QuectelTxtBx.Text, quectelCommands));

                        //Сериализую изменения
                        ConfigurationFileStorage.serializeConfigurationFileStorage();
                        configurationFrame.refreshListView();

                        //Обновляю комбобокс с конфигурациями основного окна
                        Flasher.refreshConfigurationCmBox();

                        ActiveForm.Close();

                        return;
                    }
                }
            }

            //Задаю объекту конфигурации новые параметры
            configuration.setName(name);
            configuration.setTarget_ID((byte)target_ID);
            configuration.setIndex((byte)index);
            configuration.setProtocol_ID((byte)protocol_ID);
            configuration.setEGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit(eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit);
            configuration.setPort((ushort)port);
            configuration.setSelector(selector);
            configuration.setDomenName(domenName);
            configuration.setDomenNameByteArr(domenNameByteArr);
            configuration.setFwForMKName(pathToFW_MKtxtBx.Text);
            configuration.setfwForQuectelName(pathToFW_QuectelTxtBx.Text);

            configuration.setQuectelCommandList(quectelCommands);

            //Если создаётся новая конфигурация
            if (newConfiguration) configurationFileStorage.addConfigurateFileInStorage(configuration);

            //Сериализую изменения
            ConfigurationFileStorage.serializeConfigurationFileStorage();
            configurationFrame.refreshListView();

            //Обновляю комбобокс с конфигурациями основного окна
            Flasher.refreshConfigurationCmBox();

            ActiveForm.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e) {
            Close();
        }

        //------------------------------------------------------------- Quectel commands config
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
        /// Действие при нажатии Enter
        /// Добавляет команду Quectel по нажатию клавиши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditConfigurationForm_KeyDown(object sender, KeyEventArgs e) {
            //Если пользователь нажал кнопку Enter
            if (e.KeyCode == Keys.Enter) {

                //Если выделено поле добавления команды для модуля Quectel
                if (quectelCommandTxtBox.Focused) {
                    addQuectelCommandBtn.PerformClick();
                    return;
                }

                saveEditsBtn.PerformClick();

            } else if (e.KeyCode == Keys.Escape) {
                Close();
            }

        }
    }
}
