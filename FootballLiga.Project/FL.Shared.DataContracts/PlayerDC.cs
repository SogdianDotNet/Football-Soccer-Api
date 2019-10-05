using System;
using System.Collections.Generic;
using System.Text;

namespace FL.Shared.DataContracts
{
    public class PlayerDC
    {
        public long Id { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int NationalityId { get; set; }
        public int TeamId { get; set; }

        public TeamDC Team { get; set; }

        public CountryDC Nationality { get; set; }
    }
}
