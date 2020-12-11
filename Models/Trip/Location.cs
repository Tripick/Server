using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class Location
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
    }
}
