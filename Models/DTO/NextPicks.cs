﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TripickServer.Models
{
    public class NextPicks
    {
        public int Count { get; set; }
        public int ExistingPicksCount { get; set; }
        public List<Pick> Picks { get; set; }
    }
}
