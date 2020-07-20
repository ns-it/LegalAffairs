using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalAffairs.Models
{
    [Table("rules")]
    public partial class Rule
    {
        [Key]
        [Column("rule_year")]
        public short RuleYear { get; set; }
        [Key]
        [Column("annual_serial_number")]
        public int AnnualSerialNumber { get; set; }
        [Column("issuer")]
        [StringLength(50)]
        public string Issuer { get; set; }
        [Column("topic")]
        [StringLength(50)]
        public string Topic { get; set; }
        [Column("attachement")]
        public string Attachement { get; set; }
        [Column("latest_update_user_id")]
        public int? LatestUpdateUserId { get; set; }
        [Column("latest_update_timestamp")]
        public byte[] LatestUpdateTimestamp { get; set; }

        [ForeignKey(nameof(LatestUpdateUserId))]
        [InverseProperty(nameof(User.Rules))]
        public virtual User LatestUpdateUser { get; set; }
    }
}
