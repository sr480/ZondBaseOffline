using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZondBase
{
    class ProbeViewModel
    {
        private ProbeDBItem _Model;

        public string DisplayName
        {
            get
            {
                return string.Format("{0} (Датчиков: {1})", _Model.ИмяЗонда, _Model.Sensors.Count);
            }
        }

        public ProbeViewModel(ProbeDBItem model)
        {
            _Model = model;
        }
    }
}
