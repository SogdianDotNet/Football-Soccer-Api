using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FL.Data.Domain.Entities
{
    [Table("Matches")]
    public partial class MatchEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public int GoalsHome { get; set; }

        public int GoalsAway { get; set; }

        [ForeignKey("HomeTeam")]
        public int HomeTeamId { get; set; }

        [ForeignKey("AwayTeamId")]
        public int AwayTeamId { get; set; }

        [Required]
        public TeamEntity HomeTeam { get; set; }

        [Required]
        public TeamEntity AwayTeam { get; set; }
    }
}
