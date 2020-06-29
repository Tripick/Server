using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TripickServer.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ImageAppUser Photo { get; set; }

        public List<AppUser> Friends { get; set; }
    }
}
