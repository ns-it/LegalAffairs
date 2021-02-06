using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalAffairs.Models
{
    [Table("out_books")]
    public partial class OutBook
    {
        public OutBook()
        {
            Movements = new HashSet<Movement>();
            Readings = new HashSet<Reading>();
        }

        [Key]
        [Column("out_year")]
        public short OutYear { get; set; }
        [Key]
        [Column("out_serial_number")]
        public short OutSerialNumber { get; set; }
        [Column("sending_date", TypeName = "date")]
        public DateTime? SendingDate { get; set; }
        [Column("summary")]
        public string Summary { get; set; }
        [Column("destination")]
        [StringLength(50)]
        public string Destination { get; set; }
        [Column("latest_update_timestamp", TypeName = "datetime")]
        public DateTime? LatestUpdateTimestamp { get; set; }
        [Column("latest_update_user_id")]
        public int? LatestUpdateUserId { get; set; }

        [ForeignKey(nameof(LatestUpdateUserId))]
        [InverseProperty(nameof(User.OutBooks))]
        public virtual User LatestUpdateUser { get; set; }
        [InverseProperty("Out")]
        public virtual ICollection<Movement> Movements { get; set; }
        [InverseProperty("Out")]
        public virtual ICollection<Reading> Readings { get; set; }
    }
}
