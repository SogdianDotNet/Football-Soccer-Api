using FL.Shared.DataContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FL.Services.DatabaseAccessors.Countries
{
    public interface ICountryService
    {
        Task<List<CountryDC>> GetCountries();
    }
}
