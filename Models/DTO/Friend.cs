using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class Friend : ModelBase<Friend>
    {
        public int Id { get; set; }
        public int? NeedToConfirm { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public List<Trip> SharedTrips { get; set; }

        public override bool Equals(object f) { return f != null && f is Friend && this.GetHashCode() == ((Friend)f).GetHashCode(); }
        public override int GetHashCode()
        {
            return HashCode.Combine(
                this.Id,
                this.NeedToConfirm,
                this.UserName,
                this.FirstName,
                this.LastName,
                this.SharedTrips == null ? null : this.SharedTrips.OrderBy(x => x.Id).Select(t => t.GetHashCode()).ToList()
            );
        }

        public Friend() { }

        public Friend(AppUser user)
        {
            this.Id = user.Id;
            this.NeedToConfirm = null;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Photo = string.IsNullOrWhiteSpace(user.Photo?.Image) ? "_" : user.Photo.Image;
            this.SharedTrips = new List<Trip>();
        }

        public Friend(AppUser user, Friendship friendship)
        {
            this.Id = user.Id;
            this.NeedToConfirm = friendship.NeedToConfirm;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Photo = string.IsNullOrWhiteSpace(user.Photo?.Image) ? "_" : user.Photo.Image;
            this.SharedTrips = new List<Trip>();
        }

        public Friend ToDTO() { return this; }
    }
}
