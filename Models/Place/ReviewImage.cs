using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class ReviewImage : ModelBase<ReviewImage>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Image { get; set; }

        [ForeignKey("Review")]
        public int IdReview { get; set; }
        public virtual PlaceReview Review { get; set; }

        public ReviewImage ToDTO()
        {
            return new ReviewImage()
            {
                Id = this.Id,
                Image = this.Image,
                IdReview = this.IdReview
            };
        }
    }
}
