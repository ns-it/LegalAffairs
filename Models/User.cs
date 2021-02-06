using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalAffairs.Models
{
    [Table("users")]
    public partial class User
    {
        public User()
        {
            CaseOwners = new HashSet<CaseOwner>();
            Cases = new HashSet<Case>();
            InBooks = new HashSet<InBook>();
            Movements = new HashSet<Movement>();
            OutBooks = new HashSet<OutBook>();
            Readings = new HashSet<Reading>();
            Rules = new HashSet<Rule>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        [StringLength(30)]
        public string Username { get; set; }
        [Column("password")]
        [StringLength(50)]
        public string Password { get; set; }
        [Column("first_name")]
        [StringLength(30)]
        public string FirstName { get; set; }
        [Column("last_name")]
        [StringLength(30)]
        public string LastName { get; set; }
        [Column("full_name")]
        [StringLength(61)]
        public string FullName { get; set; }

        [InverseProperty("LatestUpdateUser")]
        public virtual ICollection<CaseOwner> CaseOwners { get; set; }
        [InverseProperty("LatestUpdateUser")]
        public virtual ICollection<Case> Cases { get; set; }
        [InverseProperty("LatestUpdateUser")]
        public virtual ICollection<InBook> InBooks { get; set; }
        [InverseProperty("LatestUpdateUser")]
        public virtual ICollection<Movement> Movements { get; set; }
        [InverseProperty("LatestUpdateUser")]
        public virtual ICollection<OutBook> OutBooks { get; set; }
        [InverseProperty("LatestUpdateUser")]
        public virtual ICollection<Reading> Readings { get; set; }
        [InverseProperty("LatestUpdateUser")]
        public virtual ICollection<Rule> Rules { get; set; }
    }
}
