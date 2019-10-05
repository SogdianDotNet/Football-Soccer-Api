using System;
using System.Collections.Generic;
using System.Text;

namespace FL.HelperConsoleApp.RetrieveData.Model
{
    [Serializable]
    public class Competition
    {
        public int id { get; set; }
        public Area area { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string emblemUrl { get; set; }
        public string plan { get; set; }
        public CurrentSeason currentSeason { get; set; }
        public int numberOfAvailableSeasons { get; set; }
        public DateTime lastUpdated { get; set; }
    }
}
