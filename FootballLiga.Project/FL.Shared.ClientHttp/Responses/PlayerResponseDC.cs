using System;
using System.Collections.Generic;
using System.Text;

namespace FL.Shared.ClientHttp.Responses
{
    public class PlayerResponseDC
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int NationalityId { get; set; }
        public int TeamId { get; set; }
    }
}
