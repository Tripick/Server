using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class Guide
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
    }
}
