﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class Trip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsPublic { get; set; }
        public string CoverImage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool EnabledStartDate { get; set; }
        public DateTime? StartDate { get; set; }
        public bool EnabledEndDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool EnabledNumberOfDays { get; set; }
        public int NumberOfDays { get; set; }
        public bool EnabledStartPoint { get; set; }
        public double? StartLatitude { get; set; }
        public double? StartLongitude { get; set; }
        public double? StartLatitudeDelta { get; set; }
        public double? StartLongitudeDelta { get; set; }
        public bool EnabledEndPoint { get; set; }
        public double? EndLatitude { get; set; }
        public double? EndLongitude { get; set; }
        public double? EndLatitudeDelta { get; set; }
        public double? EndLongitudeDelta { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }

        // Filters
        public int FilterIntense { get; set; } = 3;
        public int FilterSportive { get; set; } = 3;
        public int FilterCity { get; set; } = 3;
        public int FilterFamous { get; set; } = 3;
        public int FilterFar { get; set; } = 3;
        public int FilterExpensive { get; set; } = 3;

        [ForeignKey("Owner")]
        public int IdOwner { get; set; }
        public virtual AppUser Owner { get; set; }

        public virtual Itinerary Itinerary { get; set; }

        public virtual List<Destination> Destinations { get; set; }
        public virtual List<AppUser> Members { get; set; }
        public virtual List<Pick> Picks { get; set; }
    }
}
