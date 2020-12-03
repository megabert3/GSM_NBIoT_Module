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
    /// <summary>
    /// Данный класс выполняет роль хранилища для создаваемых конфигураций микроконтроллера (class ConfigurationFW).
    /// Сереализирует и десереализирует файл с объектами конфигурациы
    /// </summary>
    [Serializable]
    class ConfigurationFileStorage {

        //Последняя использемая конфигурация
        private string lastConfiguration = "";

        //Контейнер для хранения
        private static ConfigurationFileStorage configurationFileStorage;

        //Лист с конфигурационными файлами
        List<ConfigurationFW> configurationFiles = new List<ConfigurationFW>();

        //Путь для сохранения файла после сериализации
        static string pathForSerialization = Directory.GetCurrentDirectory() + "\\configuration.dat";

        //Пароль к конфигурационному файлу
        string password = "";

        /// <summary>
        /// Использую паттерн singleton экземпляр хранилища во всей программе должен быть один
        /// </summary>
        private ConfigurationFileStorage() { }

        /// <summary>
        /// Предоставляет доступ к хранилищу
        /// </summary>
        /// <returns>Объект хранилища</returns>
        public static ConfigurationFileStorage GetConfigurationFileStorageInstanse() {

            if (configurationFileStorage == null) {
                deserializeConfigurationFileStorage();
            }
            return configurationFileStorage;
        }

        /// <summary>
        /// Возвращает необходимый файл конфигурации из храналища
        /// </summary>
        /// <param name="name">Имя необходимого конфигурационного файла</param>
        /// <returns></returns>
        public ConfigurationFW getConfigurationFile(string name) {

            foreach (ConfigurationFW configurationFile in configurationFiles) {
                if (configurationFile.getName().Equals(name)) return configurationFile;
            }

            return null;
        }

        /// <summary>
        /// Даёт доступ ко всем конфигурационным файлам
        /// </summary>
        /// <returns>Возвращает лист со всеми созданными конфигурационными файлами</returns>
        public List<ConfigurationFW> getAllConfigurationFiles() {
            return configurationFiles;
        }

        /// <summary>
        /// Кладёт конфигурационный файл в хранилище
        /// </summary>
        /// <param name="configuration"></param>
        public void addConfigurateFileInStorage(ConfigurationFW configuration) {
            configurationFiles.Add(configuration);
        }

        /// <summary>
        /// Удаляет конфигурационный файл из хранилища по объекту
        /// </summary>
        /// <param name="configuration"></param>
        public void removeConfigurateFileInStorage(ConfigurationFW configuration) {
            configurationFiles.Remove(configuration);
        }

        /// <summary>
        /// Удаляет конфигурационный файл из хранилища по названию
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

            //Если файл хранилища не существует
            if (!File.Exists(pathForSerialization)) {
                Flasher.exceptionDialog("Не удалось найти файл с конфигурациями в корневой папке, создаю новый файл");

                //Создаю новое хранилище
                configurationFileStorage = new ConfigurationFileStorage();

                serializeConfigurationFileStorage();
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(pathForSerialization, FileMode.OpenOrCreate)) {
                configurationFileStorage = (ConfigurationFileStorage)formatter.Deserialize(fs);
            }
        }

        public void setPassword(string newPass) {
            this.password = newPass;
        }

        public string getPass() {
            return password;
        }

        public void setLastConfiguration(string lastConfiguration) {
            this.lastConfiguration = lastConfiguration;
        }

        public string getLastConfiguration() {
            return lastConfiguration;
        }
    }
}
