using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FL.Common.Base.Configuration;
using FL.Common.Base.Exceptions;
using FL.Services.DatabaseAccessors.ApiKeys;
using FL.Services.DatabaseAccessors.Players;
using FL.Shared.ClientHttp;
using FL.Shared.ClientHttp.Responses;
using FL.Shared.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FootballLiga.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : BaseController
    {
        private readonly IPlayerService _playerService; 

        public PlayersController(
            ILogger<PlayersController> logger,
            AppConfiguration configuration,
            IAuthenticationService authenticationService, 
            IPlayerService playerService) : base (logger, configuration, authenticationService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        [Route("GetPlayer/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDC<PlayerResponseDC>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDC<PlayerResponseDC>>> GetPlayer(long id)
        {
            var response = await CreateResponse<PlayerResponseDC>();
            try
            {
                if (!response.IsValid)
                {
                    await WriteResponse(response);
                    return Unauthorized(response);
                }

                var player = await GetPlayerByIdAsync(id);

                if (player == null)
                {
                    response.Status = SUCCES_STATUS;
                    response.ErrorMessages = new List<ErrorMessageDC> { new ErrorMessageDC { Message = "No player found.", Description = "No player exists with the provided id." } };
                    return NotFound(response);
                }

                if (player != null)
                {
                    response.Value = MapDataContractToResponse(new[] { player }).FirstOrDefault();
                    response.Status = SUCCES_STATUS;
                    response.TotalSize = 1;
                    response.ErrorMessages = null;
                    return Ok(response);
                }
            }
            catch (AuthenticationServiceException authException)
            {
                _logger.LogError($"An error has occurred in the authentication service.", authException);
            }
            catch (DatabaseServiceException dsException)
            {
                _logger.LogError($"An error has occurred in the database service.", dsException);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An error has occurred.", exception);
            }

            return response;
        }

        [HttpGet]
        [Route("GetPlayersByTeamId/{teamId}")]

        [ProducesResponseType(typeof(ResponseDC<List<PlayerResponseDC>>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDC<List<PlayerResponseDC>>>> GetPlayersByTeamId(int teamId)
        {
            var response = await CreateResponse<List<PlayerResponseDC>>();
            try
            {
                if (!response.IsValid)
                {
                    await WriteResponse(response);
                    return Unauthorized(response);
                }

                var players = await GetPlayersByTeamIdAsync(teamId);

                if (players == null || !players.Any())
                {
                    response.Status = SUCCES_STATUS;
                    response.ErrorMessages = new List<ErrorMessageDC> { new ErrorMessageDC { Message = "No players found.", Description = $"No players found with the provided {nameof(teamId)}." } };
                    return NotFound(response);
                }

                if (players != null && players.Any())
                {
                    response.Value = MapDataContractToResponse(players.ToArray()).ToList();
                    response.Status = SUCCES_STATUS;
                    response.TotalSize = response.Value.Count;
                    response.ErrorMessages = null;
                    return Ok(response);
                }
            }
            catch (AuthenticationServiceException authException)
            {
                _logger.LogError($"An error has occurred in the authentication service.", authException);
            }
            catch (DatabaseServiceException dsException)
            {
                _logger.LogError($"An error has occurred in the database service.", dsException);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An error has occurred.", exception);
            }

            return response;
        }

        #region Private methods
        
        private async Task<PlayerDC> GetPlayerByIdAsync(long id)
        {
            try
            {
                return await _playerService.GetPlayerById(id);
            }
            catch (Exception exception)
            {
                throw new DatabaseServiceException();
            }
        }

        private async Task<List<PlayerDC>> GetPlayersByTeamIdAsync(int teamId)
        {
            try
            {
                return await _playerService.GetPlayersByTeamId(teamId);
            }
            catch (Exception exception)
            {
                throw new DatabaseServiceException();
            }
        }

        private IEnumerable<PlayerResponseDC> MapDataContractToResponse(PlayerDC[] players)
        {
            foreach (var player in players)
            {
                yield return new PlayerResponseDC
                {
                    Id = player.Id,
                    Firstname = player.Firstname,
                    Lastname = player.Lastname,
                    TeamId = player.TeamId,
                    NationalityId = player.NationalityId
                };
            }
        }

        #endregion
    }
}