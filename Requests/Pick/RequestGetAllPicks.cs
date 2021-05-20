using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Pick
{
    public class RequestGetAllPicks
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Skip { get; set; }
    }
}
