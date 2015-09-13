using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;

namespace ZondBase
{
    public class CommunicationService : INotifyPropertyChanged
    {
        const byte POWERON = 0x01;
        const byte POWEROFF = 0x0F;

        const byte SENSOR1 = 0x1;
        const byte SENSOR2 = 0x2;
        const byte TEMP = 0x4;
        const byte ADDRESS = 0x7;

        const byte MOD1 = 0x90;
        const byte MOD2 = 0xA0;
        const byte MOD3 = 0xB0;
        const byte MOD4 = 0xC0;
        const byte MOD5 = 0xD0;
        const byte MOD6 = 0xE0;
        const byte MOD7 = 0xF0;
        private const int DELAY = 1000;

        const int RETRY = 3;

        private string _SelectedPort;
        private ObservableCollection<string> _Ports;
        private ObservableCollection<string> _Log;

        public ObservableCollection<string> Ports { get { return _Ports; } }
        public ObservableCollection<string> Log { get { return _Log; } }
        public string SelectedPort
        {
            get
            {
                return _SelectedPort;
            }
            set
            {
                if (_SelectedPort == value)
                    return;
                _SelectedPort = value;
                RaisePropertyChanged("SelectedPort");
            }
        }
        public Command RefreshPortsCommand { get; private set; }

        public CommunicationService()
        {
            _Ports = new ObservableCollection<string>();
            _Log = new ObservableCollection<string>();
            RefreshPortsCommand = new Command((x) => RefreshPorts(), (x) => true);
            RefreshPorts();
        }

        private void RefreshPorts()
        {
            _Ports.Clear();
            var ports = System.IO.Ports.SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                _Ports.Add(port);
            }
            if (_Ports.Count > 0)
                _SelectedPort = _Ports[0];
            else
                _SelectedPort = null;
        }

        public bool AskSensor(SensorDBItem sensor)
        {
            if (string.IsNullOrEmpty(SelectedPort))
            {
                SynchronisationHelper.ShowMessage("Порт не выбран", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
                return false;
            }

            SynchronisationHelper.ShowMessage("Запрос может занять несколько секунд.", "Внимание!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

            try
            {
                using (SerialPort port = new SerialPort(SelectedPort, 38400))
                {
                    port.Open();

                    PowerOn(port);

                    for (int i = 1; i <= RETRY; i++) //Запрос адреса
                    {
                        SendCommand((byte)(ModIndexToBt(sensor.SensorIndex) + ADDRESS), port);
                        int? address = ReadBytes(port);
                        if (!address.HasValue)  //Опрос завершился ошибкой - идем на перееопрос
                        {
                            if (i != RETRY) continue;
                            else   //Датчик не ответил
                            {
                                PowerOff(port);
                                SynchronisationHelper.ShowMessage("Датчик не отвечает", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                                return false;
                            }
                        }


                        if (address.Value != sensor.SensorId) //Адрес не совпал поругались и прервали опрос
                        {
                            PowerOff(port);
                            SynchronisationHelper.ShowMessage("Индекс датчика не соотвествует выбранному", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                            return false;
                        }
                        else
                            break;  //Адрес совпал выходим из опроса
                    }
                    System.Threading.Thread.Sleep(200); //Задержка
                    int? sens1 = 0;
                    for (int i = 0; i <= RETRY; i++) //Опрос первого датчика
                    {
                        SendCommand((byte)(ModIndexToBt(sensor.SensorIndex) + SENSOR1), port);
                        sens1 = ReadBytes(port);
                        if (sens1.HasValue)
                            break;

                        if (i == RETRY)
                        {
                            PowerOff(port);
                            SynchronisationHelper.ShowMessage("Датчик не ответил на запросы первого показания", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                            return false;
                        }
                    }
                    System.Threading.Thread.Sleep(200); //Задержка
                    int? sens2 = 0;
                    for (int i = 0; i <= RETRY; i++) //Опрос первого датчика
                    {
                        SendCommand((byte)(ModIndexToBt(sensor.SensorIndex) + SENSOR2), port);
                        sens2 = ReadBytes(port);
                        if (sens2.HasValue)
                            break;

                        if (i == RETRY)
                        {
                            PowerOff(port);
                            SynchronisationHelper.ShowMessage("Датчик не ответил на запросы второго показания", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                            return false;
                        }
                    }
                    System.Threading.Thread.Sleep(2500); //Задержка
                    int? temp = 0;
                    for (int i = 0; i <= RETRY; i++) //Опрос первого температуры
                    {
                        SendCommand((byte)(ModIndexToBt(sensor.SensorIndex) + TEMP), port);
                        temp = ReadBytes(port);
                        if (temp.HasValue)
                        {
                            temp = GetTemperature(temp.Value) / 16;
                            break;
                        }

                        if (i == RETRY)
                        {
                            PowerOff(port);
                            SynchronisationHelper.ShowMessage("Датчик не ответил на запросы температуры", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                            return false;
                        }
                    }

                    var sdbh = new SensorDBHistory { Sensor1Value = sens1.Value, Sensor2Value = sens2.Value, Temperature = temp.Value, TimeStamp = DateTime.Now };
                    sensor.History.Add(sdbh);

                    PowerOff(port);
                }
            }
            catch (Exception err)
            {
                SynchronisationHelper.ShowMessage(err.Message, "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            return false;
        }

        private void PowerOn(SerialPort port)
        {
            WriteLog(string.Format(" >> {0} //Включение питания", POWERON.ToString("X")));

            byte[] data = new byte[] { POWERON };
            port.Write(data, 0, 1);
            System.Threading.Thread.Sleep(2000);
        }

        private void PowerOff(SerialPort port)
        {
            WriteLog(string.Format(" >> {0} //Выключение питания", POWEROFF.ToString("X")));

            byte[] data = new byte[] { POWEROFF };
            port.Write(data, 0, 1);
        }

        private void SendCommand(byte bt, SerialPort port)
        {
            WriteLog(string.Format(" >> {0} {1}", bt.ToString("X"), bt.ToString("X")));

            byte[] dataT = new byte[] { bt, bt };
            port.ReadExisting();
            port.Write(dataT, 0, 2);
        }

        private int? ReadBytes(SerialPort port)
        {
            var timer = DateTime.Now.AddMilliseconds(DELAY);

            while (port.BytesToRead < 2 & DateTime.Now < timer) ;

            if (port.BytesToRead == 2)
            {
                byte rH = (byte)port.ReadByte();
                byte rL = (byte)port.ReadByte();
                rH = (byte)(rH & 0x7F);

                int result = (rH << 7) + rL;
                WriteLog(string.Format(" << {0}", result));
                return result;
            }

            WriteLog("//Превышено время ожидания ответа");
            if (port.BytesToRead > 0)
                port.ReadExisting();
            return null;
        }

        private int GetTemperature(int value)
        {
            if ((value & 0x1000) > 0)
                return 0 - (value & 0x0FFF);
            else
                return value;
        }

        private void WriteLog(string str)
        {
            _Log.Insert(0, str);
        }

        private byte ModIndexToBt(int idx)
        {
            if (idx < 1 | idx > 7)
                throw new Exception("Индекс модуля за пределами");

            switch (idx)
            {
                case 1:
                    return MOD1;
                case 2:
                    return MOD2;
                case 3:
                    return MOD3;
                case 4:
                    return MOD4;
                case 5:
                    return MOD5;
                case 6:
                    return MOD6;
                case 7:
                    return MOD7;
            }
            return 0;
        }

        #region RaisePropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
