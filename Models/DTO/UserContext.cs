using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TripickServer.Models
{
    public class UserContext
    {
        public AppUser User { get; set; }
        public AuthenticationKeys AuthenticationKeys { get; set; }
        public AppConfiguration Configuration { get; set; }
        public List<Friend> Friends { get; set; }
        public List<Trip> Trips { get; set; }
        public List<Guide> RecommendedGuides { get; set; }

        public UserContext(AppConfiguration configuration, List<Friend> friends, List<Trip> trips, List<Guide> guides)
        {
            this.Configuration = configuration;
            this.Friends = friends;
            this.Trips = trips;
            this.RecommendedGuides = guides;
        }

        public UserContext(AppUser user, string accessToken, AppConfiguration configuration)
        {
            this.User = user;
            this.AuthenticationKeys = new AuthenticationKeys() { Id = user.Id, AccessToken = accessToken };
            this.Configuration = configuration;
            this.Friends = new List<Friend>();
            this.Trips = new List<Trip>();
            this.RecommendedGuides = new List<Guide>();
        }
    }
}
