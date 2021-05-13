using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Country
{
    public class RequestGetAll
    {
        [Required]
        public int Quantity { get; set; }
    }
}
