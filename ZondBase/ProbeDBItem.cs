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
        private string _��������;
        public string ��������
        {
            get
            {
                return _��������;
            }
            set
            {
                if (_�������� == value)
                    return;
                _�������� = value;
                RaisePropertyChanged("��������");
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
