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
    public class ConfigurationFW : ICloneable {

        //Имя конфигурации (наименование счётчика для которого эта кнофигурация)
        private string name = "";

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

        private string fwForMKName = "";
        private string fwForQuectelName = "";

        //Для информации, типы селекторов
        private byte selectorIPv4 = 0x01; //IPv4
        private byte selectorIPv6 = 0x02; //IPv6

        //Новыве параметры (03.03.2021)
        private ushort listenPort = 4059;
        private string APN_Name = "\"\"";
        private byte[] APN_NameByteArr;

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
        /// <param name="comandsQuectelList">Лист с командами для модуля Quectel</param>
        public ConfigurationFW(string name, byte Target_ID, byte Index, byte Protocol_ID, bool eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit,
            ushort port, byte selector, string domenName, byte[] domenNameByteArr, string fwForMKName, string fwForQuectelName, List<string> comandsQuectelList) {
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
            this.quectelCommandList = comandsQuectelList;
        }

        public ConfigurationFW() { }

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

                        if (freeSpace < 64) throw new MKCommandException("Недостаточно места для записи сценария №3, необходимо 64 байт");

                        configList = new List<byte>(64 - verIDAndFW_Name.Length);

                        configList.AddRange(firstPage(verIDAndFW_Name.Length));
                        configList.AddRange(secondPage());

                        return configList.ToArray();
                    } break;

                case 4: {
                        throw new NotSupportedException("Сценарий №4 не реализован");
                    } break;

                case 5: {
                        if (freeSpace < 96) throw new MKCommandException("Недостаточно места для записи сценария №5, необходимо 96 байт");

                        configList = new List<byte>(96 - verIDAndFW_Name.Length);

                        configList.AddRange(firstPage(verIDAndFW_Name.Length));
                        configList.AddRange(secondPage());
                        configList.AddRange(thirdPage());

                        return configList.ToArray();

                    } break;

                default: throw new MKCommandException("В программе нет сценария конфигурации для выбранного Nver");
            }
        }

        /// <summary>
        /// Первая страница данных
        /// </summary>
        private List<byte> firstPage(int verIDAndFW_Name_size) {

            //Имя и номер конфигурации уже записаны в буфер
            List<byte> pageList = new List<byte>(32 - verIDAndFW_Name_size);

            //Формирую дженерал ID
            byte[] arrGeneralID = BitConverter.GetBytes(general_ID_Nmb);
            Array.Reverse(arrGeneralID);

            //Изменяю дженерал айди для его уникальности
            general_ID_Nmb++;

            //Сериализую изменённое значение
            ConfigurationFileStorage.serializeConfigurationFileStorage();

            //24
            pageList.AddRange(arrGeneralID);

            //23
            //Формирую байт десятилетия
            byte year = (byte)((DateTimeOffset.Now.Year % 2000) / 10);
            pageList.Add(year);

            //19
            //Вычисляю время в секундах с начала столетия
            TimeSpan dt = DateTime.Now - new DateTime(((DateTimeOffset.Now.Year / 100) * 100), 1, 1);
            uint seconds = (uint)dt.TotalSeconds;
            byte[] arrTotalSec = BitConverter.GetBytes(seconds);
            Array.Reverse(arrTotalSec);
            pageList.AddRange(arrTotalSec);

            //13
            //Формирование паролей
            Random randPass = new Random();
            //Формирую NW_pass
            for (int i = 0; i < 6; i++)
                pageList.Add((byte)randPass.Next(0, 255));

            //5
            //Формирую SW_pass
            for (int i = 0; i < 8; i++)
                pageList.Add((byte)randPass.Next(0, 255));

            //4
            //Reserv byte
            pageList.Add(0);

            //3
            //Target_ID
            pageList.Add(Target_ID);

            //2
            //Index
            pageList.Add(Index);

            //1
            //Protocol_ID
            pageList.Add(Protocol_ID);

            //0
            //Сделаю флаг при добавлении
            if (eGeneral_ID_Interface_Func_MCL_Mode_flg_Nbit) {
                //ID_Interface_Func (Есть только один остальные просто зарезервированны в случае если что-то изменится необходимо будет добавить)
                pageList.Add(0x04);
            } else {
                pageList.Add(0x00);
            }

            return pageList;
        }

        private List<byte> secondPage() {

            List<byte> pageList = new List<byte>(32);

            //Записываю порт
            byte[] portArr = BitConverter.GetBytes(port);
            Array.Reverse(portArr);
            pageList.AddRange(portArr);

            //Если неоходим селектор
            if (selector != 0) {
                //Значит IP адрес указан в формате IPv4 или IPv6
                pageList.Add(selector);
                pageList.AddRange(domenNameByteArr);
            } else {
                pageList.AddRange(domenNameByteArr);
            }

            //Заполняю оставшееся место в листе 0x00
            for (int i = pageList.Count; i < 32; i++) {
                pageList.Add(0x00);
            }

            return pageList;
        }

        private List<byte> thirdPage() {
            List<byte> pageList = new List<byte>(32);

            //Дабавляю слушающий порт
            byte[] portArr = BitConverter.GetBytes(listenPort);
            Array.Reverse(portArr);
            pageList.AddRange(portArr);

            pageList.AddRange(APN_NameByteArr);

            for (int i = pageList.Count; i < 32; i++) {
                pageList.Add(0x00);
            }

            return pageList;
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

        public void setQuectelCommandList(List<string> commandsQuectel) {
            this.quectelCommandList = commandsQuectel;
        }

        public List<string> getQuectelCommandList() {
            return quectelCommandList;
        }

        public ushort getListenPort() {
            return listenPort;
        }

        public void setListenPort(ushort listenPort) {
            this.listenPort = listenPort;
        }


        public void setAPN_Name(string APN_Name) {
            this.APN_Name = APN_Name;
        }

        public string getAPN_Name() {
            return APN_Name;
        }

        public void setAPN_NameByteArr(byte[] APN_NameByteArr) {
            this.APN_NameByteArr = APN_NameByteArr;
        }

        public object Clone() {
            return MemberwiseClone();
        }
    }
}
