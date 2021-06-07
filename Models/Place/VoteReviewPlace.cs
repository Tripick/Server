﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class VoteReviewPlace : ModelBase<VoteReviewPlace>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsUp { get; set; }

        [ForeignKey("ReviewPlace")]
        public int IdReviewPlace { get; set; }
        public virtual PlaceReview ReviewPlace { get; set; }

        [ForeignKey("Author")]
        public int IdAuthor { get; set; }
        public virtual AppUser Author { get; set; }

        public VoteReviewPlace ToDTO()
        {
            return new VoteReviewPlace()
            {
                Id = this.Id,
                IsUp = this.IsUp,
                IdReviewPlace = this.IdReviewPlace,
                IdAuthor = this.IdAuthor,
            };
        }
    }
}
