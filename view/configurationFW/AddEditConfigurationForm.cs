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

namespace GSM_NBIoT_Module {

    /// <summary>
    /// Окно создания и редактирования конфигурации
    /// </summary>
    public partial class AddEditConfigurationForm : Form {
        private ConfigurationFrame configurationFrame;
        private ConfigurationFW configuration;

        //Это окно вызвано кнопкой "Создать новую конфигурацию?"
        private bool newConfiguration;

        //Подсказка для вывода текста при наведении на строку где прописываются команды для Quectel
        ToolTip addEditConfToolTip = new ToolTip();

        public AddEditConfigurationForm() {
            InitializeComponent();
        }

        public AddEditConfigurationForm(Form configurationFrame, ConfigurationFW configuration, string header, bool newConfiguration) {
            InitializeComponent();

            Text = header;
            this.newConfiguration = newConfiguration;

            this.configurationFrame = (ConfigurationFrame)configurationFrame;
            this.configuration = configuration;

            domenNameRdBtn.CheckedChanged += IPrdBtn_CheckedChanged;
            IPv4rdBtn.CheckedChanged += IPrdBtn_CheckedChanged;
            IPv6rdBtn.CheckedChanged += IPrdBtn_CheckedChanged;

            if (configuration.getQuectelCommandList().Count > 0) {

                copyAllConfCommnadQuectel.Enabled = true;
                deleteAllConfCommnadQuectel.Enabled = true;
                upCommandBtn.Enabled = true;
                downCommandBtn.Enabled = true;
                deleteConfCommnadQuectel.Enabled = true;

            } else {
                copyAllConfCommnadQuectel.Enabled = false;
                deleteAllConfCommnadQuectel.Enabled = false;
                upCommandBtn.Enabled = false;
                downCommandBtn.Enabled = false;
                deleteConfCommnadQuectel.Enabled = false;
            }

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

                switch (configuration.getSelector()) {
                    case 0:
                        domenNameRdBtn.Checked = true;
                        break;
                    case 1:
                        IPv4rdBtn.Checked = true;
                        break;
                    case 2:
                        IPv6rdBtn.Checked = true;
                        break;
                }

                domenNameTxtBox.Text = configuration.getDomenName();

                pathToFW_MKtxtBx.Text = configuration.getFwForMKName();
                pathToFW_QuectelTxtBx.Text = configuration.getfwForQuectelName();

                foreach (string commandsQuectel in configuration.getQuectelCommandList()) {
                    quectelCommnadsdtGrdView.Rows.Add(commandsQuectel);
                }

                portListenTxtBx.Text = configuration.getListenPort().ToString();
                APN_domenName.Text = configuration.getAPN_Name();

            }

            addEditConfToolTip.InitialDelay = 500;
            addEditConfToolTip.AutoPopDelay = 6000;
            addEditConfToolTip.ReshowDelay = 500;

            addEditConfToolTip.ShowAlways = true;

            //Подсказка для кнопки добавить конфигурационные команды для модуля Quectel
            string mess = "Возможен ввод сразу нескольких команд, используйте в качестве разделителя символ \";\"" + "\nПримеры ввода:" + "\nAT+CGSN=0" + "\nAT+CGSN=0; AT+IPR=9600";
            addEditConfToolTip.SetToolTip(addQuectelCommandBtn, mess);

            addEditConfToolTip.SetToolTip(domenNameRdBtn, "Доменное имя должно именть знак \" в начале и в конце.\nДоменное имя должно быть не более 28 символов.\nПример: \"devices.226.taipit.ru\"");

            addEditConfToolTip.SetToolTip(IPv4rdBtn, "Формат \"XXX.XXX.XXX.XXX\", где ХХХ это цифры");
            addEditConfToolTip.SetToolTip(IPv6rdBtn, "Формат \"XXXX:XXXX:XXXX:XXXX:XXXX:XXXX:XXXX:XXXX\", где Х в диапазоне от 0..F (HEX)");

            addEditConfToolTip.SetToolTip(target_IDtxtBox, "Значение должно быть в диапазоне: 0..255");
            addEditConfToolTip.SetToolTip(protocol_idTxtBox, "Значение должно быть в диапазоне: 0..255");
            addEditConfToolTip.SetToolTip(indexTxtBox, "Значение должно быть в диапазоне: 0..255");

