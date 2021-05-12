using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace TripickServer.Models
{
    public class TripickContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public IConfiguration configuration { get; }

        public TripickContext(DbContextOptions<TripickContext> options) : base(options) { }

        /////// When doing a migration, uncomment this :
        //public TripickContext(IConfiguration configuration, DbContextOptions<TripickContext> options) : base(options)
        //{
        //    this.configuration = configuration;
        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), b => b.ProvideClientCertificatesCallback(clientCerts =>
        //    {
        //        var databaseCertificate = configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + "/Resources/databaseCert.pfx";
        //        var cert = new X509Certificate2(databaseCertificate, configuration.GetValue<string>("Settings:databaseCertPassword"));
        //        clientCerts.Add(cert);
        //    }));
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friendship>().HasKey(x => new { x.IdOwner, x.IdFriend });

            modelBuilder.Entity<Trip>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.Trips);

            modelBuilder.Entity<Trip>()
                .HasMany(x => x.Members)
                .WithMany(x => x.GuestTrips);

            modelBuilder.Entity<Trip>()
                .HasMany(x => x.Subscribers)
                .WithMany(x => x.WatchedTrips);

            base.OnModelCreating(modelBuilder);
        }

        // Commons
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<ConfigReviewFlag> ConfigReviewFlags { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }

        // Countries
        public DbSet<Country> Countries { get; set; }

        // Places
        public DbSet<Place> Places { get; set; }
        public DbSet<ImagePlace> ImagePlaces { get; set; }
        public DbSet<ReviewPlace> ReviewPlace { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }
        public DbSet<ReviewFlag> ReviewFlags { get; set; }
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
        public DbSet<Location> Locations { get; set; }
        public DbSet<MapPoint> MapPoint { get; set; }
        public DbSet<MapTile> MapTiles { get; set; }

        // Filters
        public DbSet<Filter> Filters { get; set; }

        // Guides
        public DbSet<Guide> Guides { get; set; }
        public DbSet<ImageGuide> ImageGuides { get; set; }
        public DbSet<ReviewGuide> ReviewGuides { get; set; }
        public DbSet<VoteReviewGuide> VoteReviewGuides { get; set; }
    }
}
