using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.controllerOnBoard.Configuration {

    [Serializable]
    class ConfigurationFileStorage {

        private static ConfigurationFileStorage configurationFileStorage;

        //Лист с конфигурационными файлами
        List<ConfigurationFW> configurationFiles = new List<ConfigurationFW>();

        //Путь для сериализации
        static string pathForSerialization = Directory.GetCurrentDirectory() + "\\configuration.dat";

        //Пароль к конфигурационному файлу
        string password = "";

        private ConfigurationFileStorage() { }

        public static ConfigurationFileStorage GetConfigurationFileStorageInstanse() {

            if (configurationFileStorage == null) {
                deserializeConfigurationFileStorage();
            }
            return configurationFileStorage;
        }

        /// <summary>
        /// Возвращает необходимый файл конфигурации из списка
        /// </summary>
        /// <param name="name">Имя необходимого конфигурационного файла</param>
        /// <returns></returns>
        public ConfigurationFW getConfigurationFile(string name) {

            foreach (ConfigurationFW configurationFile in configurationFiles) {
                if (configurationFile.getName().Equals(name)) return configurationFile;
            }

            return null;
        }

        public List<ConfigurationFW> getAllConfigurationFiles() {
            return configurationFiles;
        }

        /// <summary>
        /// Кладёт конфигурационный файл к общим файлам в хранилище
        /// </summary>
        /// <param name="configuration"></param>
        public void addConfigurateFileInStorage(ConfigurationFW configuration) {
            configurationFiles.Add(configuration);
        }

        /// <summary>
        /// Удаляет кофнигурацию из коллекции по объекту конфигурации
        /// </summary>
        /// <param name="configuration"></param>
        public void removeConfigurateFileInStorage(ConfigurationFW configuration) {
            configurationFiles.Remove(configuration);
        }

        /// <summary>
        /// Удаляет кофнигурацию из коллекции по имени
        /// </summary>
        /// <param name="configurationName"></param>
        public void removeConfigurateFileInStorage(string configurationName) {
            for (int i = 0; i < configurationFiles.Count; i++) {

                if (configurationFiles.ElementAt(i).getName().Equals(configurationName)) {
                    configurationFiles.Remove(configurationFiles.ElementAt(i));
                }
            }
        }

        /// <summary>
        /// Серелизует хранилище конфигураций
        /// </summary>
        public static void serializeConfigurationFileStorage() {

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(pathForSerialization, FileMode.Create)) {
                formatter.Serialize(fs, configurationFileStorage);
            }
        }

        /// <summary>
        /// Десериализует хранилище конфигураций
        /// </summary>
        private static void deserializeConfigurationFileStorage() {

            if (!File.Exists(pathForSerialization)) {
                Flasher.exceptionDialog("Не удалось найти файл с конфигурациями в корневой папке, создаю новый файл");
                configurationFileStorage = new ConfigurationFileStorage();
                serializeConfigurationFileStorage();
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(pathForSerialization, FileMode.OpenOrCreate)) {
                configurationFileStorage = (ConfigurationFileStorage) formatter.Deserialize(fs);
            }
        }

        public void setPassword(string newPass) {
            this.password = newPass;
        }

        public string getPass() {
            return password;
        }
    }
}
