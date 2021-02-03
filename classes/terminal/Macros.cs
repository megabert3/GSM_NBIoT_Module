using GSM_NBIoT_Module.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.terminal {
    /// <summary>
    /// Отвечает за отправку предподготовленных данных в COM порт
    /// </summary>
    [Serializable]
    public class Macros : ICloneable {

        //Название макроса
        private string macrosName = "";
        //Данные, которые необходимо отправить в COM порт
        private string macrosValue = "";
        //Отправлять данные с какой-то переодичностью?
        private bool macrosInCycle = false;
        //Пауза между отправкой данных с какой-то переодичностью
        private int timeCycle = 1000;

        [NonSerialized]
        //Треад для циклической отпраки сообщений
        private Thread cycleThreadSendData;

        public Macros(string macrosName, string macrosValue, bool macrosIncycle, int timeCycle) {
            this.macrosName = macrosName;
            this.macrosValue = macrosValue;
            this.macrosInCycle = macrosIncycle;
            this.timeCycle = timeCycle;
        }

        /// <summary>
        /// Название макроса
        /// </summary>
        public string MacrosName {
            get { return macrosName; }
            set { macrosName = value; }
        }

        /// <summary>
        /// Данные, которые необходимо отправить в COM порт
        /// </summary>
        public string MacrosValue {
            get { return macrosValue; }
            set { macrosValue = value; }
        }

        /// <summary>
        /// Включает циклическую отправку данных в COM порт
        /// </summary>
        public bool MacrosInCycle {
            get { return macrosInCycle; }
            set { macrosInCycle = value; }
        }

        /// <summary>
        /// Пауза между отправкой данных в COM порт в цеклическом режиме
        /// </summary>
        public int TimeCycle {
            get { return timeCycle; }

            set {
                if (value < 0) {
                    throw new FormatException("Не удалось привести значение к целочисленному типу");
                } else {
                    timeCycle = value;
                }
            }
        }

        public object Clone() {
            return MemberwiseClone();
        }

        /// <summary>
        /// Запускает циклическую отправку данных в COM порт
        /// </summary>
        /// <param name="terminalForm"></param>
        public void startCycleSendDataInCOM(Terminal terminalForm) {

            cycleThreadSendData = new Thread(delegate() {

                while (Thread.CurrentThread.IsAlive) {

                    terminalForm.sendCommandInCOMPort(macrosValue);

                    Thread.Sleep(timeCycle);
                }
            });

            cycleThreadSendData.Start();
        }

        /// <summary>
        /// Возвращает состояние потока циклической отправки данных в COM порт
        /// </summary>
        /// <returns></returns>
        public bool cycleThreadIsLeave() {
            return cycleThreadSendData.IsAlive;
        }

        public Thread getCycleThreadSendData() {
            return cycleThreadSendData;
        }
    }
}