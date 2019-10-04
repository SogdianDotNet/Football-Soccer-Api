using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FL.Data.Domain.Entities
{
    [Table("Leagues")]
    public class LeagueEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Season")]
        public int SeasonId { get; set; }

        public virtual ICollection<TeamEntity> Teams { get; set; }

        public virtual ICollection<MatchEntity> Matches { get; set; }

        public virtual SeasonEntity Season { get; set; }
    }
}
