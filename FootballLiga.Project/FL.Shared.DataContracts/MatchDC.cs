using System;
using System.Collections.Generic;
using System.Text;

namespace FL.Shared.DataContracts
{
    public class MatchDC
    {
        public long Id { get; set; }

        public int GoalsHome { get; set; }

        public int GoalsAway { get; set; }
        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public TeamDC HomeTeam { get; set; }

        public TeamDC AwayTeam { get; set; }
    }
}
