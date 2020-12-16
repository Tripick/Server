using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Traveler
{
    public class RequestAdd
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public int IdFriend { get; set; }
    }
}
