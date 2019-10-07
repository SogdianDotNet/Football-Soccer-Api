using System;
using System.Collections.Generic;
using System.Text;

namespace FL.HelperConsoleApp.RetrieveData
{
    public class ConfigurationSettings
    {
        public string ApiKey { get; set; }
        public string AppSecret { get; set; }
        public string AzureSqlDatabase { get; set; }
        public string GetListOfLeagues { get; set; }
        public string GetListOfCountries { get; set; }
        public string GetCompetitionsByCountry { get; set; }
        public string GetListOfTeamsByCountry { get; set; }
        public string GetListOfPastMatches { get; set; }
        public string GetListOfMatchesOfCompetition { get; set; }

    }
}
