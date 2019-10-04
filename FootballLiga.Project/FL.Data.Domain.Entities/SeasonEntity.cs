using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FL.Data.Domain.Entities
{
    [Table("Seasons")]
    public partial class SeasonEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public virtual ICollection<LeagueEntity> Leagues { get; set; }

        public string SeasonYear
        {
            get
            {
                return $"{FromDate.Year}/{ToDate.Year}";
            }
        }
    }
}
