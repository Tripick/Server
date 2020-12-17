using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TripickServer.Models
{
    public class Follower
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }

        public Follower(AppUser user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Photo = string.IsNullOrWhiteSpace(user.Photo?.Image) ? "_" : user.Photo.Image;
        }
    }
}
