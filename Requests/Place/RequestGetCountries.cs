using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Place
{
    public class RequestGetCountries
    {
        [Required]
        public int Quantity { get; set; }
    }
}
