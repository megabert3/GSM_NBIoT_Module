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

        public EditConfigurationForm()
        {
            InitializeComponent();
        }

        public EditConfigurationForm(Form configurationFrame, ConfigurationFW configuration)
        {
            InitializeComponent();

            this.configurationFrame = (ConfigurationFrame) configurationFrame;

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

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    pathToFW_MKtxtBx.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                }
            }
        }

        private void pathToFW_QuectelBtn_Click(object sender, EventArgs e)
        {
            //Директория где хранятся прошивки для микроконтроллера
            string dirStorageForQuectelFW = Directory.GetCurrentDirectory() + "\\StorageQuectelFW";

            //Проверяю существует ли директория
            if (!Directory.Exists(dirStorageForQuectelFW))
            {
                Directory.CreateDirectory(dirStorageForQuectelFW);
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = dirStorageForQuectelFW;
                openFileDialog.Filter = "lod files (*.lod)|*.lod|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

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
                            Flasher.exceptionDialog("Неверный формат записи в поле \"Доменное имя\"");
                            return;
                        }

                        //Если символ числовой, то добавляю его к блоку
                    } else if (domenNameArr[i] >= '0' && domenNameArr[i] <= '9') {

                        addressByteBlock += domenNameArr[i];

                    } else if (domenNameArr[i] == ' ') {
                        //Игнорирую все пробелы между цифрами
                    } else {
                        Flasher.exceptionDialog("Неверный формат записи в поле \"Доменное имя\"");
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

                if (domenName.Length > 28) {
                    Flasher.exceptionDialog("Доменное имя не должно быть больше 28 символов");
                    return;
                }

                foreach (char symbol in domenName.ToCharArray()) {
                    if (symbol < 0x20 || symbol > 0x7F) Flasher.exceptionDialog("Доменное имя должно содержать только ASCII символы");
                }

                domenNameByteArr = Encoding.ASCII.GetBytes(domenName);
            }

            //Проверяю состояние
            if (MCL_chkBox.Checked) eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit = true;

            //Создаю объект конфигурации
            ConfigurationFW configurationFW = new ConfigurationFW(name, (byte)target_ID, (byte)index, (byte)protocol_ID,
                eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit, (ushort)port, selector, domenName, domenNameByteArr, pathToFW_MKtxtBx.Text, pathToFW_QuectelTxtBx.Text);

            //Добавляю созданную конфигурацию к остальным
            configurationFileStorage.addConfigurateFileInStorage(configurationFW);

            //Сериализую изменения
            ConfigurationFileStorage.serializeConfigurationFileStorage();
            configurationFrame.refreshListView();

            //Обновляю комбобокс с конфигурациями основного окна
            Flasher.refreshConfigurationCmBox();
        }
    }
}
