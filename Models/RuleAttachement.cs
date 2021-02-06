using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalAffairs.Models
{
    [Table("rule_attachements")]
    public partial class RuleAttachement
    {
        [Key]
        [Column("rule_id")]
        public int RuleId { get; set; }
        [Key]
        [Column("attachment_number")]
        public short AttachmentNumber { get; set; }
        [Column("path")]
        public string Path { get; set; }

        [ForeignKey(nameof(RuleId))]
        [InverseProperty("RuleAttachements")]
        public virtual Rule Rule { get; set; }
    }
}
