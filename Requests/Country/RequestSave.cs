using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Country
{
    public class RequestSave
    {
        [Required]
        public TripickServer.Models.Country Country { get; set; }
    }
}
