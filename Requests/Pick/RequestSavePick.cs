using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Pick
{
    public class RequestSavePick
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        public int IdPlace { get; set; }
        [Required]
        public int Rating { get; set; }
    }
}
