using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Traveler
{
    public class RequestSave
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public List<int> IdsFriends { get; set; }
    }
}
