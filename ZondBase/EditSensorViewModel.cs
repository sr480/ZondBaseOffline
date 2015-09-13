using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ZondBase
{
    class EditSensorViewModel
    {
        ProbeDBItem _probe;
        SensorDBItem _sensor;
        EditSensor _view;
        List<int> _availableIndexes;

        public Command Ok { get; private set; }
        public Command Cancel { get; private set; }
        public EditNodeResult Result { get; private set; }
        public IEnumerable<int> SensorIndexes { get { return _availableIndexes; } }
        public int SensorIndex { get; set; }
        public int SensorID { get; set; }
        public string Name { get; set; }
        public double ActualPosition { get; set; }

        public EditSensorViewModel(SensorDBItem sensor, ProbeDBItem probe)
        {
            if (sensor == null)
                throw new ArgumentNullException("sensor", "sensor is null.");
            if (probe == null)
                throw new ArgumentNullException("probe", "probe is null.");

            _probe = probe;
            _sensor = sensor;
            _availableIndexes = CalculateIndexes();
            SensorID = _sensor.SensorId == -1 ? _availableIndexes.FirstOrDefault() : _sensor.SensorId;
            SensorIndex = _sensor.SensorIndex;
            Name = sensor.Name;
            ActualPosition = sensor.ActualPosition;
            _view = new EditSensor();
            _view.DataContext = this;
            Ok = new Command((x) => OkAction(), (x) => true);
            Cancel = new Command((x) => CancelAction(), (x) => true);
        }

        public void Show()
        {
            _view.ShowDialog();
        }
        private List<int> CalculateIndexes()
        {
            var result = new List<int>();
            for (int i = 1; i <= 7; i++)
                if (!_probe.Sensors.Any(s => s.SensorIndex == i & s != _sensor))
                    result.Add(i);

            return result;
        }
        private void OkAction()
        {
            _sensor.SensorId = SensorID;
            _sensor.SensorIndex = SensorIndex;
            _sensor.Name = Name;
            _sensor.ActualPosition = ActualPosition;
            Result = EditNodeResult.Ok;
            _view.Close();
        }
        private void CancelAction()
        {
            _view.Close();
        }
    }
}
