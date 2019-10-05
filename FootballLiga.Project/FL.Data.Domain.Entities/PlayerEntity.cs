using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FL.Data.Domain.Entities
{
    [Table("Players")]
    public partial class PlayerEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }

        [ForeignKey("Nationality")]
        public int NationalityId { get; set; }

        [ForeignKey("Team")]
        public int TeamId { get; set; }

        public virtual TeamEntity Team { get; set; }

        public virtual CountryEntity Nationality { get; set; }
    }
}