            addEditConfToolTip.SetToolTip(portTxtBox, "Значение должно быть в диапазоне: 0..65535");
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

        private void saveEditsBtn_Click(object sender, EventArgs e) {

            string name;
            int target_ID;
            int index;
            int protocol_ID;
            bool eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit = false;
            int port;
            byte selector = 0;

            string domenName;
            byte[] domenNameByteArr = new byte[0];

            int listenPort = 1;
            string APN_DomenName = "";
            byte[] APN_DomenNameByteArr = new byte[0];


            if (String.IsNullOrEmpty(ConfigNameTxtBx.Text.Trim())) {
                Flasher.exceptionDialog("Поле: \"Название конфигурации\" не должно быть пустым");
                return;
            }

            //Проверка, что поля заполнены
            if (String.IsNullOrEmpty(target_IDtxtBox.Text.Trim()) ||
                String.IsNullOrEmpty(protocol_idTxtBox.Text.Trim()) ||
                String.IsNullOrEmpty(indexTxtBox.Text.Trim()) ||
                String.IsNullOrEmpty(portTxtBox.Text.Trim()) ||
                String.IsNullOrEmpty(domenNameTxtBox.Text) ||
                String.IsNullOrEmpty(ConfigNameTxtBx.Text.Trim())) {

                Flasher.exceptionDialog("Поля: Target_ID, Protocol_ID, Index, Порт, Доменное имя / IPv4, не должны быть пустыми");
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

                try {
                    string ipv4 = domenNameTxtBox.Text.Trim();

                    string[] ipv4Arr = ipv4.Split('.');

                    List<byte> byteList = new List<byte>();

                    if (ipv4Arr.Length != 4) throw new FormatException();

                    foreach (string ipNode in ipv4Arr) {
                        if (String.IsNullOrEmpty(ipNode.Trim())) {
                            byteList.Add(0);
                            continue;
                        }

                        int localByte = Convert.ToInt32(ipNode.Trim());

                        if (localByte < 0 || localByte > 255) throw new FormatException();

                        byteList.Add((byte)localByte);
                    }

                    domenNameByteArr = byteList.ToArray();

                    domenName = byteList[0] + "." +
                        byteList[1] + "." +
                        byteList[2] + "." +
                        byteList[3];

                } catch (Exception) {
                    domenNameTxtBox.Focus();
                    domenNameTxtBox.SelectAll();
                    Flasher.exceptionDialog("Неверный формат записи IPv4, формат должен иметь вид xxx.xxx.xxx.xxx, где xxx могут иметь значения от 0..255");
                    return;
                }

                //Селектор для IPv4
                selector = 0x01;

                //Если выбран формат IPv6
            } else if (IPv6rdBtn.Checked) {

                try {
                    string ipv6 = domenNameTxtBox.Text.Trim();

                    string localDomenName = "";

                    string[] ipv6Arr = ipv6.Split(':');

                    List<byte> listByteIPv6 = new List<byte>();

                    if (ipv6Arr.Length != 8) throw new FormatException();

                    foreach (string cell in ipv6Arr) {

                        string cell_2_Byte = cell.Trim();

                        if (cell_2_Byte.Length > 4) throw new FormatException();

                        if (String.IsNullOrEmpty(cell_2_Byte)) {
                            listByteIPv6.AddRange(new byte[] { 0, 0 });
                            localDomenName += "0:";
                            continue;
                        }

                        //В зависимости от длины формата ячейки с адресом заполняю массив байтами
                        if (cell_2_Byte.Length <= 2) {
                            listByteIPv6.Add(0);
                            listByteIPv6.Add(Convert.ToByte(cell_2_Byte, 16));

                        } else {
                            if (cell_2_Byte.Length == 3) {
                                listByteIPv6.Add(Convert.ToByte(cell_2_Byte.Substring(0, 1), 16));
                                listByteIPv6.Add(Convert.ToByte(cell_2_Byte.Substring(1), 16));

                            } else {
                                string messArr = cell_2_Byte.Substring(0, 2);
                                string arr = cell_2_Byte.Substring(2);
                                listByteIPv6.Add(Convert.ToByte(cell_2_Byte.Substring(0, 2), 16));
                                listByteIPv6.Add(Convert.ToByte(cell_2_Byte.Substring(2), 16));
                            }
                        }

                        localDomenName += cell_2_Byte + ":";
                    }

                    //2001:DB0:0:123A:0:0:0:30
                    domenNameByteArr = listByteIPv6.ToArray();
                    domenName = localDomenName.Substring(0, localDomenName.Length - 1);

                    //Селектор для IPv6
                    selector = 0x02;

                } catch (Exception ex) {

                    domenNameTxtBox.Focus();
                    domenNameTxtBox.SelectAll();
                    Flasher.exceptionDialog("Неверный формат записи IPv6, диапазон каждого значения в одном хекстете может быть от 0..F (HEX)" +
                        "\nПример записи: 2001:DB0:0:123A::::30");
                    return;
                }

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

                        if (cell.Value != null) {
                            quectelCommands.Add(cell.Value.ToString());
                        }
                    }
                }
            }

