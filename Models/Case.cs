using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalAffairs.Models
{
    [Table("cases")]
    public partial class Case
    {
        public Case()
        {
            Movements = new HashSet<Movement>();
        }

        [Key]
        [Column("case_year")]
        public short CaseYear { get; set; }
        [Key]
        [Column("annual_serial_number")]
        public int AnnualSerialNumber { get; set; }
        [Column("decision_number_year")]
        public short? DecisionNumberYear { get; set; }
        [Column("decision_number")]
        public int? DecisionNumber { get; set; }
        [Column("type")]
        [StringLength(50)]
        public string Type { get; set; }
        [Column("title")]
        [StringLength(50)]
        public string Title { get; set; }
        [Column("is_finished")]
        public bool IsFinished { get; set; }
        [Column("case_owner_id")]
        public int CaseOwnerId { get; set; }
        [Column("latest_update_timestamp")]
        public byte[] LatestUpdateTimestamp { get; set; }
        [Column("latest_update_user_id")]
        public int? LatestUpdateUserId { get; set; }

        [ForeignKey(nameof(CaseOwnerId))]
        [InverseProperty(nameof(Models.CaseOwner.Cases))]
        public virtual CaseOwner CaseOwner { get; set; }
        [ForeignKey(nameof(LatestUpdateUserId))]
        [InverseProperty(nameof(User.Cases))]
        public virtual User LatestUpdateUser { get; set; }
        [InverseProperty("Cases")]
        public virtual ICollection<Movement> Movements { get; set; }
    }
}
