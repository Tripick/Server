using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class PlaceReview : ModelBase<PlaceReview>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Rating { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual List<ReviewFlag> Flags { get; set; }
        public virtual List<ReviewImage> Pictures { get; set; }
        public virtual List<VoteReviewPlace> Votes { get; set; }

        [ForeignKey("Place")]
        public int IdPlace { get; set; }
        public virtual Place Place { get; set; }

        [ForeignKey("Author")]
        public int IdAuthor { get; set; }
        public virtual AppUser Author { get; set; }

        public PlaceReview ToDTO()
        {
            return new PlaceReview()
            {
                Id = this.Id,
                IdPlace = this.IdPlace,
                IdAuthor = this.IdAuthor,
                Author = this.Author == null ? null : new AppUser()
                {
                    Id = this.Author.Id,
                    FirstName = this.Author.FirstName,
                    LastName = this.Author.LastName,
                    UserName = this.Author.UserName,
                    Photo = this.Author.Photo.ToDTO(),
                },
                Rating = this.Rating,
                Title = this.Title,
                Message = this.Message,
                Flags = this.Flags.ToDTO(),
                Pictures = this.Pictures.ToDTO(),
                Votes = this.Votes.ToDTO(),
            };
        }
    }
}
