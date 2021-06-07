using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class Itinerary : ModelBase<Itinerary>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsCustomized { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("Trip")]
        public int IdTrip { get; set; }
        public virtual Trip Trip { get; set; }

        public virtual List<Day> Days { get; set; }

        public Itinerary ToDTO()
        {
            return new Itinerary()
            {
                Id = this.Id,
                IdTrip = this.IdTrip,
            };
        }
    }
}
