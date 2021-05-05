using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TripickServer.Models
{
    public class AppConfiguration
    {
        public List<ConfigReviewFlag> Flags { get; set; }
    }
}
