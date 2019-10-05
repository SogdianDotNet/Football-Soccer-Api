using System;
using System.Collections.Generic;
using System.Text;

namespace FL.Shared.DataContracts
{
    public class SeasonDC
    {
        public SeasonDC()
        {
            Leagues = new List<LeagueDC>();
        }

        public int Id { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<LeagueDC> Leagues { get; set; }

        public string SeasonYear
        {
            get
            {
                return $"{FromDate.Year}/{ToDate.Year}";
            }
        }
    }
}
