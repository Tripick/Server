using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class Friendship : ModelBase<Friendship>
    {
        [ForeignKey("Owner")]
        public int IdOwner { get; set; }
        public AppUser Owner { get; set; }

        public int IdFriend { get; set; }
        public int? NeedToConfirm { get; set; }

        public Friendship ToDTO()
        {
            return new Friendship()
            {
                IdOwner = this.IdOwner,
                IdFriend = this.IdFriend,
                NeedToConfirm = this.NeedToConfirm
            };
        }
    }
}
