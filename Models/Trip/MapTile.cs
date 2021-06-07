using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class MapTile : ModelBase<MapTile>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        [ForeignKey("Trip")]
        public int IdTrip { get; set; }
        public virtual Trip Trip { get; set; }

        public MapTile ToDTO()
        {
            return new MapTile()
            {
                Id = this.Id,
                IdTrip = this.IdTrip,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                Height = this.Height,
                Width = this.Width,
            };
        }
    }
}
