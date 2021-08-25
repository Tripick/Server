using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Trip
{
    public class RequestSaveDays
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public List<ItineraryDay> Days { get; set; }
    }
}
