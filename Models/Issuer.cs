using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalAffairs.Models
{
    [Table("issuers")]
    public partial class Issuer
    {
        public Issuer()
        {
            Rules = new HashSet<Rule>();
        }

        [Key]
        [Column("issuer_id")]
        public short IssuerId { get; set; }
        [Required]
        [Column("issuer_name")]
        [StringLength(50)]
        public string IssuerName { get; set; }

        [InverseProperty("Issuer")]
        public virtual ICollection<Rule> Rules { get; set; }
    }
}
