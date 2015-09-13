using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ZondBase
{
    [Serializable]
    public class RoadDBItem : INotifyPropertyChanged
    {
        private string _Наименование;
        public string Наименование
        {
            get
            {
                return _Наименование;
            }
            set
            {
                if (_Наименование == value)
                    return;
                _Наименование = value;
                RaisePropertyChanged("Наименование");
            }
        }
        public ObservableCollection<ProbeDBItem> Probes { get; set; }

        public RoadDBItem()
        {
            Probes = new ObservableCollection<ProbeDBItem>();
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
