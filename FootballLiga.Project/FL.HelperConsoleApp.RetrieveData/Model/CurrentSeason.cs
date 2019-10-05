using System;
using System.Collections.Generic;
using System.Text;

namespace FL.HelperConsoleApp.RetrieveData.Model
{
    [Serializable]
    public class CurrentSeason
    {
        public int id { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int? currentMatchday { get; set; }
    }
}
