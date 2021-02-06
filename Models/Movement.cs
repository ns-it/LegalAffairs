using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalAffairs.Models
{
    [Table("movements")]
    public partial class Movement
    {
        [Key]
        [Column("serial_number")]
        public byte SerialNumber { get; set; }
        [Key]
        [Column("case_year")]
        public short CaseYear { get; set; }
        [Key]
        [Column("annual_serial_number")]
        public int AnnualSerialNumber { get; set; }
        [Column("movement_date", TypeName = "date")]
        public DateTime? MovementDate { get; set; }
        [Column("title")]
        [StringLength(50)]
        public string Title { get; set; }
        [Column("attachement")]
        public string Attachement { get; set; }
        [Column("in_year")]
        public short? InYear { get; set; }
        [Column("in_serial_number")]
        public short? InSerialNumber { get; set; }
        [Column("out_year")]
        public short? OutYear { get; set; }
        [Column("out_serial_number")]
        public short? OutSerialNumber { get; set; }
        [Column("latest_update_timestamp", TypeName = "datetime")]
        public DateTime? LatestUpdateTimestamp { get; set; }
        [Column("latest_update_user_id")]
        public int? LatestUpdateUserId { get; set; }

        [ForeignKey("CaseYear,AnnualSerialNumber")]
        [InverseProperty("Movements")]
        public virtual Case Cases { get; set; }
        [ForeignKey("InYear,InSerialNumber")]
        [InverseProperty(nameof(InBook.Movements))]
        public virtual InBook In { get; set; }
        [ForeignKey(nameof(LatestUpdateUserId))]
        [InverseProperty(nameof(User.Movements))]
        public virtual User LatestUpdateUser { get; set; }
        [ForeignKey("OutYear,OutSerialNumber")]
        [InverseProperty(nameof(OutBook.Movements))]
        public virtual OutBook Out { get; set; }
    }
}