            //Проверка параметров настройки локальной сети
            //Проверка порта
            if (String.IsNullOrEmpty(portListenTxtBx.Text.Trim())) {
                portListenTxtBx.Focus();
                portListenTxtBx.SelectAll();
                Flasher.exceptionDialog("Значение порта локальной сети должно быть целочисленным числом в диапозоне 1..65535");
                return;
            }

            if (int.TryParse(portListenTxtBx.Text.Trim(), out listenPort)) {

                if (listenPort < 1 || listenPort > 65535) {
                    portListenTxtBx.Focus();
                    portListenTxtBx.SelectAll();
                    Flasher.exceptionDialog("значение порта входящего соединения должно быть в диапазоне 1..65535");
                    return;
                }

            } else {
                portListenTxtBx.Focus();
                portListenTxtBx.SelectAll();
                Flasher.exceptionDialog("Значение порта входящего соединения должно быть целочисленным числом");
                return;
            }

            //Если поле доменного имени пустое
            if (String.IsNullOrEmpty(APN_domenName.Text.Trim())) {
                APN_domenName.Focus();
                APN_domenName.SelectAll();
                Flasher.exceptionDialog("Неверный формат APN. APN должно иметь знак \" в начале и в конце имени");
                return;
            }

            APN_DomenName = APN_domenName.Text.Trim();

            char[] APN_DomenNameCharArr = APN_DomenName.ToCharArray();

            //Проверка, что доменное имя в кавычках
            if (APN_DomenNameCharArr[0] != '"' || APN_DomenNameCharArr[APN_DomenNameCharArr.Length - 1] != '"') {
                APN_domenName.Focus();
                APN_domenName.SelectAll();
                Flasher.exceptionDialog("Неверный формат APN. APN должно иметь знак \" в начале и в конце имени");
                return;
            }

            //С учётом кавычек
            if (APN_DomenName.Length - 2 > 28) {
                Flasher.exceptionDialog("Неверный формат APN. APN не должно быть больше 28 символов");
                APN_domenName.Focus();
                APN_domenName.SelectAll();
                return;
            }

            for (int i = 1; i < APN_DomenName.Length - 1; i++) {

                if (APN_DomenNameCharArr[i] < 0x20 || APN_DomenNameCharArr[i] > 0x7F) {
                    Flasher.exceptionDialog("Доменное имя должно содержать только ASCII символы");
                    return;
                }
            }

            //Сохраняю доменное имя, но уже без кавычек
            APN_DomenNameByteArr = Encoding.ASCII.GetBytes(APN_DomenName.Substring(1, APN_DomenName.Length - 2));


