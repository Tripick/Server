using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Trip
{
    public class RequestGetItinerary
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public bool ForceRegeneration { get; set; }
    }
}
