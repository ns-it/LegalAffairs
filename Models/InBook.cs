using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalAffairs.Models
{
    [Table("in_books")]
    public partial class InBook
    {
        public InBook()
        {
            Movements = new HashSet<Movement>();
        }

        [Key]
        [Column("in_year")]
        public short InYear { get; set; }
        [Key]
        [Column("in_serial_number")]
        public short InSerialNumber { get; set; }
        [Column("storing_location")]
        [StringLength(50)]
        public string StoringLocation { get; set; }
        [Column("summary")]
        public string Summary { get; set; }
        [Column("sender")]
        [StringLength(50)]
        public string Sender { get; set; }
        [Column("arrival_date", TypeName = "date")]
        public DateTime? ArrivalDate { get; set; }
        [Column("latest_update_user_id")]
        public int? LatestUpdateUserId { get; set; }
        [Column("latest_update_timestamp")]
        public byte[] LatestUpdateTimestamp { get; set; }

        [ForeignKey(nameof(LatestUpdateUserId))]
        [InverseProperty(nameof(User.InBooks))]
        public virtual User LatestUpdateUser { get; set; }
        [InverseProperty("In")]
        public virtual ICollection<Movement> Movements { get; set; }
    }
}