            //Если имя конфигурации не равно старому
            if (!configuration.getName().Equals(name) && !String.IsNullOrWhiteSpace(configuration.getName())) {

                //Проверяю, что в списке нет конфигурации с новым именем
                if (configurationFileStorage.getConfigurationFile(name) != null) {

                    string dialogMess = "Конфигурация с именем " + "\"" + name + "\"" + " уже существует в списке конфигураций, " +
                        "заменить её параметры текущими?";

                    bool result = Flasher.YesOrNoDialog(dialogMess, "Замена конфигурации");

                    if (result) {

                        //Удаляю старую конфигурацию
                        configurationFileStorage.removeConfigurateFileInStorage(configurationFileStorage.getConfigurationFile(name));

                        //И добавляю новую после чего заполню её поля текущими данными
                        configuration = new ConfigurationFW();
                        configurationFileStorage.addConfigurateFileInStorage(configuration);

                    } else {
                        return;
                    }

                    //Если в списке нет конфигураций с таким же именем, но имя отличается от старого
                } else {

                    DialogResult answer = Flasher.YesOrNoOrCancelDialog("Было изменено имя конфигурации. Добавить в список новую конфигурацию с указанным именем и текущими параметрами?" +
                        "\n\nДа - добавить новую конфигурацию.\nНет - изменить текущую конфигурацию.\nОтмена - отменить действие.", "Добавление новой конфигурации");

                    if (answer == DialogResult.Yes) {

                        ConfigurationFW configurationFW = new ConfigurationFW(name, (byte)target_ID, (byte)index, (byte)protocol_ID, eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit,
                            (ushort)port, selector, domenName, domenNameByteArr, pathToFW_MKtxtBx.Text, pathToFW_QuectelTxtBx.Text, quectelCommands);

                        configuration.setListenPort((ushort)listenPort);
                        configuration.setAPN_Name(APN_DomenName);
                        configuration.setAPN_NameByteArr(APN_DomenNameByteArr);

                        configurationFileStorage.addConfigurateFileInStorage(configurationFW);

                        //Сериализую изменения
                        ConfigurationFileStorage.serializeConfigurationFileStorage();
                        configurationFrame.setEditebleOrAddedConfName(name);
                        configurationFrame.refreshListView();

                        //Обновляю комбобокс с конфигурациями основного окна
                        Flasher.refreshConfigurationCmBox();

                        ActiveForm.Close();

                        return;

                    } else if (answer == DialogResult.Cancel) {
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

            configuration.setListenPort((ushort)listenPort);
            configuration.setAPN_Name(APN_DomenName);
            configuration.setAPN_NameByteArr(APN_DomenNameByteArr);

            //Если создаётся новая конфигурация
            if (newConfiguration) configurationFileStorage.addConfigurateFileInStorage(configuration);

            //Сериализую изменения
            ConfigurationFileStorage.serializeConfigurationFileStorage();
            configurationFrame.setEditebleOrAddedConfName(name);
            configurationFrame.refreshListView();

            //Обновляю комбобокс с конфигурациями основного окна
            Flasher.refreshConfigurationCmBox();

            Close();
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

                        if (cell.Value != null) {
                            quectelCommands.Append(cell.Value.ToString() + ";");
                        }
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

                    copyAllConfCommnadQuectel.Enabled = false;
                    deleteAllConfCommnadQuectel.Enabled = false;
                    upCommandBtn.Enabled = false;
                    downCommandBtn.Enabled = false;
                    deleteConfCommnadQuectel.Enabled = false;
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

                    if (cell.Value != null) {
                        quectelCommnadsdtGrdView.Rows.Remove(row);
                    }
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

            DataGridViewRow row = quectelCommnadsdtGrdView.Rows[e.RowIndex];
            DataGridViewCell cell = row.Cells[e.ColumnIndex];

            try {
                if (cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString().Trim())) {
                    quectelCommnadsdtGrdView.Rows.Remove(row);

                } else {
                    cell.Value = cell.Value.ToString().ToUpper();
                }
            } catch (InvalidOperationException) { }
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

        private void upCommandBtn_Click(object sender, EventArgs e) {

            if (quectelCommnadsdtGrdView.Rows.Count > 0) {

                if (quectelCommnadsdtGrdView.SelectedRows.Count > 0) {

                    DataGridViewRow selectedRow = quectelCommnadsdtGrdView.SelectedRows[0];

                    int indexRow = selectedRow.Index;

                    try {
                        if (--indexRow >= 0) {

                            quectelCommnadsdtGrdView.Rows.Remove(selectedRow);
                            quectelCommnadsdtGrdView.Rows.Insert(indexRow, selectedRow);
                            selectedRow.Selected = true;
                        }
                    } catch (InvalidOperationException) { }
                }
            }
        }

        private void downCommandBtn_Click(object sender, EventArgs e) {

            if (quectelCommnadsdtGrdView.Rows.Count > 0) {

                if (quectelCommnadsdtGrdView.SelectedRows.Count > 0) {

                    DataGridViewRow selectedRow = quectelCommnadsdtGrdView.SelectedRows[0];

                    int indexRow = selectedRow.Index;

                    try {
                        if (++indexRow < quectelCommnadsdtGrdView.Rows.Count - 1) {
                            quectelCommnadsdtGrdView.Rows.Remove(selectedRow);
                            quectelCommnadsdtGrdView.Rows.Insert(indexRow, selectedRow);
                            selectedRow.Selected = true;
                        }

                    } catch (InvalidOperationException) { }
                }
            }
        }

        private void quectelCommnadsdtGrdView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {

            copyAllConfCommnadQuectel.Enabled = true;
            deleteAllConfCommnadQuectel.Enabled = true;
            upCommandBtn.Enabled = true;
            downCommandBtn.Enabled = true;
            deleteConfCommnadQuectel.Enabled = true;
        }

        private void quectelCommnadsdtGrdView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e) {

            if (quectelCommnadsdtGrdView.Rows.Count == 1) {
                copyAllConfCommnadQuectel.Enabled = false;
                deleteAllConfCommnadQuectel.Enabled = false;
                upCommandBtn.Enabled = false;
                downCommandBtn.Enabled = false;
                deleteConfCommnadQuectel.Enabled = false;
            }
        }

        /// <summary>
        /// Мгновенное редактирование ячейки в командах конфигурации модуля Quectel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quectelCommnadsdtGrdView_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            quectelCommnadsdtGrdView.BeginEdit(true);
        }

