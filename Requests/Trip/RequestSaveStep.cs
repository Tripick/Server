using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Trip
{
    public class RequestSaveStep
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public int IdDay { get; set; }
        [Required]
        public ItineraryDayStep Step { get; set; }
    }
}
