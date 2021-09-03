using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Trip
{
    public class RequestDeleteItinerary
    {
        [Required]
        public int IdTrip { get; set; }
    }
}
