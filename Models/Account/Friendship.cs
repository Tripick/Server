using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class Friendship
    {
        [ForeignKey("Owner")]
        public int IdOwner { get; set; }
        public AppUser Owner { get; set; }

        public int IdFriend { get; set; }
    }
}
