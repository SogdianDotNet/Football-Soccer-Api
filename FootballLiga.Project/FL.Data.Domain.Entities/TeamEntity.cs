using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FL.Data.Domain.Entities
{
    [Table("Teams")]
    public partial class TeamEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("league")]
        public int LeagueId { get; set; }

        public virtual LeagueEntity League { get; set; }

        public virtual ICollection<MatchEntity> HomeMatches { get; set; }
        public virtual ICollection<MatchEntity> AwayMatches { get; set; }
        public virtual ICollection<PlayerEntity> Players { get; set; }
    }
}
