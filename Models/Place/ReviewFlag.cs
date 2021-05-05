using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class ReviewFlag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Value { get; set; }

        [ForeignKey("Config")]
        public int IdReviewFlagConfig { get; set; }
        public virtual ConfigReviewFlag Config { get; set; }

        [ForeignKey("Review")]
        public int IdReview { get; set; }
        public virtual ReviewPlace Review { get; set; }
    }
}
