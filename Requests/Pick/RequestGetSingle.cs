using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Pick
{
    public class RequestGetSingle
    {
        [Required]
        public int Id { get; set; }
    }
}
