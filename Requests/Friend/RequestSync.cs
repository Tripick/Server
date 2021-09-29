using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripickServer.Requests.Friend
{
    public class RequestSync
    {
        [Required]
        public List<Models.Friend> Friends { get; set; }
    }
}
