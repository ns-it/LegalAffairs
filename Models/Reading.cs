using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalAffairs.Models
{
    [Table("readings")]
    public partial class Reading
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("topic")]
        [StringLength(50)]
        public string Topic { get; set; }
        [Column("category")]
        [StringLength(50)]
        public string Category { get; set; }
        [Column("attachements")]
        public string Attachements { get; set; }
        [Column("out_year")]
        public short? OutYear { get; set; }
        [Column("out_serial_number")]
        public short? OutSerialNumber { get; set; }
        [Column("latest_update_timestamp", TypeName = "datetime")]
        public DateTime? LatestUpdateTimestamp { get; set; }
        [Column("latest_update_user_id")]
        public int? LatestUpdateUserId { get; set; }

        [ForeignKey(nameof(LatestUpdateUserId))]
        [InverseProperty(nameof(User.Readings))]
        public virtual User LatestUpdateUser { get; set; }
        [ForeignKey("OutYear,OutSerialNumber")]
        [InverseProperty(nameof(OutBook.Readings))]
        public virtual OutBook Out { get; set; }
    }
}
