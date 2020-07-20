using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalAffairs.Models
{
    [Table("case_owners")]
    public partial class CaseOwner
    {
        public CaseOwner()
        {
            Cases = new HashSet<Case>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("first_name")]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Column("last_name")]
        [StringLength(20)]
        public string LastName { get; set; }
        [Column("middle_name")]
        [StringLength(20)]
        public string MiddleName { get; set; }
        [Column("full_name")]
        [StringLength(60)]
        public string FullName { get; set; }
        [Column("class")]
        public byte? Class { get; set; }
        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; }
        [Column("latest_update_timestamp")]
        public byte[] LatestUpdateTimestamp { get; set; }
        [Column("latest_update_user_id")]
        public int? LatestUpdateUserId { get; set; }

        [ForeignKey(nameof(LatestUpdateUserId))]
        [InverseProperty(nameof(User.CaseOwners))]
        public virtual User LatestUpdateUser { get; set; }
        [InverseProperty("CaseOwner")]
        public virtual ICollection<Case> Cases { get; set; }
    }
}
