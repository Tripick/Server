using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class UserContext : ModelBase<UserContext>
    {
        public AppUser User { get; set; }
        public AuthenticationKeys AuthenticationKeys { get; set; }
        public AppConfiguration Configuration { get; set; }
        public List<Friend> Friends { get; set; }
        public List<Trip> Trips { get; set; }
        public List<Guide> RecommendedGuides { get; set; }

        public UserContext() { }

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

        public UserContext ToDTO()
        {
            return new UserContext()
            {
                User = this.User?.ToDTO(),
                AuthenticationKeys = this.AuthenticationKeys?.ToDTO(),
                Configuration = this.Configuration?.ToDTO(),
                Friends = this.Friends?.ToDTO(),
                Trips = this.Trips?.ToDTO(),
                RecommendedGuides = this.RecommendedGuides?.ToDTO(),
            };
        }
    }
}