        private void domenNameMskTxtBox_Enter(object sender, EventArgs e) {
            MaskedTextBox maskedTextBox = sender as MaskedTextBox;

            //Если поле пустое, то перевожу в начало
            if (!String.IsNullOrEmpty(maskedTextBox.Text)) {
                BeginInvoke((MethodInvoker)delegate () {
                    ((MaskedTextBox)sender).SelectionStart = 0;
                    return;
                });
            }

            int caretIndex = ((MaskedTextBox)sender).Text.IndexOf(' ');

            if (caretIndex != -1) {
                BeginInvoke((MethodInvoker)delegate () {
                    maskedTextBox.SelectionStart = caretIndex;
                });

            } else {
                BeginInvoke((MethodInvoker)delegate () {
                    maskedTextBox.SelectionStart = maskedTextBox.Text.Length;
                });
            }
        }

        /// <summary>
        /// Проверяет правильность введённого формата IPv4 адреса
        /// </summary>
        private void checkAndAddIPv4() {
            string ipv4 = domenNameTxtBox.Text.Trim();

            string[] ipv4Arr = ipv4.Split('.');

            List<byte> byteList = new List<byte>();

            if (ipv4Arr.Length != 4) throw new FormatException();

            foreach (string ipNode in ipv4Arr) {
                if (String.IsNullOrEmpty(ipNode.Trim())) {
                    byteList.Add(0);
                    continue;
                }

                int localByte = Convert.ToInt32(ipNode.Trim());

                if (localByte < 0 || localByte > 255) throw new FormatException();

                byteList.Add((byte)localByte);
            }
        }

        private string domenName = "";
        private string ipv4 = "";
        private string ipv6 = "";
        /// <summary>
        /// Сохраняет предыдущие данные введённые пользователем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void domenNameTxtBox_TextChanged(object sender, EventArgs e) {
            if (domenNameRdBtn.Checked) {
                domenName = domenNameTxtBox.Text;

            } else if (IPv4rdBtn.Checked) {
                ipv4 = domenNameTxtBox.Text;

            } else {
                ipv6 = domenNameTxtBox.Text;
            }
        }

        private void IPrdBtn_CheckedChanged(object sender, EventArgs e) {
            if (sender == domenNameRdBtn) {
                domenNameTxtBox.Text = domenName;
            } else if (sender == IPv4rdBtn) {
                domenNameTxtBox.Text = ipv4;
            } else {
                domenNameTxtBox.Text = ipv6;
            }
        }
    }
}