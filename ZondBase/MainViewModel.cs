using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Windows;

namespace ZondBase
{
    class MainViewModel : INotifyPropertyChanged
    {
        private MeassuresViewModel _Meassures;
        private SensorDBHistoryViewModel _CurrentHistory;
        private SensorDBItem _CurrentSensor;
        private ProbeDBItem _CurrentProbe;
        private RoadDBItem _CurrentRoad;
        private DBManager _DataBaseManager;
        private SensorsDB _SensorsDB;
        private CommunicationService _comService;

        public RoadDBItem CurrentRoad
        {
            get { return _CurrentRoad; }
            set
            {
                if (_CurrentRoad == value)
                    return;
                _CurrentRoad = value;
                CurrentProbe = _CurrentRoad.Probes.FirstOrDefault();
                DeleteRoad.RaiseCanExecuteChanged();
                EditRoad.RaiseCanExecuteChanged();
                AddProbe.RaiseCanExecuteChanged();
                RaisePropertyChanged("CurrentRoad");
            }
        }
        public ProbeDBItem CurrentProbe
        {
            get { return _CurrentProbe; }
            set
            {
                if (_CurrentProbe == value)
                    return;
                _CurrentProbe = value;
                DeleteProbe.RaiseCanExecuteChanged();
                EditProbe.RaiseCanExecuteChanged();
                AddSensor.RaiseCanExecuteChanged();
                Graph.SelectedProbe = value;
                RaisePropertyChanged("CurrentProbe");
            }
        }
        public SensorDBItem CurrentSensor
        {
            get { return _CurrentSensor; }
            set
            {
                if (_CurrentSensor == value)
                    return;

                AttachToHistoryCollection(value);

                
                _CurrentSensor = value;
                EditSensor.RaiseCanExecuteChanged();
                DeleteSensor.RaiseCanExecuteChanged();
                RequestSensor.RaiseCanExecuteChanged();
                AddHistoryItem.RaiseCanExecuteChanged();
                RaisePropertyChanged("CurrentSensor");
            }
        }
        public ObservableCollection<SensorDBHistoryViewModel> History
        {
            get;
            private set;
        }
        public SensorDBHistoryViewModel CurrentHistory
        {
            get
            {
                return _CurrentHistory;
            }
            set
            {
                if (_CurrentHistory == value)
                    return;
                if (_CurrentHistory != null)
                    _CurrentHistory.PropertyChanged -= OnHistoryChanged;
                if (value != null)
                    value.PropertyChanged += OnHistoryChanged;
                _CurrentHistory = value;
                if (_CurrentHistory != null)
                    Meassures = new MeassuresViewModel(_CurrentSensor.SensorId, CurrentHistory.Sensor1Value, CurrentHistory.Sensor2Value, _SensorsDB);
                else
                    Meassures = new MeassuresViewModel();

                RaisePropertyChanged("CurrentHistory");
                DeleteHistoryItem.RaiseCanExecuteChanged();
                EdHistoryItem.RaiseCanExecuteChanged();
            }
        }
        public MeassuresViewModel Meassures
        {
            get
            {
                return _Meassures;
            }
            set
            {
                if (_Meassures == value)
                    return;
                _Meassures = value;
                RaisePropertyChanged("Meassures");
            }
        }
        public DBManager DataBaseManager
        {
            get
            {
                return _DataBaseManager;
            }
        }
        public CommunicationService ComService
        {
            get
            {
                return _comService;
            }
        }

        public GraphViewModel Graph { get; private set; }
        #region Commands
        public Command AddRoad { get; private set; }
        public Command DeleteRoad { get; private set; }
        public Command EditRoad { get; private set; }

        public Command AddProbe { get; private set; }
        public Command DeleteProbe { get; private set; }
        public Command EditProbe { get; private set; }

        public Command AddSensor { get; private set; }
        public Command DeleteSensor { get; private set; }
        public Command EditSensor { get; private set; }

        public Command DeleteHistoryItem { get; private set; }

