using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TripickServer.Models
{
    public class AppConfiguration
    {
        public List<ConfigFlag> Flags { get; set; }
    }
}
