using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZondBase
{
    class GraphViewModel : INotifyPropertyChanged
    {
        private ProbeDBItem _SelectedProbe;
        public ProbeDBItem SelectedProbe
        {
            get
            {
                return _SelectedProbe;
            }
            set
            {
                if (_SelectedProbe == value)
                    return;
                _SelectedProbe = value;
                OnSelectedProbeChanged();
                RaisePropertyChanged("SelectedProbe");
            }
        }
        public ObservableCollection<GraphItemViewModel> GraphItems { get; private set; }

        public GraphViewModel()
        {
            GraphItems = new ObservableCollection<GraphItemViewModel>();
        }
        private void OnSelectedProbeChanged()
        {
            GraphItems.Clear();
            if (SelectedProbe == null)
                return;
            if (SelectedProbe.Sensors.Count < 2)
                return;

            for (int i = 1; i < SelectedProbe.Sensors.Count; i++)    //Начиная со второго сенсора получаем данные
            {
                if (SelectedProbe.Sensors[i - 1].History.Count == 0 | //Если нет данных об истории, то заполним график нулями
                    SelectedProbe.Sensors[i].History.Count == 0)
                {
                    GraphItems.Add(new GraphItemViewModel(SelectedProbe.Sensors[i].Name, 0));
                    continue;
                }
                //Берем вью модели элементов истории, так как там есть подсчет нужных значений.
                //Из истории берем последние элементы
                var hi_ps = new SensorDBHistoryViewModel(SelectedProbe.Sensors[i - 1],
                    SelectedProbe.Sensors[i - 1].History.Last());       
                var hi_cs = new SensorDBHistoryViewModel(SelectedProbe.Sensors[i],
                    SelectedProbe.Sensors[i].History.Last());

                GraphItems.Add(new GraphItemViewModel(SelectedProbe.Sensors[i].Name,
                    hi_cs.FullDeformation - hi_ps.FullDeformation));
            }
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

    class GraphItemViewModel : INotifyPropertyChanged
    {
        public string Name { get; private set; }
        public double Value { get; private set; }
        public GraphItemViewModel(string name, double value)
        {
            Name = name;
            Value = Math.Round(value, 2);
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
