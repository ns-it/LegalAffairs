using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalAffairs.Models
{
    [Table("topics")]
    public partial class Topic
    {
        public Topic()
        {
            Children = new HashSet<Topic>();
            Rules = new HashSet<Rule>();
        }

        [Key]
        [Column("topic_id")]
        public short TopicId { get; set; }
        [Required]
        [Column("topic_name")]
        [StringLength(50)]
        public string TopicName { get; set; }
        [Column("parent_topic_id")]
        public short? ParentTopicId { get; set; }
        [Column("has_children")]
        public bool HasChildren { get; set; }


        [ForeignKey(nameof(ParentTopicId))]
        [InverseProperty(nameof(Topic.Children))]
        public virtual Topic ParentTopic { get; set; }
        [InverseProperty(nameof(Topic.ParentTopic))]
        public virtual ICollection<Topic> Children { get; set; }
        [InverseProperty("Topic")]
        public virtual ICollection<Rule> Rules { get; set; }

    }
}
