using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class CountryArea : ModelBase<CountryArea>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public double MinLat { get; set; }
        public double MinLon { get; set; }
        public double MaxLat { get; set; }
        public double MaxLon { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public CountryArea ToDTO()
        {
            return new CountryArea()
            {
                Id = this.Id,
                MinLat = this.MinLat,
                MinLon = this.MinLon,
                MaxLat = this.MaxLat,
                MaxLon = this.MaxLon,
            };
        }
    }
}
