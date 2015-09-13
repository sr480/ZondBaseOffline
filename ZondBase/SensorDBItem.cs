using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ZondBase
{
    [Serializable]
    public class SensorDBItem : INotifyPropertyChanged
    {
        private double _ActualPosition;
        private string _Name;
        private int _SensorIndex;
        private int _SensorId;
        public int SensorId
        {
            get
            {
                return _SensorId;
            }
            set
            {
                if (_SensorId == value)
                    return;
                _SensorId = value;
                RaisePropertyChanged("SensorId");
            }
        }
        public int SensorIndex
        {
            get
            {
                return _SensorIndex;
            }
            set
            {
                if (_SensorIndex == value)
                    return;
                _SensorIndex = value;
                RaisePropertyChanged("SensorIndex");
            }
        }
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name == value)
                    return;
                _Name = value;
                RaisePropertyChanged("Name");
            }
        }
        public double ActualPosition
        {
            get
            {
                return _ActualPosition;
            }
            set
            {
                if (_ActualPosition == value)
                    return;
                _ActualPosition = value;
                RaisePropertyChanged("ActualPosition");
            }
        }
        public ObservableCollection<SensorDBHistory> History { get; set; }

        public SensorDBItem()
        {
            SensorIndex = -1;
            History = new ObservableCollection<SensorDBHistory>();
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
