using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class TripFriendship
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Trip")]
        public int IdTrip { get; set; }
        public virtual Trip Trip { get; set; }

        [Key]
        [Column(Order = 1)]
        public int IdOwner { get; set; }
        [Key]
        [Column(Order = 2)]
        public int IdFriend { get; set; }

        [ForeignKey("IdOwner, IdFriend")]
        public virtual Friendship Friendship { get; set; }
    }
}