        public Command RequestSensor { get; private set; }
        public Command AddHistoryItem { get; private set; }
        public Command EdHistoryItem { get; private set; }
        public Command OpenTare { get; private set; }
        private void InitializeCommands()
        {
            AddRoad = new Command((x) => AddRoadAction(), (x) => true);
            DeleteRoad = new Command((x) => DeleteRoadAction(), (x) => CurrentRoad != null);
            EditRoad = new Command((x) => EditRoadAction(), (x) => CurrentRoad != null);

            AddProbe = new Command((x) => AddProbeAction(), (x) => CurrentRoad != null);
            DeleteProbe = new Command((x) => DeleteProbeAction(), (x) => CurrentProbe != null);
            EditProbe = new Command((x) => EditProbeAction(), (x) => CurrentProbe != null);

            AddSensor = new Command((x) => AddSensorAction(), (x) => CurrentProbe != null);
            DeleteSensor = new Command((x) => DeleteSensorAction(), (x) => CurrentSensor != null);
            EditSensor = new Command((x) => EditSensorAction(), (x) => CurrentSensor != null);

            DeleteHistoryItem = new Command((x) => DeleteHistoryItemAction(), (x) => CurrentHistory != null);

            RequestSensor = new Command((x) => ComService.AskSensor(CurrentSensor), (x) => CurrentSensor != null);
            AddHistoryItem = new Command((x) => AddHistoryItemAction(), (x) => CurrentSensor != null);
            EdHistoryItem = new Command((x) => EditHistoryItemAction(), (x) => CurrentHistory != null);

            OpenTare = new Command((x) => OpenSensorDBAction(), (x)=> true);
        }
        #endregion

        public MainViewModel()
        {
            _DataBaseManager = new DBManager();
            _comService = new CommunicationService();
            History = new ObservableCollection<SensorDBHistoryViewModel>();
            Graph = new GraphViewModel();
            InitializeCommands();
            //FillTestDB();
        }
        private void FillTestDB()
        {
            var r1 = new RoadDBItem { Наименование = "Ростов-Краснодар" };

            var p1 = new ProbeDBItem { ИмяЗонда = "Зонд 1" };
            p1.Sensors.Add(new SensorDBItem { SensorId = 331, SensorIndex = 1, Name = "т.б.", ActualPosition = -120 });

            var s2 = new SensorDBItem { SensorId = 332, SensorIndex = 2, Name = "гр.", ActualPosition = -32 };
            s2.History.Add(new SensorDBHistory { Sensor1Value = 9578, Sensor2Value = 7562, Temperature = 23, Position = 897.32d, TimeStamp = DateTime.Now.AddMonths(-1) });
            s2.History.Add(new SensorDBHistory { Sensor1Value = 10780, Sensor2Value = 9578, Temperature = 12, Position = 898.32d, TimeStamp = DateTime.Now.AddMonths(-2) });
            s2.History.Add(new SensorDBHistory { Sensor1Value = 13890, Sensor2Value = 11234, Temperature = 17, Position = 899.32d, TimeStamp = DateTime.Now.AddMonths(-3) });
            p1.Sensors.Add(s2);

            r1.Probes.Add(p1);
            r1.Probes.Add(new ProbeDBItem { ИмяЗонда = "Зонд 2" });

            var r2 = new RoadDBItem { Наименование = "Ростов-Таганрог" };
            r2.Probes.Add(new ProbeDBItem { ИмяЗонда = "Зонд 1" });
            r2.Probes.Add(new ProbeDBItem { ИмяЗонда = "Зонд 2" });
            r2.Probes.Add(new ProbeDBItem { ИмяЗонда = "Зонд 3" });
            r2.Probes.Add(new ProbeDBItem { ИмяЗонда = "Зонд 4" });

            _DataBaseManager.DataBase.Roads.Add(r1);
            _DataBaseManager.DataBase.Roads.Add(r2);           
        }

