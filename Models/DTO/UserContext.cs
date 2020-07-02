using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TripickServer.Models
{
    public class UserContext
    {
        public AppUser User { get; set; }
        public AuthenticationKeys AuthenticationKeys { get; set; }
        public List<Trip> Trips { get; set; }
        public List<Guide> RecommendedGuides { get; set; }

        public UserContext(AppUser user, string accessToken)
        {
            this.User = user;
            this.AuthenticationKeys = new AuthenticationKeys() { Id = user.Id, AccessToken = accessToken };
            this.Trips = new List<Trip>();
            this.RecommendedGuides = new List<Guide>();
        }
    }
}
