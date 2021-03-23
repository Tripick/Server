using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Place
{
    public class RequestSearchAutocomplete
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
