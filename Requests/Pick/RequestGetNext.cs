using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Pick
{
    public class RequestGetNext
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
