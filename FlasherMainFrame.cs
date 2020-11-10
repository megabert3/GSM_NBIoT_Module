using GSM_NBIoT_Module.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSM_NBIoT_Module {
    public partial class Flasher : Form {
        public Flasher() {
            InitializeComponent();
        }

        //Типы используемых модемов
        private string[] modemType = { "GSM3" };

        private static ProgressBar progressBarFlashingStatic;

        private static RichTextBox flashProcessTxtBoxStatic;

        private static Stopwatch firmwareWriteStart = new Stopwatch();

        //Буфер сообщений для перепрошивки микроконтроллера
        private static StringBuilder logBuffer;

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

        private void Flasher_Load(object sender, EventArgs e) {
            //Устанавливаю статическому полю ссылку на основное окно Лога (Для статического доступа к окну поля из всей программы)
            flashProcessTxtBoxStatic = flashProcessRichTxtBox;

            progressBarFlashingStatic = progressBarFlashing;

            modemTypeCmBox.Items.AddRange(modemType);
            modemTypeCmBox.SelectedIndex = 0;
        }

        private void startFlashBtn_Click(object sender, EventArgs e) {

            switch (modemTypeCmBox.SelectedItem) {

                //Если выбран модем GSM3
                case "GSM3": {

                        new Thread(new ThreadStart(reflashGSM3Modem)).Start();

                    }
                    break;

                default: throw new NotSupportedException("В программе нет сценария работы для выбранного модена");
            }
        }

        private void reflashGSM3Modem() {
            logBuffer = new StringBuilder();

            try {
                //Отключаю кнопку старт
                enableStartButton(false);

                //Получаю пути выбранные пользователем
                string pathToFirwareQuectel = pathToQuectelFirmwareTextBox.Text.Trim();
                string pathToFirwareSTM32 = pathToSTM32FirmwareTextBox.Text.Trim();

                //Проверка путей =================================================================
                if (String.IsNullOrEmpty(pathToFirwareQuectel)) throw new FormatException("Выберите путь к прошивке модуля Quectel");
                if (String.IsNullOrEmpty(pathToFirwareSTM32)) throw new FormatException("Выберите путь к прошивке микроконтроллера");

                if (!File.Exists(pathToFirwareQuectel)) throw new FileNotFoundException("Не удалось найти файл прошивки для модуля Quectel по указанному пути");

                if (!pathToFirwareQuectel.EndsWith(".lod")) throw new FormatException("Неверное расширение файла прошивки для модуля Quectel");

                //Проверяю содержит ли путь русские символы или пробелы
                Regex regex = new Regex("[А-Яа-я]");
                MatchCollection matches = regex.Matches(pathToFirwareQuectel);

                if (matches.Count > 0) throw new FormatException("Путь не должен содержать русские символы или пробельные символы");


                if (!File.Exists(pathToFirwareSTM32)) throw new FileNotFoundException("Не удалось найти файл прошивки для микроконтроллера по указанному пути");

                if (!pathToFirwareSTM32.EndsWith(".hex")) throw new FormatException("Неверное расширение файла прошивки для микроконтроллера");

                //Перепрошивка модема ===========================================================

                firmwareWriteStart.Start();

                Board GSM3 = new GSM3_Board(pathToFirwareQuectel, pathToFirwareSTM32);

                GSM3.Reflash();

                firmwareWriteStart.Stop();

                addMessageInMainLog("Общее время перепрошивки модема " + parseMlsInMMssMls(firmwareWriteStart.ElapsedMilliseconds));

                //включаю кнопку старт
                enableStartButton(true);

            } catch (Exception ex) {
                addInLog();               
                addMessageInMainLog("\n==========================================================================================");
                addMessageInMainLog("Тип ошибки: " + ex.GetType().ToString());
                addMessageInMainLog("Метод: " + ex.TargetSite.ToString());
                addMessageInMainLog("ОШИБКА: " + ex.Message);

                //включаю кнопку старт
                enableStartButton(true);
            }
        }

        /// <summary>
        /// Выводит сообщение в основной лог программы
        /// </summary>
        /// <param name="mess"></param>
        public static void addMessageInMainLog(string mess) {

            flashProcessTxtBoxStatic.Invoke((MethodInvoker)delegate {
                flashProcessTxtBoxStatic.AppendText(parseMlsInMMssMls(firmwareWriteStart.ElapsedMilliseconds) + ":    " + mess + Environment.NewLine);
            });
        }

        /// <summary>
        /// Записывает ход выполнения контроллера во внутренний буфер (необходимо для быстродействия)
        /// </summary>
        /// <param name="mess"></param>
        public static void addMessInLogBuffer(string mess) {
            logBuffer.Append(parseMlsInMMssMls(firmwareWriteStart.ElapsedMilliseconds) + ":    " + mess + Environment.NewLine);
        }

        public static void addInLog() {
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
        /// Преобразует миллисекунды в mm:ss:mls
        /// </summary>
        /// <param name="mls"></param>
        /// <returns></returns>
        public static string parseMlsInMMssMls(long mls) {

            TimeSpan interval = TimeSpan.FromMilliseconds(mls);

            return interval.Minutes + ":" + interval.Seconds + ":" + interval.Milliseconds + " (mm:ss:mls)";
        }

        /// <summary>
        /// Выводит инофрмацию об ошибке
        /// </summary>
        /// <param name="errorMess"></param>
        private static void exceptionDialog(string errorMess) {
            MessageBox.Show(
                errorMess,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.ServiceNotification);
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
    }
}