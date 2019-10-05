using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using FL.Common.Base.Data;
using FL.Common.Base.Extensions;
using FL.Data.Domain.Entities.Authentication;
using FL.Shared.ClientHttp.Requests;
using FL.Shared.DataContracts;
using Microsoft.Extensions.Logging;

namespace FL.Services.DatabaseAccessors.ApiKeys
{
    public class AuthenticationDatabaseAccessor : IAuthenticationDatabaseAccessor
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly IRepository<ApiKeyEntity> _authenticationRepository;

        #endregion

        #region Ctor

        public AuthenticationDatabaseAccessor(ILogger<AuthenticationDatabaseAccessor> logger, IRepository<ApiKeyEntity> repository)
        {
            _logger = logger;
            _authenticationRepository = repository;
        }

        #endregion

        #region Methods

        public Task<AuthenticationDC> Authenticate(AuthenticateRequest request)
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation($"Starting of method {this.GetCallerMemberName()} in class {GetType().Name}.");
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                if (string.IsNullOrEmpty(request.Username))
                    throw new ArgumentNullException(nameof(request.Username));

                if (string.IsNullOrEmpty(request.ApiKey))
                    throw new ArgumentNullException(nameof(request.ApiKey));

                var authEntity = _authenticationRepository.FindBy(auth => auth.Username == request.Username && auth.ApiKey == request.ApiKey);

                if (authEntity != null)
                    return Task.FromResult(new AuthenticationDC { Id = authEntity.Id, Username = authEntity.Username, ApiKey = authEntity.ApiKey, ValidUntilDateUtc = authEntity.ValidUntilDateUtc });
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

        #endregion
    }
}
