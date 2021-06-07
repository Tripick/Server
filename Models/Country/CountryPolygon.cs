using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class CountryPolygon : ModelBase<CountryPolygon>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public List<CountryPoint> Points { get; set; }

        public CountryPolygon ToDTO()
        {
            return new CountryPolygon()
            {
                Id = this.Id,
                CountryId = this.CountryId,
                Points = this.Points?.ToDTO()
            };
        }
    }
}
