using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TripickServer.Requests
{
    public class RequestReset
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
