using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Country
{
    public class RequestSaveAll
    {
        [Required]
        public List<TripickServer.Models.Country> Countries { get; set; }
    }
}
