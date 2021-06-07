using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TripickServer.Models.Common;

namespace TripickServer.Models
{

    public class Follower : ModelBase<Follower>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }

        public Follower() { }

        public Follower(AppUser user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Photo = string.IsNullOrWhiteSpace(user.Photo?.Image) ? "_" : user.Photo.Image;
        }

        public Follower ToDTO()
        {
            return new Follower()
            {
                Id = this.Id,
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Photo = this.Photo
            };
        }
    }
}
