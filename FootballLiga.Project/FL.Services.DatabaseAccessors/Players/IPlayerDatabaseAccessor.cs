using FL.Shared.DataContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FL.Services.DatabaseAccessors.Players
{
    public interface IPlayerDatabaseAccessor
    {
        Task<PlayerDC> GetPlayerById(long id);
        Task<List<PlayerDC>> GetPlayersByTeamId(int teamId);
        Task<List<PlayerDC>> GetPlayersByCountryId(int countryId);
        Task<bool> InsertPlayers(PlayerDC[] playersToInsert);
    }
}
