using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Place
{
    public class RequestSave
    {
        [Required]
        public TripickServer.Models.Place Place { get; set; }
    }
}
