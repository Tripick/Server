using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Pick
{
    public class RequestGenerate
    {
        [Required]
        public int IdTrip { get; set; }
    }
}
