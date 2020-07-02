using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TripickServer.Models
{
    public class AuthenticationKeys
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
    }
}
