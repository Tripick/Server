using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class Trip : ModelBase<Trip>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool IsPublic { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string CoverImage { get; set; }

        public DateTime? StartDate { get; set; }
        public double? StartLatitude { get; set; }
        public double? StartLongitude { get; set; }
        public double? StartLatitudeDelta { get; set; }
        public double? StartLongitudeDelta { get; set; }

        public DateTime? EndDate { get; set; }
        public double? EndLatitude { get; set; }
        public double? EndLongitude { get; set; }
        public double? EndLatitudeDelta { get; set; }
        public double? EndLongitudeDelta { get; set; }

        public Location Region { get; set; }
        public List<MapPoint> Polygon { get; set; }
        public List<MapTile> Tiles { get; set; }

        [ForeignKey("Owner")]
        public int IdOwner { get; set; }
        public virtual AppUser Owner { get; set; }
        [NotMapped]
        public List<Filter> Filters { get; set; }

        public virtual List<AppUser> Members { get; set; }
        [NotMapped]
        public List<Traveler> Travelers { get; set; }

        public virtual List<AppUser> Subscribers { get; set; }
        [NotMapped]
        public List<Follower> Followers { get; set; }

        public virtual Itinerary Itinerary { get; set; }

        public Trip ToDTO()
        {
            return new Trip()
            {
                Id = this.Id,
                IdOwner = this.IdOwner,
                CreationDate = this.CreationDate,
                Name = this.Name,
                Description = this.Description,
                Note = this.Note,
                CoverImage = this.CoverImage,
                StartDate = this.StartDate,
                StartLatitude = this.StartLatitude,
                StartLatitudeDelta = this.StartLatitudeDelta,
                StartLongitude = this.StartLongitude,
                StartLongitudeDelta = this.StartLongitudeDelta,
                EndDate = this.EndDate,
                EndLatitude = this.EndLatitude,
                EndLatitudeDelta = this.EndLatitudeDelta,
                EndLongitude = this.EndLongitude,
                EndLongitudeDelta = this.EndLongitudeDelta,
                Region = this.Region,
                Polygon = this.Polygon.ToDTO(),
                Tiles = this.Tiles.ToDTO(),
                Travelers = this.Members == null ? new List<Traveler>() : this.Members.Select(m => new Traveler(m)).ToList().ToDTO(),
                Followers = this.Subscribers == null ? new List<Follower>() : this.Subscribers.Select(f => new Follower(f)).ToList().ToDTO(),
                Itinerary = this.Itinerary.ToDTO()
            };
        }
    }
}
