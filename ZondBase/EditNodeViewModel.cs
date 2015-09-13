using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ZondBase
{
    class EditNodeViewModel
    {
        object _model;
        EditNode _view;
        public string Name
        {
            get;
            set;
        }

        public Command Ok { get; private set; }
        public Command Cancel { get; private set; }
        public EditNodeResult Result { get; private set; }
        public EditNodeViewModel(object model)
        {
            _model = model;
            if (_model is RoadDBItem)
                Name = ((RoadDBItem)_model).Наименование;
            else if (_model is ProbeDBItem)
                Name = ((ProbeDBItem)_model).ИмяЗонда;

            _view = new EditNode();
            _view.DataContext = this;
            Result = EditNodeResult.Null;
            Ok = new Command((x) => OkAction(), (x) => true);
            Cancel = new Command((x) => CancelAction(), (x) => true);
        }
        public void Show()
        {
            _view.ShowDialog();
        }
        private bool ValidateModel()
        {
            if (_model is RoadDBItem && string.IsNullOrEmpty(((RoadDBItem)_model).Наименование))
            {
                SynchronisationHelper.ShowMessage(_view, "Наименование не может быть пустым", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
                return false;
            }
            else if (_model is ProbeDBItem && string.IsNullOrEmpty(((ProbeDBItem)_model).ИмяЗонда))
            {
                SynchronisationHelper.ShowMessage(_view, "Наименование не может быть пустым", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
                return false;
            }
            return true;
        }
        private void OkAction()
        {
            if (!ValidateModel())
                return;
            if (_model is RoadDBItem)
                ((RoadDBItem)_model).Наименование = Name;
            else if (_model is ProbeDBItem)
                ((ProbeDBItem)_model).ИмяЗонда = Name;

            Result = EditNodeResult.Ok;
            _view.Close();
        }
        private void CancelAction()
        {
            Result = EditNodeResult.Cancel;
            _view.Close();
        }
    }
}
