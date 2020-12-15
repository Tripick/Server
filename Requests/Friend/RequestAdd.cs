using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Friend
{
    public class RequestAdd
    {
        [Required]
        public int Id { get; set; }
    }
}
