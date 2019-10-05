using System;
using System.Collections.Generic;
using System.Text;

namespace FL.Shared.ClientHttp.Requests
{
    public class AuthenticateRequest
    {
        public AuthenticateRequest() { }

        public AuthenticateRequest(string username, string apiKey)
        {
            Username = username;
            ApiKey = apiKey;
        }

        public string Username { get; set; }
        public string ApiKey { get; set; }
    }
}
