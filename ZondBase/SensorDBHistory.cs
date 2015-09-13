using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ZondBase
{
    [Serializable]
    public class SensorDBHistory : INotifyPropertyChanged
    {
        public int Sensor1Value { get; set; }
        public int Sensor2Value { get; set; }
        public int Temperature { get; set; }
        private double _Position;
        public double Position
        {
            get
            {
                return _Position;
            }
            set
            {
                if (_Position == value)
                    return;
                _Position = value;
                RaisePropertyChanged("Position");
            }
        }
        public DateTime TimeStamp { get; set; }

        public SensorDBHistory()
        {
            Sensor1Value = 0;
            Sensor2Value = 0;
            Temperature = 0;
            _Position = 0d;
            TimeStamp = DateTime.Now;
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
