using System;
using System.Collections.Generic;
using System.Text;

namespace FL.Shared.DataContracts
{
    public class CountryDC
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of a country
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the shortname of a country
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the leagues of a country
        /// </summary>
        public List<LeagueDC> Leagues { get; set; }
    }
}
