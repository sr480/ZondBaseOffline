using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace ZondBase
{
    public class MeassuresViewModel : INotifyPropertyChanged
    {
        private Visibility _MeassureVisibility;
        public Visibility MeassureVisibility
        {
            get
            {
                return _MeassureVisibility;
            }
            set
            {
                if (_MeassureVisibility == value)
                    return;
                _MeassureVisibility = value;
                RaisePropertyChanged("MeassureVisibility");
            }
        }

        public IEnumerable<double> S1Points
        { get; private set; }
        public IEnumerable<double> S2Points
        { get; private set; }

        public MeassuresViewModel()
        {
            MeassureVisibility = Visibility.Visible;
        }
        public MeassuresViewModel(int sensorID, int s1, int s2, SensorsDB db)
        {
            if (db == null)
            {
                MeassureVisibility = Visibility.Visible;
                return;
            }

            MeassureVisibility = Visibility.Visible;

            var sensor = db.Sensors.SingleOrDefault(s => s.SensorAddress == sensorID);
            if (sensor == null)
                return;

            MeassureVisibility = Visibility.Hidden;
            var pts = sensor.Points.OrderBy(p => p.Position).ToList();
            List<double> s1ps = new List<double>();
            List<double> s2ps = new List<double>();

            for (int i = 1; i < pts.Count; i++)
            {
                if (pts[i].Sensor1 < s1 & pts[i - 1].Sensor1 > s1 |
                    pts[i].Sensor1 > s1 & pts[i - 1].Sensor1 < s1)
                    s1ps.Add(CountPosition(
                        pts[i - 1].Sensor1,
                        pts[i].Sensor1,
                        pts[i - 1].Position,
                        pts[i].Position,
                        s1));

                if (pts[i].Sensor2 < s2 & pts[i - 1].Sensor2 > s2 |
                    pts[i].Sensor2 > s2 & pts[i - 1].Sensor2 < s2)
                    s2ps.Add(CountPosition(
                        pts[i - 1].Sensor2,
                        pts[i].Sensor2,
                        pts[i - 1].Position,
                        pts[i].Position,
                        s2));
            }
            S1Points = s1ps;
            S2Points = s2ps;
        }
        private double CountPosition(int s1, int s2, double x1, double x2, int s)
        {
            return Math.Round(Math.Abs((double)(s - s1)) * Math.Abs(x2 - x1) / Math.Abs((double)(s2 - s1)) + x1, 2);
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
