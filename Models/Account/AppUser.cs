using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class AppUser : IdentityUser<int>, ModelBase<AppUser>
    {
        public DateTime NewConnectionDate { get; set; } = DateTime.Now;
        public DateTime LastConnectionDate { get; set; } = DateTime.Now;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ImageAppUser Photo { get; set; }
        public virtual List<Trip> Trips { get; set; }
        public virtual List<Trip> GuestTrips { get; set; }
        public virtual List<Trip> WatchedTrips { get; set; }
        public virtual List<Friendship> Friendships { get; set; }
        public virtual List<Retribution> Retributions { get; set; }

        public AppUser ToDTO()
        {
            return new AppUser()
            {
                Id = this.Id,
                NewConnectionDate = this.NewConnectionDate,
                LastConnectionDate = this.LastConnectionDate,
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Photo = this.Photo?.ToDTO(),
                Trips = this.Trips?.ToDTO(),
                GuestTrips = this.GuestTrips?.ToDTO(),
                WatchedTrips = this.WatchedTrips?.ToDTO(),
                Friendships = this.Friendships?.ToDTO(),
                Retributions = this.Retributions?.ToDTO(),
            };
        }
    }
}
