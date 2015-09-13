using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace ZondBase
{
    [Serializable]
    public class RoadDataBase
    {
        public ObservableCollection<RoadDBItem> Roads { get; set; }
        public RoadDataBase()
        {
            Roads = new ObservableCollection<RoadDBItem>();
        }
    }
}
