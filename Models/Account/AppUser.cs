using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class AppUser : IdentityUser<int>, ModelBase<AppUser>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ImageAppUser Photo { get; set; }
        public virtual List<Trip> Trips { get; set; }
        public virtual List<Trip> GuestTrips { get; set; }
        public virtual List<Trip> WatchedTrips { get; set; }
        public virtual List<Friendship> Friendships { get; set; }

        public AppUser ToDTO()
        {
            return new AppUser()
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Photo = this.Photo?.ToDTO(),
                Trips = this.Trips?.ToDTO(),
                GuestTrips = this.GuestTrips?.ToDTO(),
                WatchedTrips = this.WatchedTrips?.ToDTO(),
                Friendships = this.Friendships?.ToDTO(),
            };
        }
    }
}
