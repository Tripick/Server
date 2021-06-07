using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class Location : ModelBase<Location>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double LatitudeDelta { get; set; }
        public double LongitudeDelta { get; set; }

        [ForeignKey("Trip")]
        public int? IdTrip { get; set; }
        public virtual Trip Trip { get; set; }

        public Location ToDTO()
        {
            return new Location()
            {
                Id = this.Id,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                LatitudeDelta = this.LatitudeDelta,
                LongitudeDelta = this.LongitudeDelta,
                IdTrip = this.IdTrip
            };
        }
    }
}
