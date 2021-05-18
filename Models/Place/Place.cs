using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class Place
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PlaceId { get; set; }
        public string BusinessStatus { get; set; }
        public string Name { get; set; }
        public string NameTranslated { get; set; }

        /// Refined
        public string Description { get; set; }
        // Filters
        public double? Price { get; set; }
        public double? Length { get; set; }
        public int? Duration { get; set; }
        public int? Difficulty { get; set; }
        public int? Touristy { get; set; }
        /// Refined

        public string PriceLevel { get; set; }
        public string Address { get; set; }
        public int CountryId { get; set; } = -1;
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Rating { get; set; }
        public double NbRating { get; set; }
        public string Types { get; set; }
        public string Reference { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual List<ImagePlace> Images { get; set; }
        public virtual List<PlaceFlag> Flags { get; set; }
        public virtual List<PlaceReview> Reviews { get; set; }
    }
}
