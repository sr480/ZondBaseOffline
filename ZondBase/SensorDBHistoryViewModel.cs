using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ZondBase
{
    public class SensorDBHistoryViewModel : INotifyPropertyChanged
    {
        private SensorDBItem _sensor;
        private SensorDBHistory _sensorHistory;

        public SensorDBHistory SensorHistory
        {
            get
            {
                return _sensorHistory;
            }
        }
        public DateTime TimeStamp
        {
            get
            {
                return SensorHistory.TimeStamp;
            }
        }
        public int Sensor1Value
        {
            get
            {
                return SensorHistory.Sensor1Value;
            }
        }
        public int Sensor2Value
        {
            get
            {
                return SensorHistory.Sensor2Value;
            }
        }
        public int Temperature
        {
            get
            {
                return SensorHistory.Temperature;
            }
        }
        public double Position
        {
            get
            {
                return SensorHistory.Position;
            }
            set
            {
                if (SensorHistory.Position == value)
                    return;
                SensorHistory.Position = value;
                RaisePropertyChanged("Position");
            }
        }
        public double FullDeformation
        {
            get
            {
                return Math.Round(GetFirstHistoryItem().Position - Position, 2);
            }
        }
        public double Deformation
        {
            get
            {
                var prev = GetPreviousHistoryItem();
                if (prev == null)
                    return 0;
                else
                    return Math.Round(prev.Position - Position, 2);
            }
        }
        public double ActualDeformation
        {
            get
            {
                return Math.Round(_sensor.ActualPosition + FullDeformation, 2);
            }
        }
        public SensorDBHistoryViewModel(SensorDBItem sensor, SensorDBHistory sensorHistory)
        {
            if (sensor == null)
                throw new ArgumentNullException("sensor", "sensor is null.");
            if (sensorHistory == null)
                throw new ArgumentNullException("sensorHistory", "sensorHistory is null.");

            _sensor = sensor;
            _sensorHistory = sensorHistory;
        }

        private SensorDBHistory GetFirstHistoryItem()
        {
            if (_sensor.History.Count != 0)
                return _sensor.History.OrderBy(s => s.TimeStamp).First();
            return null;
        }
        private SensorDBHistory GetPreviousHistoryItem()
        {
            if (_sensor.History.Count != 0)
                return _sensor.History.OrderByDescending(s => s.TimeStamp).Where(d => d.TimeStamp < TimeStamp).FirstOrDefault();
            return null;
        }

        public void UpdateItem()
        {
            RaisePropertyChanged("FullDeformation");
            RaisePropertyChanged("Deformation");
            RaisePropertyChanged("ActualDeformation");
        }
        public void UpdateItemFull()
        {
            RaisePropertyChanged("TimeStamp");
            RaisePropertyChanged("Sensor1Value");
            RaisePropertyChanged("Sensor2Value");
            RaisePropertyChanged("Temperature");
            RaisePropertyChanged("FullDeformation");
            RaisePropertyChanged("Deformation");
            RaisePropertyChanged("ActualDeformation");
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
