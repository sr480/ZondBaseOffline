using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZondBase
{
    class EditHistoryViewModel
    {
        SensorDBHistory _history;
        private EditHistoryItem _view;
        
        public Command Ok { get; private set; }
        public Command Cancel { get; private set; }
        public EditNodeResult Result { get; private set; }

        public DateTime TimeStamp { get; set; }
        public int Sensor1Value { get; set; }
        public int Sensor2Value { get; set; }
        public int Temperature { get; set; }

        public EditHistoryViewModel(SensorDBHistory history)
        {
            _history = history;

            Sensor1Value = _history.Sensor1Value;
            Sensor2Value = _history.Sensor2Value;
            Temperature = _history.Temperature;
            TimeStamp = _history.TimeStamp;

            _view = new EditHistoryItem();
            _view.DataContext = this;
            Ok = new Command((x) => OkAction(), (x) => true);
            Cancel = new Command((x) => CancelAction(), (x) => true);
        }

        public void Show()
        {
            _view.ShowDialog();
        }

        private void OkAction()
        {
            _history.Sensor1Value = Sensor1Value;
            _history.Sensor2Value = Sensor2Value;
            _history.Temperature = Temperature;
            _history.TimeStamp = TimeStamp;
            Result = EditNodeResult.Ok;
            _view.Close();
        }
        private void CancelAction()
        {
            _view.Close();
        }
    }
}
