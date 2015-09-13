using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZondBase
{
    class RoadViewModel
    {
        private RoadDBItem _Model;

        public string DisplayName
        {
            get
            {
                return string.Format("{0} (Зондов: {1})", _Model.Наименование, _Model.Probes.Count);
            }
        }

        public RoadViewModel(RoadDBItem model)
        {
            _Model = model;
        }
    }
}
