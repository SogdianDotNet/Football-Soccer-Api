using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FL.Data.Domain.Entities.Authentication
{
    [Table("ApiKeys")]
    public class ApiKeyEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string ApiKey { get; set; }
        [Required]
        public DateTime ValidUntilDateUtc { get; set; }

    }
}
