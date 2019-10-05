using System;
using System.Collections.Generic;
using System.Text;

namespace FL.Shared.ClientHttp.Responses
{
    public class AuthenticateResponse
    {
        public string Username { get; set; }
        public string ApiKey { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Error { get; set; }
    }
}
