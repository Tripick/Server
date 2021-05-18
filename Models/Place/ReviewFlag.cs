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
        public int IdConfig { get; set; }
        public virtual ConfigFlag Config { get; set; }

        [ForeignKey("Review")]
        public int IdReview { get; set; }
        public virtual PlaceReview Review { get; set; }
    }
}