        public bool OnClosing()
        {
            var result = SynchronisationHelper.ShowMessage("Сохранить изменения?", "Закрытие...", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.Cancel)
                return false;
            if (result == MessageBoxResult.Yes)
                DataBaseManager.Save.Execute(null);
            return true;
        }
        private void OnHistoryChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (var oi in e.OldItems)
                    History.Remove(History.Single(h => h.SensorHistory == (SensorDBHistory)oi));
            if (e.NewItems != null)
                foreach (var ni in e.NewItems)
                    History.Add(new SensorDBHistoryViewModel(CurrentSensor, (SensorDBHistory)ni));
        }
        private void AttachToHistoryCollection(SensorDBItem value)
        {
            History.Clear();
            if (_CurrentSensor != null)
                _CurrentSensor.History.CollectionChanged -= OnHistoryChanged;
            if (value != null)
            {
                value.History.CollectionChanged += OnHistoryChanged;
                foreach (var item in value.History.OrderBy(i => i.TimeStamp))
                    History.Add(new SensorDBHistoryViewModel(value, item));
            }
        }
        private void OnHistoryChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Position")
                foreach (var item in History)
                    item.UpdateItem();
        }
        private void OpenSensorDBAction()
        {
            Microsoft.Win32.OpenFileDialog opDlg = new Microsoft.Win32.OpenFileDialog();
            opDlg.DefaultExt = ".sndbml";
            opDlg.Filter = "Файл базы данных тарирования (.sndbml)|*.sndbml";
            if (opDlg.ShowDialog() == true)
            {
                XmlSerializer s = new XmlSerializer(typeof(SensorsDB));

                using (FileStream fs = new FileStream(opDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    _SensorsDB = (SensorsDB)s.Deserialize(fs);
                }
            }
        }

        #region CommandActions
        private void AddRoadAction()
        {
            var r = new RoadDBItem();
            r.Наименование = "Новая дорога";
            EditNodeViewModel vm = new EditNodeViewModel(r);
            vm.Show();
            if (vm.Result == EditNodeResult.Ok)
            {
                _DataBaseManager.DataBase.Roads.Add(r);
                CurrentRoad = r;
            }
        }
        private void DeleteRoadAction()
        {
            if (SynchronisationHelper.ShowMessage("Вы уверены, что хотите удалить дорогу?", "Удаление...", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                _DataBaseManager.DataBase.Roads.Remove(CurrentRoad);
        }
        private void EditRoadAction()
        {
            EditNodeViewModel vm = new EditNodeViewModel(CurrentRoad);
            vm.Show();
        }

        private void AddProbeAction()
        {
            var p = new ProbeDBItem();
            p.ИмяЗонда = "Новый зонд";
            EditNodeViewModel vm = new EditNodeViewModel(p);
            vm.Show();
            if (vm.Result == EditNodeResult.Ok)
            {
                CurrentRoad.Probes.Add(p);
                CurrentProbe = p;
            }
        }
        private void DeleteProbeAction()
        {
            if (SynchronisationHelper.ShowMessage("Вы уверены, что хотите удалить зонд?", "Удаление...", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                CurrentRoad.Probes.Remove(CurrentProbe);
        }
        private void EditProbeAction()
        {
            EditNodeViewModel vm = new EditNodeViewModel(CurrentProbe);
            vm.Show();
        }

        private void AddSensorAction()
        {
            var s = new SensorDBItem();
            var vm = new EditSensorViewModel(s, CurrentProbe);
            vm.Show();
            if (vm.Result == EditNodeResult.Ok)
            {
                CurrentProbe.Sensors.Add(s);
                CurrentSensor = s;
            }
        }
        private void AddHistoryItemAction()
        {
            var h = new SensorDBHistory();
            var vm = new EditHistoryViewModel(h);
            vm.Show();
            if (vm.Result == EditNodeResult.Ok)
            {
                CurrentSensor.History.Add(h);                
            }
        }
        private void EditHistoryItemAction()
        {
            var vm = new EditHistoryViewModel(CurrentHistory.SensorHistory);
            vm.Show();
            CurrentHistory.UpdateItemFull();
        }

        private void DeleteSensorAction()
        {
            if (SynchronisationHelper.ShowMessage("Вы уверены, что хотите удалить датчик?", "Удаление...", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                CurrentProbe.Sensors.Remove(CurrentSensor);
        }
        private void EditSensorAction()
        {
            var vm = new EditSensorViewModel(CurrentSensor, CurrentProbe);
            vm.Show();
        }
        private void DeleteHistoryItemAction()
        {
            if (SynchronisationHelper.ShowMessage("Вы уверены, что хотите удалить запись?", "Удаление...", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                CurrentSensor.History.Remove(CurrentHistory.SensorHistory);
        }
        #endregion
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
