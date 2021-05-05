using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

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
        public List<ReviewFlag> Flags { get; set; }
        public List<string> Pictures { get; set; }
    }
}
