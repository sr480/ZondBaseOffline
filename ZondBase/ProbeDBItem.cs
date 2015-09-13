using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ZondBase
{
    [Serializable]
    public class ProbeDBItem : INotifyPropertyChanged
    {
        private string _»м€«онда;
        public string »м€«онда
        {
            get
            {
                return _»м€«онда;
            }
            set
            {
                if (_»м€«онда == value)
                    return;
                _»м€«онда = value;
                RaisePropertyChanged("»м€«онда");
            }
        }
        public ObservableCollection<SensorDBItem> Sensors { get; set; }

        public ProbeDBItem()
        {
            Sensors = new ObservableCollection<SensorDBItem>();
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
