using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Friend
{
    public class RequestSearch
    {
        [Required]
        public string UserName { get; set; }
    }
}
