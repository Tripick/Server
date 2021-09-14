using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Trip
{
    public class RequestSearchMap
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public Region Region { get; set; }
    }
}
