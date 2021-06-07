using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class Guide : ModelBase<Guide>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TypeGuide Type { get; set; }
        public List<TypeGroup> RecommendedGroups { get; set; }
        public virtual List<Hashtag> Hashtags { get; set; }
        public virtual List<ImageGuide> Images { get; set; }
        public virtual List<ReviewGuide> Reviews { get; set; }
        public double Price { get; set; }

        [ForeignKey("Author")]
        public int IdAuthor { get; set; }
        public virtual AppUser Author { get; set; }

        [ForeignKey("Trip")]
        public int IdTrip { get; set; }
        public virtual Trip Trip { get; set; }

        public Guide ToDTO()
        {
            return new Guide()
            {
                Id = this.Id,
                IdAuthor = this.IdAuthor,
                IdTrip = this.IdTrip,
                Name = this.Name,
                Description = this.Description,
                Type = this.Type.ToDTO(),
                RecommendedGroups = this.RecommendedGroups.ToDTO(),
                Hashtags = this.Hashtags.ToDTO(),
                Images = this.Images.ToDTO(),
                Reviews = this.Reviews.ToDTO(),
                Price = this.Price,
            };
        }
    }
}
