using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class Destination : ModelBase<Destination>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsMapFrame { get; set; }
        public string Name { get; set; }
        public List<BoundingBox> BoundingBoxes { get; set; }

        [ForeignKey("Trip")]
        public int IdTrip { get; set; }
        public virtual Trip Trip { get; set; }

        public Destination ToDTO()
        {
            return new Destination()
            {
                Id = this.Id,
                IsMapFrame = this.IsMapFrame,
                Name = this.Name,
                BoundingBoxes = this.BoundingBoxes.ToDTO(),
                IdTrip = this.IdTrip,
                Trip = this.Trip.ToDTO()
            };
        }
    }
}
