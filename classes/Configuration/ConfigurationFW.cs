using GSM_NBIoT_Module.classes.applicationHelper.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM_NBIoT_Module.classes.controllerOnBoard.Configuration {

    /// <summary>
    /// Данный класс выполняет фун-ции конфигурирования прошивки микроконтроллера.
    /// В прошивку в процессе записи добавляются необходимые параметры
    /// </summary>
    [Serializable]
    public class ConfigurationFW {

        //Имя конфигурации (наименование счётчика для которого эта кнофигурация)
        private string name;

        //================================================= Параметры конфигурации прошивки
        //Подробнее в файле general_id (спрашивать у Сергея Васильева)
        private ushort general_ID_Nmb = 1;

        private byte Target_ID;
        private byte Index;
        private byte Protocol_ID;

        private bool eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit; //флаг "режим MCL" (1/0 = режим MCL/стандарт)

        private ushort port;
        private byte selector;

        //Доменное имя в текстровом представлении
        private string domenName = "";
        private byte[] domenNameByteArr;

        private string fwForMKName ="";
        private string fwForQuectelName = "";

        //Для информации, типы селекторов
        private byte selectorIPv4 = 0x01; //IPv4
        private byte selectorIPv6 = 0x02; //IPv6

        //Лист с командами конфигурации модуля Quectel
        List<string> quectelCommandList = new List<string>();

        /// <summary>
        /// Создаёт конфигурационный файл с параметрами конфигурации
        /// </summary>
        /// <param name="name">Имя конфигурации</param>
        /// <param name="Target_ID"></param>
        /// <param name="Index"></param>
        /// <param name="Protocol_ID"></param>
        /// <param name="eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit"></param>
        /// <param name="port"></param>
        /// <param name="selector"></param>
        /// <param name="domenName"></param>
        /// <param name="domenNameByteArr"></param>
        /// <param name="fwForMKName"></param>
        /// <param name="fwForQuectelName"></param>
        public ConfigurationFW(string name, byte Target_ID, byte Index, byte Protocol_ID, bool eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit,
            ushort port, byte selector, string domenName, byte[] domenNameByteArr, string fwForMKName, string fwForQuectelName) {

            this.name = name;
            this.Target_ID = Target_ID;
            this.Index = Index;
            this.Protocol_ID = Protocol_ID;
            this.eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit = eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit;
            this.port = port;
            this.selector = selector;
            this.domenName = domenName;
            this.domenNameByteArr = domenNameByteArr;
            this.fwForMKName = fwForMKName;
            this.fwForQuectelName = fwForQuectelName;
        }
        
        /// <summary>
        /// Формирует конфигурацию для прошивки модема во время записи прошивки согласно выбранным пользователем параметрам
        /// </summary>
        /// <param name="numbConfigScript">Номер скрипта формирования конфигурационных байтов</param>
        /// <param name="freeSpace">Размер свободного места для записи</param>
        /// <returns></returns>
        public byte[] formationOfConfigurationData(byte numbConfigScript, int freeSpace, byte[] verIDAndFW_Name) {

            //Лист куда я буду записывать параметры
            List<byte> configList;

            switch (numbConfigScript) {
                case 3: {
                        if (freeSpace < 64) throw new MKCommandException("Недостаточно места для записи выбранного сценария");

                        //Имя и номер конфигурации уже записаны в буфер
                        configList = new List<byte>(64 - verIDAndFW_Name.Length);
                        
                        //Формирую дженерал ID
                        byte[] arrGeneralID = BitConverter.GetBytes(general_ID_Nmb);
                        Array.Reverse(arrGeneralID);                        

                        //Изменяю дженерал айди для его уникальности
                        general_ID_Nmb++;

                        //Сериализую изменённое значение
                        ConfigurationFileStorage.serializeConfigurationFileStorage();

                        configList.AddRange(arrGeneralID);

                        //Формирую байт десятилетия
                        byte year = (byte) ((DateTimeOffset.Now.Year % 2000) / 10);
                        configList.Add(year);

                        //Вычисляю время в секундах с начала столетия
                        TimeSpan dt = DateTime.Now - new DateTime(((DateTimeOffset.Now.Year / 100) * 100), 1, 1);
                        uint seconds = (uint)dt.TotalSeconds;
                        byte[] arrTotalSec = BitConverter.GetBytes(seconds);
                        Array.Reverse(arrTotalSec);
                        configList.AddRange(arrTotalSec);

                        //Формирование паролей
                        Random randPass = new Random();
                        //Формирую NW_pass
                        for (int i = 0; i < 6; i++)
                            configList.Add((byte)randPass.Next(0, 255));

                        //Формирую SW_pass
                        for (int i = 0; i < 8; i++)
                            configList.Add((byte)randPass.Next(0, 255));

                        //Reserv byte
                        configList.Add(0);

                        //Target_ID
                        configList.Add(Target_ID);

                        //Index
                        configList.Add(Index);

                        //Protocol_ID
                        configList.Add(Protocol_ID);

                        //Сделаю флаг при добавлении
                        if (eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit) {
                            //ID_Interface_Func (Есть только один остальные просто зарезервированны в случае если что-то изменится необходимо будет добавить)
                            configList.Add(0x04);
                        } else {
                            configList.Add(0x00);
                        }

                        //Записываю порт
                        byte[] portArr = BitConverter.GetBytes(port);
                        Array.Reverse(portArr);
                        configList.AddRange(portArr);

                        //Если неоходим селектор
                        if (selector != 0) {
                            //Значит IP адрес указан в формате IPv4 или IPv6
                            configList.Add(selector);
                            configList.AddRange(domenNameByteArr);
                        } else {
                            configList.AddRange(domenNameByteArr);
                        }

                        //Заполняю оставшееся место в листе 0x00
                        for (int i = configList.Count; i < 64 - verIDAndFW_Name.Length; i++) {

                            configList.Add(0x00);
                        }

                        return configList.ToArray();
                    }
                    break;
                case 4: {
                        throw new NotSupportedException("Сценарий не реализован");
                    }
                    break;
                default: throw new MKCommandException("В программе нет сценария конфигурации для выбранного Nver");
            }
        }

        public void setName(string name) {
            this.name = name;
        }

        public string getName() {
            return name;
        }

        public string getTarget_ID() {
            return Target_ID.ToString();
        }

        public void setTarget_ID(byte target_id) {
            this.Target_ID = target_id;
        }

        public string getIndex() {
            return Index.ToString();
        }

        public void setIndex(byte index) {
            this.Index = index;
        }

        public string getProtocol_ID() {
            return Protocol_ID.ToString();
        }

        public void setProtocol_ID(byte protocol_id) {
            this.Protocol_ID = protocol_id;
        }

        public bool isEGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit() {
            return eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit;
        }

        public void setEGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit(bool MCL_Mode) {
            this.eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit = MCL_Mode;
        }

        public string getPort() {
            return port.ToString();
        }

        public void setPort(ushort port) {
            this.port = port;
        }

        public string getDomenName() {
            return domenName.ToString();
        }

        public void setDomenName(string domenName) {
            this.domenName = domenName;
        }

        public byte getSelector() {
            return selector;
        }

        public void setSelector(byte selector) {
            this.selector = selector;
        }

        public string getFwForMKName() {
            return fwForMKName;
        }

        public void setFwForMKName(string fwForMKName) {
            this.fwForMKName = fwForMKName;
        }

        public string getfwForQuectelName() {
            return fwForQuectelName;
        }

        public void setfwForQuectelName(string fwForQuectelName) {
            this.fwForQuectelName = fwForQuectelName;
        }

        public ushort getGeneral_ID_Nmb() {
            return general_ID_Nmb;
        }

        public void setGeneral_ID_Nmb(ushort newGeneralIdNumb) {
            this.general_ID_Nmb = newGeneralIdNumb;
        }

        public void setDomenNameByteArr(byte[] domenNameByteArr) {
            this.domenNameByteArr = domenNameByteArr;
        }
    }
}
