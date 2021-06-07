using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class NextPicks : ModelBase<NextPicks>
    {
        public int Count { get; set; }
        public int ExistingPicksCount { get; set; }
        public List<Pick> Picks { get; set; }

        public NextPicks ToDTO()
        {
            return new NextPicks()
            {
                Count = this.Count,
                ExistingPicksCount = this.ExistingPicksCount,
                Picks = this.Picks?.ToDTO(),
            };
        }
    }
}
