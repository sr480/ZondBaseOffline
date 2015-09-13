﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ZondBase
{
    [Serializable()]
    public class SensorsDB
    {
        public DateTime LastUpdate { get; set; }
        public ObservableCollection<SensorInfo> Sensors { get; set; }
        public SensorsDB()
        {
            Sensors = new ObservableCollection<SensorInfo>();
        }
    }
    [Serializable()]
    public class SensorInfo
    {
        public int SensorNumber { get; set; }
        public int SensorAddress { get; set; }
        public ObservableCollection<CalibrationPoint> Points { get; set; }
        public SensorInfo()
        {
            Points = new ObservableCollection<CalibrationPoint>();
        }
    }
    [Serializable()]
    public class CalibrationPoint
    {
        public double Position { get; set; }
        public int Sensor1 { get; set; }
        public int Sensor2 { get; set; }
    }
}
