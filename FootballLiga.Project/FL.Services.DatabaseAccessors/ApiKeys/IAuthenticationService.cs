using FL.Shared.ClientHttp.Requests;
using FL.Shared.DataContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FL.Services.DatabaseAccessors.ApiKeys
{
    public interface IAuthenticationService
    {
        Task<AuthenticationDC> Authenticate(AuthenticateRequest request);
    }
}
