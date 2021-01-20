using GSM_NBIoT_Module.classes.controllerOnBoard.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.terminal {

    /// <summary>
    /// Данный класс выполняет роль хранилища для создаваемых групп макросов (class ConfigurationFW).
    /// Сереализирует и десереализирует файл с макросами для терминала
    /// </summary>
    [Serializable]
    class MacrosesGroupStorage {

        private static MacrosesGroupStorage macrosesGroupStorageInstance;

        //Путь для сохранения файла после сериализации
        static string pathForSerialization = Directory.GetCurrentDirectory() + "\\macroses.dat";

        //Лист с макросами
        private List<MacrosesGroup> macrosesGroupsList = new List<MacrosesGroup>();

        private MacrosesGroupStorage() {
            //5 вкладок с групппами в окне терминала
            for (int i = 0; i < 5; i++) {
                macrosesGroupsList.Add(new MacrosesGroup("Группа макросов " + (i + 1)));
            }
        }

        /// <summary>
        /// Возвращает хранилище макросов
        /// </summary>
        /// <returns></returns>
        public static MacrosesGroupStorage getMacrosesGroupStorageInstance() {

            if (macrosesGroupStorageInstance == null) {
                deserializeMacrosesGroupStorage();
            }

            return macrosesGroupStorageInstance;
        }

        /// <summary>
        /// Сериализует хранилище макросов
        /// </summary>
        public static void serializeMacrosesGroupStorage() {

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(pathForSerialization, FileMode.Create)) {
                formatter.Serialize(fs, macrosesGroupStorageInstance);
                fs.Close();
            }
        }

        /// <summary>
        /// Десериализует хранилище макросов
        /// </summary>
        private static void deserializeMacrosesGroupStorage() {

            //Если файл хранилища не существует
            if (!File.Exists(pathForSerialization)) {

                //Создаю новое хранилище
                macrosesGroupStorageInstance = new MacrosesGroupStorage();

                serializeMacrosesGroupStorage();
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(pathForSerialization, FileMode.OpenOrCreate)) {
                macrosesGroupStorageInstance = (MacrosesGroupStorage)formatter.Deserialize(fs);
                fs.Close();
            }
        }

        public List<MacrosesGroup> getmacrosesGroupsList() {
            return macrosesGroupsList;
        }
    }
}
