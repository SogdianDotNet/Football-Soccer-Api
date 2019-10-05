using System;
using System.Collections.Generic;
using System.Text;

namespace FL.Shared.DataContracts
{
    public class AuthenticationDC
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string ApiKey { get; set; }
        public DateTime ValidUntilDateUtc { get; set; }

        public bool IsExpired
        {
            get
            {
                return ValidUntilDateUtc < DateTime.UtcNow;
            }
        }
    }
}
