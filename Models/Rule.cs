using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalAffairs.Models
{
    [Table("rules")]
    public partial class Rule 
    {

        public Rule()
        {
            RuleAttachements = new HashSet<RuleAttachement>();
        }
        [Key]
        [Column("rule_id")]
        public int RuleId { get; set; }

        [Column("rule_year")]
        public short RuleYear { get; set; }
        [Column("annual_serial_number")]
        public int AnnualSerialNumber { get; set; }
        [Column("issuer_id")]
        public short IssuerId { get; set; }
        [Column("topic_id")]
        public short TopicId { get; set; }
        [Column("attachement")]
        public string Attachement { get; set; }
        [Column("latest_update_user_id")]
        public int? LatestUpdateUserId { get; set; }
        [Column("latest_update_timestamp", TypeName = "datetime")]
        public DateTime? LatestUpdateTimestamp { get; set; }

        [ForeignKey(nameof(IssuerId))]
        [InverseProperty("Rules")]
        public virtual Issuer Issuer { get; set; }
        [ForeignKey(nameof(TopicId))]
        [InverseProperty("Rules")]
        public virtual Topic Topic { get; set; }
        [ForeignKey(nameof(LatestUpdateUserId))]
        [InverseProperty(nameof(User.Rules))]
        public virtual User LatestUpdateUser { get; set; }

        [InverseProperty("Rule")]
        public virtual ICollection<RuleAttachement> RuleAttachements { get; set; }

        //public string Error => throw new NotImplementedException();

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        string errorMessage = null;
        //        switch (columnName)
        //        {
        //            case "AnnualSerialNumber":
                       
        //                    errorMessage = "Must be a number";
                       
        //                break;
        //            //case "LastName":
        //            //    if (String.IsNullOrWhiteSpace(LastName))
        //            //    {
        //            //        errorMessage = "Last Name is required.";
        //            //    }
        //            //    break;
        //        }
        //        return errorMessage;
        //    }
        //}
    }
}
