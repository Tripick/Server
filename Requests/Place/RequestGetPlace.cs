using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Place
{
    public class RequestGetPlace
    {
        [Required]
        public int Id { get; set; }
    }
}
