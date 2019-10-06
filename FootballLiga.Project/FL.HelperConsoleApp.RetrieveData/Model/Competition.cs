using System;
using System.Collections.Generic;
using System.Text;

namespace FL.HelperConsoleApp.RetrieveData.Model
{
    [Serializable]
    public class Competition
    {
        public string id { get; set; }
        public string name { get; set; }
        public string country_id { get; set; }
    }
}
