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
        public CompCountry[] countries { get; set; }
    }

    public class CompCountry
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
