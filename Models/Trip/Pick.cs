using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class Pick : ModelBase<Pick>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Index { get; set; }
        public int Rating { get; set; }

        [ForeignKey("Place")]
        public int IdPlace { get; set; }
        public virtual Place Place { get; set; }

        [ForeignKey("User")]
        public int IdUser { get; set; }
        public virtual AppUser User { get; set; }

        [ForeignKey("Trip")]
        public int IdTrip { get; set; }
        public virtual Trip Trip { get; set; }

        public Pick ToDTO()
        {
            return new Pick()
            {
                Id = this.Id,
                Index = this.Index,
                Rating = this.Rating,
                IdTrip = this.IdTrip,
                IdPlace = this.IdPlace,
                IdUser = this.IdUser,
                Place = this.Place.ToDTO(),
                Trip = this.Trip.ToDTO(),
            };
        }
    }
}
