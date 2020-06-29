using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TripickServer.Models
{
    public class TripickContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public TripickContext(DbContextOptions<TripickContext> options) : base(options) { }

        // Commons
        public DbSet<Hashtag> Hashtags { get; set; }

        // Places
        public DbSet<Place> Places { get; set; }
        public DbSet<ImagePlace> ImagePlaces { get; set; }
        public DbSet<ReviewPlace> ReviewPlace { get; set; }
        public DbSet<VoteReviewPlace> VoteReviewPlace { get; set; }

        // Trips
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Pick> Picks { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<TypeStep> TypeSteps { get; set; }
        public DbSet<ToBring> ToBrings { get; set; }
        public DbSet<TypeToBring> TypeToBrings { get; set; }
        public DbSet<TypeGroup> TypeGroups { get; set; }

        // Guides
        public DbSet<Guide> Guides { get; set; }
        public DbSet<ImageGuide> ImageGuides { get; set; }
        public DbSet<ReviewGuide> ReviewGuides { get; set; }
        public DbSet<VoteReviewGuide> VoteReviewGuides { get; set; }
    }
}
