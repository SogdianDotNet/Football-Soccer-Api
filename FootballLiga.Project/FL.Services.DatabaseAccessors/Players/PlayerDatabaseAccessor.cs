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

namespace FL.Services.DatabaseAccessors.Players
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayerDatabaseAccessor : IPlayerDatabaseAccessor
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly IRepository<PlayerEntity> _playersRepository;

        #endregion

        #region Ctor

        public PlayerDatabaseAccessor(
            ILogger<PlayerDatabaseAccessor> logger,
            IRepository<PlayerEntity> playersRepository)
        {
            _logger = logger;
            _playersRepository = playersRepository;
        }

        #endregion

        #region Methods

        public Task<PlayerDC> GetPlayerById(long id)
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation($"Starting of method {this.GetCallerMemberName()} in class {GetType().Name}.");
            try
            {
                if (id <= 0)
                {
                    _logger.LogError($"{nameof(id)} may not be less than or equal to zero");
                    throw new ArgumentOutOfRangeException(nameof(id));
                }

                PlayerEntity player = _playersRepository.FindBy(pl => pl.Id == id);

                if (player == null)
                    throw new ArgumentNullException(nameof(player));

                return Task.FromResult(MapEntitiesToDataContracts(new PlayerEntity[] { player }).FirstOrDefault());
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

        public Task<List<PlayerDC>> GetPlayersByCountryId(int countryId)
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation($"Starting of method {this.GetCallerMemberName()} in class {GetType().Name}.");
            try
            {
                if (countryId <= 0)
                {
                    _logger.LogError($"{nameof(countryId)} may not be less than or equal to zero");
                    throw new ArgumentOutOfRangeException(nameof(countryId));
                }

                IEnumerable<PlayerEntity> players = GetPlayers(pl => pl.NationalityId == countryId);

                if (players == null || !players.Any())
                    return Task.FromResult(new List<PlayerDC>());

                return Task.FromResult(MapEntitiesToDataContracts(players.ToArray()).ToList());
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

        public Task<List<PlayerDC>> GetPlayersByTeamId(int teamId)
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation($"Starting of method {this.GetCallerMemberName()} in class {GetType().Name}.");
            try
            {
                if (teamId <= 0)
                {
                    _logger.LogError($"{nameof(teamId)} may not be less than or equal to zero");
                    throw new ArgumentOutOfRangeException(nameof(teamId));
                }

                IEnumerable<PlayerEntity> players = GetPlayers(pl => pl.TeamId == teamId);

                if (players == null || !players.Any())
                    return Task.FromResult(new List<PlayerDC>());

                return Task.FromResult(MapEntitiesToDataContracts(players.ToArray()).ToList());
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

        public Task<bool> InsertPlayers(PlayerDC[] playersToInsert)
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation($"Starting of method {this.GetCallerMemberName()} in class {GetType().Name}.");
            try
            {
                var insertablePlayers = new List<PlayerDC>();
                IEnumerable<PlayerEntity> existingPlayers = GetPlayers();

                foreach (var player in playersToInsert)
                    if (!existingPlayers.Any(pl => pl.Firstname == player.Firstname && pl.Lastname == player.Lastname))
                        insertablePlayers.Add(player);

                _playersRepository.Insert(MapDataContractsToEntities(insertablePlayers.ToArray()));

                return Task.FromResult(true);
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

        #endregion

        #region Private methods

        private IEnumerable<PlayerEntity> GetPlayers(Expression<Func<PlayerEntity, bool>> filter = null)
        {
            return _playersRepository.GetList(filter, x => x.Nationality, x => x.Team);
        }

        private IEnumerable<PlayerDC> MapEntitiesToDataContracts(PlayerEntity[] players)
        {
            foreach (var player in players)
            {
                yield return new PlayerDC
                {
                    Id = player.Id,
                    Firstname = player.Firstname,
                    Lastname = player.Lastname,
                    Nationality = new CountryDC
                    {
                        Id = player.Nationality.Id,
                        Name = player.Nationality.Name,
                        ShortName = player.Nationality.ShortName
                    },
                    NationalityId = player.NationalityId,
                    Team = new TeamDC
                    {
                        Id = player.Team.Id,
                        LeagueId = player.Team.LeagueId,
                        Name = player.Team.Name
                    },
                    TeamId = player.TeamId
                };
            }
        }

        private IEnumerable<PlayerEntity> MapDataContractsToEntities(PlayerDC[] players)
        {
            foreach (var player in players)
            {
                yield return new PlayerEntity
                {
                    Firstname = player.Firstname,
                    Lastname = player.Lastname,
                    NationalityId = player.NationalityId,
                    TeamId = player.TeamId
                };
            }
        }

        #endregion
    }
}
