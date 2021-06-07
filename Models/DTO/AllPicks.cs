using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class AllPicks : ModelBase<AllPicks>
    {
        public int ExistingPicksCount { get; set; }
        public List<Pick> Picks { get; set; }

        public AllPicks ToDTO()
        {
            return new AllPicks()
            {
                ExistingPicksCount = this.ExistingPicksCount,
                Picks = this.Picks.ToDTO()
            };
        }
    }
}
