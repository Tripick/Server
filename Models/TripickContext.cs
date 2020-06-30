using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace TripickServer.Models
{
    public class TripickContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public IConfiguration configuration { get; }

        public TripickContext(IConfiguration configuration, DbContextOptions<TripickContext> options) : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString, b => b.ProvideClientCertificatesCallback(clientCerts =>
            {
                var databaseCertificate = @"~/Resources/databaseCert.pfx";
                var cert = new X509Certificate2(databaseCertificate);
                clientCerts.Add(cert);
            }));
        }

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
