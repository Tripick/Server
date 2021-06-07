using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class MapPoint : ModelBase<MapPoint>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Index { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [ForeignKey("Trip")]
        public int IdTrip { get; set; }
        public virtual Trip Trip { get; set; }

        public MapPoint ToDTO()
        {
            return new MapPoint()
            {
                Id = this.Id,
                IdTrip = this.IdTrip,
                Index = this.Index,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
            };
        }
    }
}
