using System;
using System.Collections.Generic;
using System.Text;

namespace FL.Shared.DataContracts
{
    public class TeamDC
    {
        public TeamDC()
        {
            HomeMatches = new List<MatchDC>();
            AwayMatches = new List<MatchDC>();
            Players = new List<PlayerDC>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public int LeagueId { get; set; }

        public LeagueDC League { get; set; }

        public List<MatchDC> HomeMatches { get; set; }
        public List<MatchDC> AwayMatches { get; set; }
        public List<PlayerDC> Players { get; set; }
    }
}
