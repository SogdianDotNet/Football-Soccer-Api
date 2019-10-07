using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FL.Common.Base.Data;
using FL.Common.Base.Extensions;
using FL.Data.Domain.Entities;
using FL.Shared.DataContracts;
using Microsoft.Extensions.Logging;

namespace FL.Services.DatabaseAccessors.Countries
{
    public class CountryService : ICountryService
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly IRepository<CountryEntity> _countriesRepository;

        #endregion

        #region Ctor

        public CountryService(ILogger<CountryService> logger, IRepository<CountryEntity> repository)
        {
            _logger = logger;
            _countriesRepository = repository;
        }

        #endregion

        #region Methods

        public Task<List<CountryDC>> GetCountries()
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation($"Starting of method {this.GetCallerMemberName()} in class {GetType().Name}.");
            try
            {
                IEnumerable<CountryEntity> countries = _countriesRepository.GetList();

                return Task.FromResult(MapEntitiesToDataContracts(countries.ToArray()).ToList());
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

        #region Private methods

        private IEnumerable<CountryDC> MapEntitiesToDataContracts(CountryEntity[] countries)
        {
            foreach (var country in countries)
            {
                yield return new CountryDC
                {
                    Id = country.Id,
                    Name = country.Name
                };
            }
        }

        #endregion

        #endregion
    }
}
