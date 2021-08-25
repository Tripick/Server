using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Trip
{
    public class RequestSaveDay
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public ItineraryDay Day { get; set; }
    }
}
