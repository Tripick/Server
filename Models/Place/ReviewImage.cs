using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class ReviewImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Image { get; set; }

        [ForeignKey("Review")]
        public int IdReview { get; set; }
        public virtual PlaceReview Review { get; set; }
    }
}
