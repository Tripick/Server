using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Trip
{
    public class RequestMoveStep
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public int IdOldDay { get; set; }
        [Required]
        public int IdNewDay { get; set; }
        [Required]
        public int IdStep { get; set; }
    }
}
