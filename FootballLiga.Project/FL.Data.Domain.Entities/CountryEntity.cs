using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FL.Data.Domain.Entities
{
    [Table("Countries")]
    public class CountryEntity
    {
        /// <summary>
        /// Gets or sets the Id of a country
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public virtual ICollection<LeagueEntity> Leagues { get; set; }
    }
}
