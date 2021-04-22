using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Place
{
    public class RequestReview
    {
        [Required]
        public int IdPlace { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public string Message { get; set; }
        public string Title { get; set; }
    }
}
