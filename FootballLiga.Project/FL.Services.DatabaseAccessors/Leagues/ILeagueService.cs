using FL.Shared.DataContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FL.Services.DatabaseAccessors.Leagues
{
    public interface ILeagueService
    {
        Task<LeagueDC> GetLeagueById(int id);
        Task<List<LeagueDC>> GetLeagues();
        Task<List<LeagueDC>> GetLeaguesByCountryId(int countryId);
        Task<bool> InsertLeagues(LeagueDC[] leagues);
    }
}
