using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class Filter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

        [ForeignKey("User")]
        public int IdUser { get; set; }
        public virtual AppUser User { get; set; }

        [ForeignKey("Trip")]
        public int IdTrip { get; set; }
        public virtual Trip Trip { get; set; }

        public Filter ToDTO()
        {
            return new Filter()
            {
                Id = this.Id,
                IdUser = this.IdUser,
                IdTrip = this.IdTrip,
                Name = this.Name,
                Min = this.Min,
                Max = this.Max,
            };
        }
    }
}
