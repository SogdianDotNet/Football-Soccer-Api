using System;
using System.Collections.Generic;
using System.Text;

namespace FL.Shared.DataContracts
{
    public class LeagueDC
    {
        public LeagueDC()
        {
            Teams = new List<TeamDC>();
            Matches = new List<MatchDC>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int SeasonId { get; set; }

        public List<TeamDC> Teams { get; set; }

        public List<MatchDC> Matches { get; set; }

        public SeasonDC Season { get; set; }
    }
}
