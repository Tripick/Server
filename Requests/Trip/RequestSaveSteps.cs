using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Trip
{
    public class RequestSaveSteps
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public int IdDay { get; set; }
        [Required]
        public List<ItineraryDayStep> Steps { get; set; }
    }
}
