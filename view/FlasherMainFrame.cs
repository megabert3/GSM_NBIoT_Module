using GSM_NBIoT_Module.classes;
using GSM_NBIoT_Module.classes.applicationHelper;
using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using GSM_NBIoT_Module.classes.controllerOnBoard.Configuration;
using GSM_NBIoT_Module.Properties;
using GSM_NBIoT_Module.view;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace GSM_NBIoT_Module {
    public partial class Flasher : Form {
        public Flasher() {
            InitializeComponent();
        }

        //Ссылка на форму с конфигурацией
        private Form configurationForm;

        private Form passForm;

        private Thread flashThread;

        //Типы используемых модемов
        private string[] modemType = { "GSM3" };

        private static ProgressBar progressBarFlashingStatic;

        private static RichTextBox flashProcessTxtBoxStatic;

        public static ComboBox configurationCmBoxStatic;

        private static TextBox configurationTextBoxStatic;

        public static Stopwatch firmwareWriteStart = new Stopwatch();

        private static Form mainFrame;

        //Буфер сообщений для перепрошивки микроконтроллера
        private static StringBuilder logBuffer;

        //Буфер для хранения информации о конфигурации модуля Quectel
        public static StringBuilder confCommandsAndAnswerQuectel;

        private void Flasher_Load(object sender, EventArgs e) {
            //Устанавливаю статическому полю ссылку на основное окно Лога (Для статического доступа к окну поля из всей программы)
            flashProcessTxtBoxStatic = flashProcessRichTxtBox;
            mainFrame = this;

            Text = "Тайпит Flasher " + Settings.Default.version;

            //Установка подсказки
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 1000;
            toolTip.AutoPopDelay = 5000;
            toolTip.ReshowDelay = 500;

            toolTip.ShowAlways = true;

            toolTip.SetToolTip(saveLogBtn, "Сохранить лог");

            progressBarFlashingStatic = progressBarFlashing;

            //Задаю стиль прогресс бара
            progressBar.SetState(progressBarFlashing, 1);

            //Выгрузка типов модемов
            modemTypeCmBox.Items.AddRange(modemType);
            modemTypeCmBox.SelectedIndex = 0;

            configurationCmBoxStatic = configurationCmBox;

            configurationTextBoxStatic = configurationTextBox;

            loadConfigurationCmBx();
        }

        //Использовалось в первой версии программы, сохранено на всякий случай!!!!!!!!!!!!!!!!
        private void pathToQuectelFirmwareBtn_Click(object sender, EventArgs e) {

            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "lod files (*.lod)|*.lod|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) {

                    pathToQuectelFirmwareTextBox.Text = openFileDialog.FileName;
                }
            }
        }

        private void pathToSTM32FirmwareBtn_Click(object sender, EventArgs e) {

            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "hex files (*.hex)|*.hex|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) {

                    pathToSTM32FirmwareTextBox.Text = openFileDialog.FileName;
                }
            }
        }
        //Использовалось в первой версии программы, сохранено на всякий случай!!!!!!!!!!!!!!!!
        //END

        private void startFlashBtn_Click(object sender, EventArgs e) {
            flashProcessRichTxtBox.Focus();

            //Стиль прогресс бара
            progressBar.SetState(progressBarFlashing, 1);

            //Обнуляю значение прогресс бара
            progressBarFlashing.Value = 0;

            confCommandsAndAnswerQuectel = new StringBuilder("");

            switch (modemTypeCmBox.SelectedItem) {

                //Если выбран модем GSM3
                case "GSM3": {

                        flashThread = new Thread(new ThreadStart(reflashGSM3Modem));
                        flashThread.Start();

                    }
                    break;

                default: throw new NotSupportedException("В программе нет сценария работы для выбранного модема");
            }
        }

        private void reflashGSM3Modem() {
            //Лог буфер для логов прошивки микроконтроллера
            logBuffer = new StringBuilder();

            Board GSM3;

            try {
                //Отключаю кнопку старт
                enableStartButton(false);
                //Отключаю кнопку конфигурации
                enableEditConfButton(false);

                //============================================================= Передаю выбранный конфиурационный файл
                string selectedConfiguration = "";

                Invoke((MethodInvoker)delegate {
                    selectedConfiguration = configurationCmBox.GetItemText(configurationCmBox.SelectedItem);
                });

                ConfigurationFW configurationFW;

                if (!String.IsNullOrEmpty(selectedConfiguration)) {

                    ConfigurationFileStorage configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

                    configurationFW = configurationFileStorage.getConfigurationFile(selectedConfiguration);

                    //Запоминаю последнюю используемую конфигурацию для инициализации комбобокса при старте программы
                    Settings.Default.lastConfiguration = selectedConfiguration;
                    Settings.Default.Save();

                    //Если не создан конфигурационный файл
                } else {

                    Invoke((MethodInvoker)delegate {
                        exceptionDialog("Для работы программы необходимо создать конфигурационный файл");
                        enableStartButton(true);
                        enableEditConfButton(true);
                    });

                    return;
                }

                //Пути к файлам с прошивкой для модуля Quectel и Микроконтроллера
                string pathWFforQuectel = "";
                string pathWFforMK = "";

                //================================ проверка наличия нужных файлов для перепрошивки =======================================================
                //Если поле названия прошивки для микроконтроллера не пустое
                if (!String.IsNullOrEmpty(configurationFW.getFwForMKName())) {

                    //Если удалена директория с файлами прошивки
                    if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\StorageMKFW")) {

                        Invoke((MethodInvoker)delegate {
                            exceptionDialog("Программе не удалось найти папку с прошивками для микроконтроллера \"StorageMKFW\", проверьте целостность программы" +
                                " или переустановите её и попробуйте снова");
                            enableStartButton(true);
                            enableEditConfButton(true);
                        });
                        return;
                    }

                    pathWFforMK = Directory.GetCurrentDirectory() + "\\StorageMKFW" + "\\" + configurationFW.getFwForMKName() + ".hex";
                    //То проверяю её наличие в нужной папке созданной программой
                    if (!File.Exists(pathWFforMK)) {
                        Invoke((MethodInvoker)delegate {
                            exceptionDialog("Программе не удалось найти файл с прошивкой для микроконтроллера " + "\"" + configurationFW.getFwForMKName() + "\" " +
                                "необходимо добавить файл в папку \"StorageMKFW\"");
                            enableStartButton(true);
                            enableEditConfButton(true);
                        });
                        return;
                    }
                }

                //Если поле названия прошивки для модуля Quectel не пустое
                if (!String.IsNullOrEmpty(configurationFW.getfwForQuectelName())) {

                    //Если удалена директория с файлами прошивки
                    if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\StorageQuectelFW")) {

                        Invoke((MethodInvoker)delegate {
                            exceptionDialog("Программе не удалось найти папку с прошивками для модуля Quectel \"StorageQuectelFW\", проверьте целостность программы" +
                                " или переустановите её и попробуйте снова");
                            enableStartButton(true);
                            enableEditConfButton(true);
                        });
                        return;
                    }

                    pathWFforQuectel = Directory.GetCurrentDirectory() + "\\StorageQuectelFW" + "\\" + configurationFW.getfwForQuectelName() + ".lod";
                    //То проверяю её наличие в нужной папке созданной программой
                    if (!File.Exists(pathWFforQuectel)) {
                        Invoke((MethodInvoker)delegate {
                            exceptionDialog("Программе не удалось найти файл с прошивкой для модуля Quectel " + "\"" + configurationFW.getfwForQuectelName() + "\" " +
                                "необходимо добавить файл в папку \"StorageQuectelFW\"");
                            enableStartButton(true);
                            enableEditConfButton(true);
                        });
                        return;
                    }

                    //Проверяю содержит ли путь русские символы
                    Regex regex = new Regex("[а-я]");
                    MatchCollection matches = regex.Matches(pathWFforQuectel.ToLower());

                    if (matches.Count > 0) {
                        Invoke((MethodInvoker)delegate {
                            exceptionDialog("Путь к прошивке не должен содержать русские символы или пробельные символы\n" + pathWFforQuectel);
                            enableStartButton(true);
                            enableEditConfButton(true);
                        });
                        return;
                    }
                }

                //Если не указана прошивка ни для контроллера на для модуля Quectel, то выхожу
                if (String.IsNullOrEmpty(configurationFW.getfwForQuectelName()) & String.IsNullOrEmpty(configurationFW.getFwForMKName())) {
                    Invoke((MethodInvoker)delegate {
                        exceptionDialog("В конфигурации не указана прошивка ни для микроконтроллера, ни для модуля Quectel");
                        enableStartButton(true);
                        enableEditConfButton(true);
                    });
                    return;
                }

                //================================== Перепрошивка модема ===========================================================
                firmwareWriteStart.Start();

                GSM3 = new GSM3_Board(pathWFforQuectel, pathWFforMK, configurationFW);

                //Перепрошиваю
                GSM3.Reflash();

                firmwareWriteStart.Stop();

                //включаю кнопку старт
                enableStartButton(true);
                enableEditConfButton(true);

            } catch (Exception ex) {
                addProgressFlashMKLogInMainLog();               
                addMessageInMainLog("\n==========================================================================================");
                addMessageInMainLog("Тип ошибки: " + ex.GetType().ToString());
                addMessageInMainLog("Метод: " + ex.TargetSite.ToString());
                addMessageInMainLog("ОШИБКА: " + ex.Message);
                addMessageInMainLogWithoutTime("");

                Invoke((MethodInvoker)delegate {
                    progressBar.SetState(progressBarFlashing, 2);
                    exceptionDialog(ex.Message);
                });

                firmwareWriteStart.Stop();

                if (!(ex is CP_Error)) {
                    CP2105_Connector cp2105 = CP2105_Connector.GetCP2105_ConnectorInstance();
                    cp2105.WriteGPIOStageAndSetFlags(cp2105.getStandardPort(), true, true, true, 300, true);
                    cp2105.WriteGPIOStageAndSetFlags(cp2105.getEnhancedPort(), true, true, 100, true);
                }

                //включаю кнопку старт
                enableStartButton(true);
                enableEditConfButton(true);
            }
        }

        /// <summary>
        /// Выводит сообщение в основной лог программы
        /// </summary>
        /// <param name="mess"></param>
        public static void addMessageInMainLog(string mess) {

            flashProcessTxtBoxStatic.Invoke((MethodInvoker)delegate {
                //Старый вариант
                //flashProcessTxtBoxStatic.AppendText(parseMlsInMMssMls(firmwareWriteStart.ElapsedMilliseconds) + ":    " + mess + Environment.NewLine);
                flashProcessTxtBoxStatic.AppendText(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + " >>    " + mess + Environment.NewLine);
            });
        }

        /// <summary>
        /// Выводит информацию в основной лог без метки времени
        /// </summary>
        /// <param name="mess"></param>
        public static void addMessageInMainLogWithoutTime(string mess) {
            flashProcessTxtBoxStatic.Invoke((MethodInvoker)delegate {
                flashProcessTxtBoxStatic.AppendText(mess + Environment.NewLine);
            });
        }

        /// <summary>
        /// Записывает ход выполнения контроллера во внутренний буфер (необходимо для быстродействия)
        /// </summary>
        /// <param name="mess"></param>
        public static void addMessInLogBuffer(string mess) {            
            logBuffer.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + " >>    " + mess + Environment.NewLine);
        }

        public static void addMessInLogBufferWithoutTime(string mess) {
            logBuffer.Append(mess + Environment.NewLine);
        }

        /// <summary>
        /// Выводит информацию при перпрошивке микроконтроллера в лог
        /// </summary>
        public static void addProgressFlashMKLogInMainLog() {
            flashProcessTxtBoxStatic.Invoke((MethodInvoker) delegate {
                flashProcessTxtBoxStatic.AppendText(logBuffer.ToString());
            });
        }

        /// <summary>
        /// Отключает кнопку старт
        /// </summary>
        /// <param name="stateButton"></param>
        private void enableStartButton(bool stateButton) {
            startFlashBtn.Invoke((MethodInvoker) delegate {
                startFlashBtn.Enabled = stateButton;
            });
        }

        /// <summary>
        /// Отключает кнопку конфигурации
        /// </summary>
        /// <param name="stateButton"></param>
        private void enableEditConfButton(bool stateButton) {
            startFlashBtn.Invoke((MethodInvoker)delegate {
                editConfiguration.Enabled = stateButton;
            });
        }
        

        /// <summary>
        /// Обновляет комбобокс с конфигурациями основоного окна при добавлении новой конфигурации
        /// </summary>
        public static void refreshConfigurationCmBox() {
            configurationCmBoxStatic.Items.Clear();

            //Выгрузка конфигурации прошивки
            ConfigurationFileStorage configuratinFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

            if (configuratinFileStorage.getAllConfigurationFiles().Count() > 0) {

                foreach (ConfigurationFW configuration in configuratinFileStorage.getAllConfigurationFiles()) {
                    configurationCmBoxStatic.Items.Add(configuration.getName());
                }

                //Выставляю в списке конфигураций селект на последнюю используемую
                if (!ConfigurationFrame.editebleOrAddedConfName.Equals("")) {

                    //Флаг для нахождения последней используемой конфигурации в списке конфигурации
                    bool find = false;

                    foreach (string confName in configurationCmBoxStatic.Items) {

                        if (confName.Equals(ConfigurationFrame.editebleOrAddedConfName)) {
                            configurationCmBoxStatic.SelectedItem = confName;
                            find = true;
                            break;
                        }
                    }

                    //Если не найдено, то выставляю первую конфигурацию
                    if (!find) {
                        configurationCmBoxStatic.SelectedIndex = 0;
                    }
                }

            } else {
                configurationTextBoxStatic.Text = "";
            }

        }

        private void loadConfigurationCmBx() {
            configurationCmBoxStatic.Items.Clear();

            //Выгрузка конфигурации прошивки
            ConfigurationFileStorage configuratinFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

            if (configuratinFileStorage.getAllConfigurationFiles().Count() > 0) {

                foreach (ConfigurationFW configuration in configuratinFileStorage.getAllConfigurationFiles()) {
                    configurationCmBoxStatic.Items.Add(configuration.getName());
                }

                //Выставляю в списке конфигураций селект на последнюю используемую
                if (!Settings.Default.lastConfiguration.Equals("")) {

                    //Флаг для нахождения последней используемой конфигурации в списке конфигурации
                    bool find = false;

                    foreach (string confName in configurationCmBoxStatic.Items) {

                        if (confName.Equals(Settings.Default.lastConfiguration)) {
                            configurationCmBoxStatic.SelectedItem = confName;
                            find = true;
                            break;
                        }
                    }

                    //Если не найдено, то выставляю первую конфигурацию
                    if (!find) {
                        configurationCmBoxStatic.SelectedIndex = 0;
                    }

                } else {
                    configurationCmBoxStatic.SelectedIndex = 0;
                }

            } else {
                configurationTextBoxStatic.Text = "";
            }
        }

        /// <summary>
        /// Вызывает откно установки портов в ручную
        /// </summary>
        /// <returns></returns>
        public static DialogResult setupPorts() {

            DialogResult result = DialogResult.Abort;

            mainFrame.Invoke((MethodInvoker)delegate {
                result = new PortsFrame().ShowDialog();
            });
            return result;
        }


        /// <summary>
        /// Преобразует миллисекунды в mm:ss:mls
        /// </summary>
        /// <param name="mls"></param>
        /// <returns></returns>
        public static string parseMlsInMMssMls(long mls) {

            TimeSpan interval = TimeSpan.FromMilliseconds(mls);

            return interval.ToString(@"mm\:ss\.fff") + " (mm:ss.ms)";
        }

        /// <summary>
        /// Выводит инофрмацию об ошибке
        /// </summary>
        /// <param name="errorMess"></param>
        public static void exceptionDialog(string errorMess) {
            MessageBox.Show(
                errorMess,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1);
        }

        public static void successfullyDialog(string mess, string heading) {
            MessageBox.Show(
                mess,
                heading,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Создаёт диалоговое окно "да или нет"
        /// </summary>
        /// <param name="errorMess">Сообщение</param>
        /// <param name="heading">Заголовок</param>
        /// <returns></returns>
        public static bool YesOrNoDialog(string mess, string heading) {
            bool result = false;

            mainFrame.Invoke((MethodInvoker) delegate {
                DialogResult res = MessageBox.Show(
                mess,
                heading,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

                if (res == DialogResult.Yes) {
                    result = true;
                }
            });
            
            return result;                
        }

        /// <summary>
        /// Создаёт диалоговое окно "да, нет или отмена"
        /// </summary>
        /// <param name="mess">Сообщение</param>
        /// <param name="heading">Заголовок</param>
        /// <returns></returns>
        public static DialogResult YesOrNoOrCancelDialog(string mess, string heading) {
            return MessageBox.Show(
                mess,
                heading,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Возвращает значение прогресс бара
        /// </summary>
        /// <returns></returns>
        public static int getValueProgressBarFlashingStatic() {
            return progressBarFlashingStatic.Value;
        }

        /// <summary>
        /// Устанавливает значение прогресс бара
        /// </summary>
        /// <param name="value"></param>
        public static void setValuePogressBarFlashingStatic(int value) {
            progressBarFlashingStatic.Invoke((MethodInvoker)delegate {
                progressBarFlashingStatic.Value = value;
            });
        }

        private void editConfiguration_Click(object sender, EventArgs e) {

            if (configurationForm != null) {
                configurationForm.Activate();
                configurationForm.BringToFront();
                configurationForm.WindowState = FormWindowState.Normal;

            } else {

                ConfigurationFileStorage configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

                if (!String.IsNullOrEmpty(configurationFileStorage.getPass())) {

                    if (passForm != null) {
                        passForm.Activate();
                        passForm.BringToFront();
                        passForm.WindowState = FormWindowState.Normal;
                        return;

                    } else {

                        passForm = new Password(this);

                        passForm.ShowDialog();
                    }

                } else {
                    configurationForm = new ConfigurationFrame(this);
                    configurationForm.Show();
                }

                if (configurationForm != null) {
                    configurationForm.Activate();
                    configurationForm.BringToFront();
                }
            }
        }

        /// <summary>
        /// Действие при изменении значения комбобокса с названиями конфигурации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationCmBox_SelectedValueChanged(object sender, EventArgs e) {

            configurationTextBox.Clear();

            string selectedConfiguration = configurationCmBox.GetItemText(configurationCmBox.SelectedItem);

            if (!String.IsNullOrEmpty(selectedConfiguration)) {

                ConfigurationFileStorage configurationFileStorage = ConfigurationFileStorage.GetConfigurationFileStorageInstanse();

                ConfigurationFW configurationFW = configurationFileStorage.getConfigurationFile(selectedConfiguration);

                string domenNameOrIPv4 = "";

                if (configurationFW.getSelector() != 0) {
                    domenNameOrIPv4 = "IP адрес: ";
                } else {
                    domenNameOrIPv4 = "Доменное имя: ";
                }

                configurationTextBox.AppendText("Target_ID = " + configurationFW.getTarget_ID() + Environment.NewLine);
                configurationTextBox.AppendText(Environment.NewLine);
                configurationTextBox.AppendText("Protocol_ID = " + configurationFW.getProtocol_ID() + Environment.NewLine);
                configurationTextBox.AppendText(Environment.NewLine);
                configurationTextBox.AppendText("Index = " + configurationFW.getIndex() + Environment.NewLine);
                configurationTextBox.AppendText(Environment.NewLine);
                configurationTextBox.AppendText("Порт: " + configurationFW.getPort() + Environment.NewLine);
                configurationTextBox.AppendText(Environment.NewLine);
                configurationTextBox.AppendText(domenNameOrIPv4 + configurationFW.getDomenName() + Environment.NewLine);
                configurationTextBox.AppendText(Environment.NewLine);
                configurationTextBox.AppendText("Имя прошивки микроконтроллера: " + configurationFW.getFwForMKName() + Environment.NewLine);
                configurationTextBox.AppendText(Environment.NewLine);
                configurationTextBox.AppendText("Имя прошивки Quectel: " + configurationFW.getfwForQuectelName() + Environment.NewLine);
                configurationTextBox.AppendText(Environment.NewLine);

                List<string> list = configurationFW.getQuectelCommandList();

                if (list != null) {

                    //Если есть конфигурационные команды для модуля Quectel
                    if (configurationFW.getQuectelCommandList().Count > 0) {

                        configurationTextBox.AppendText("Конфигурационные команды модуля Quectel:" + Environment.NewLine);

                        //Отображаю конфигурационные команды модуля Quectel
                        foreach (string commandQuectel in configurationFW.getQuectelCommandList()) {

                            if (!String.IsNullOrEmpty(commandQuectel)) {

                                configurationTextBox.AppendText(commandQuectel + Environment.NewLine);
                            }
                        }
                    }
                }
            }

            //Изменение строки в таблице конфигурации
            if (configurationForm != null) {

                foreach(DataGridViewRow row in ((ConfigurationFrame) configurationForm).getConfigurationDataGridView().Rows) {

                    if (row.Cells[0].Value.ToString().Equals(configurationCmBox.Text)) {
                        row.Selected = true;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// Показывает всплывающую подсказку на указателе мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="form"></param>
        /// <param name="duration"></param>
        public static void ShowToolTip(string message, Form form, int duration) {
            new ToolTip().Show(message, form, Cursor.Position.X - form.Location.X, Cursor.Position.Y - form.Location.Y, duration);
        }

        public void setConfigurationForm(Form configurationForm) {
            this.configurationForm = configurationForm;
        }

        public void setPassForm(Form passForm) {
            this.passForm = passForm;
        }

        public Form getConfigurationForm() {
            return configurationForm;
        }

        private void saveLogBtn_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = "taipitFlasherLog " + DateTime.Now.ToString("yyyy_MM_dd_HH-mm-ss") + ".log";
            saveFileDialog.Filter = "All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
           

            if (saveFileDialog.ShowDialog() == DialogResult.OK) {

                // сохраняем текст в файл
                File.WriteAllText(saveFileDialog.FileName, flashProcessRichTxtBox.Text);

                Flasher.successfullyDialog("Лог успешно сохранен", "Сохранение лога");
            }
        }

        private void terminalBtn_Click(object sender, EventArgs e) {
            new Terminal().Show();
        }

        private void Flasher_FormClosing(object sender, FormClosingEventArgs e) {
            if (flashThread != null) {
                if (flashThread.IsAlive) {
                    Flasher.exceptionDialog("Нельзя выйти во время перепрошивки модема");
                    e.Cancel = true;
                }
            }
        }
    }
}