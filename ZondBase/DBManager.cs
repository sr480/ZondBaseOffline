using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows;
using System.ComponentModel;

namespace ZondBase
{
    class DBManager : INotifyPropertyChanged
    {
        private RoadDataBase _DataBase;
        private const string DEFAULTEXTENSION = ".rdbml";
        private const string DEFAULTFILTER = "Файл базы данных дорог (.rdbml)|*.rdbml";
        private XmlSerializer _Serializer;

        public Command Open { get; private set; }
        public Command Save { get; private set; }
        public Command SaveAs { get; private set; }

        public RoadDataBase DataBase
        {
            get
            {
                return _DataBase;
            }
            private set
            {
                if (_DataBase == value)
                    return;
                _DataBase = value;
                RaisePropertyChanged("DataBase");
            }
        }
        public string FileName { get; set; }
        public bool IsSaved { get; set; }

        public DBManager()
        {
            Open = new Command((x) => OpenAction(), (x) => true);
            Save = new Command((x) => SaveAction(), (x) => true);
            SaveAs = new Command((x) => SaveAsAction(), (x) => true);
            _Serializer = new XmlSerializer(typeof(RoadDataBase));
            DataBase = new RoadDataBase();
        }

        private void OpenAction()
        {
            if (!IsSaved)
                if (SynchronisationHelper.ShowMessage("Не сохраненные данные будут утрачены. Продолжить?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;
            Microsoft.Win32.OpenFileDialog opDlg = new Microsoft.Win32.OpenFileDialog();
            opDlg.DefaultExt = DEFAULTEXTENSION;
            opDlg.Filter = DEFAULTFILTER;
            if (opDlg.ShowDialog() == true)
                OpenFile(opDlg.FileName);
        }
        private void SaveAction()
        {
            if (string.IsNullOrEmpty(FileName))
                SaveAsAction();
            else
                SaveFile(FileName);
        }
        private void SaveAsAction()
        {
            Microsoft.Win32.SaveFileDialog svDlg = new Microsoft.Win32.SaveFileDialog();
            svDlg.DefaultExt = DEFAULTEXTENSION;
            svDlg.Filter = DEFAULTFILTER;
            if (svDlg.ShowDialog() == true)
                SaveFile(svDlg.FileName);
        }

        private bool OpenFile(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    DataBase = (RoadDataBase)_Serializer.Deserialize(fs);
                    FileName = path;
                    IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                SynchronisationHelper.ShowMessage(ex.Message, "Ошибка открытия", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool SaveFile(string path)
        {
            try
            {
                using (TextWriter fs = new StreamWriter(path))
                {
                    _Serializer.Serialize(fs, DataBase);
                    FileName = path;
                    IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                SynchronisationHelper.ShowMessage(ex.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
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
