using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FL.Common.Base.Data;
using FL.Common.Base.Extensions;
using FL.Data.Domain.Entities;
using FL.Shared.DataContracts;
using Microsoft.Extensions.Logging;

namespace FL.Services.DatabaseAccessors.Leagues
{
    public class LeagueService : ILeagueService
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly IRepository<LeagueEntity> _leaguesRepository;

        #endregion

        #region Ctor

        public LeagueService(ILogger<LeagueService> logger, IRepository<LeagueEntity> repository)
        {
            _logger = logger;
            _leaguesRepository = repository;
        }

        #endregion

        public Task<LeagueDC> GetLeagueById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<LeagueDC>> GetLeagues()
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation($"Starting of method {this.GetCallerMemberName()} in class {GetType().Name}.");
            try
            {
                var leagues = RetrieveLeagues();

                if (leagues == null || !leagues.Any())
                    return Task.FromResult(new List<LeagueDC>());

                return Task.FromResult(MapEntitiesToDataContracts(leagues.ToArray()).ToList());
            }
            catch (Exception exception)
            {
                _logger.LogError($"An error has occurred in {this.GetCallerMemberName()} in class {GetType().Name}.", exception);
                throw;
            }
            finally
            {
                watch.Stop();
                _logger.LogInformation($"Finished method {this.GetCallerMemberName()} in class {GetType().Name} in {watch.Elapsed.TotalSeconds} seconds.");
            }
        }

        public Task<List<LeagueDC>> GetLeaguesByCountryId(int countryId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertLeagues(LeagueDC[] leagues)
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation($"Starting of method {this.GetCallerMemberName()} in class {GetType().Name}.");
            try
            {
                foreach (var league in leagues)
                {

                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"An error has occurred in {this.GetCallerMemberName()} in class {GetType().Name}.", exception);
                throw;
            }
            finally
            {
                watch.Stop();
                _logger.LogInformation($"Finished method {this.GetCallerMemberName()} in class {GetType().Name} in {watch.Elapsed.TotalSeconds} seconds.");
            }
            return null;
        }

        #region Private methods

        private IEnumerable<LeagueEntity> RetrieveLeagues(Expression<Func<LeagueEntity, bool>> filter = null)
        {
            return _leaguesRepository.GetList(filter, x => x.Season, x => x.Teams, x => x.Matches);
        }

        private IEnumerable<LeagueDC> MapEntitiesToDataContracts(LeagueEntity[] leagues)
        {
            foreach (var league in leagues)
            {
                yield return new LeagueDC
                {
                    Id = league.Id,
                    Name = league.Name,
                    SeasonId = league.SeasonId,
                    Matches = league.Matches.Select(m => new MatchDC
                    {
                        Id = m.Id,
                        HomeTeamId = m.HomeTeamId,
                        AwayTeamId = m.AwayTeamId,
                        GoalsHome = m.GoalsHome,
                        GoalsAway = m.GoalsAway,
                        HomeTeam = new TeamDC
                        {
                            Id = m.HomeTeam.Id,
                            Name = m.HomeTeam.Name
                        },
                        AwayTeam = new TeamDC
                        {
                            Id = m.AwayTeam.Id,
                            Name = m.AwayTeam.Name
                        }
                    }).ToList(),
                    Season = new SeasonDC
                    {
                        Id = league.Season.Id,
                        ToDate = league.Season.ToDate,
                        FromDate = league.Season.FromDate
                    },
                    Teams = league.Teams.Select(t => new TeamDC
                    {
                        Id = t.Id,
                        Name = t.Name
                    }).ToList()
                };
            }
        }

        #endregion
    }
}
