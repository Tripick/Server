using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class ReviewFlag : ModelBase<ReviewFlag>
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

        public ReviewFlag ToDTO()
        {
            return new ReviewFlag()
            {
                Id = this.Id,
                Value = this.Value,
                IdConfig = this.IdConfig,
                Config = this.Config,
                IdReview = this.IdReview
            };
        }
    }
}
