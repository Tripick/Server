using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class Region : ModelBase<Region>
    {
        public double Latitude { get; set; }
        public double LatitudeDelta { get; set; }
        public double Longitude { get; set; }
        public double LongitudeDelta { get; set; }

        public Region ToDTO() { return this; }
    }
}
