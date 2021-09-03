using TripickServer.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace TripickServer.Models
{
    public class ItineraryDayStep : ModelBase<ItineraryDayStep>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime? Time { get; set; }
        public bool IsPassage { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public bool IsVisit { get; set; }
        public bool IsSuggestion { get; set; }
        public double DistanceToPassage { get; set; }
        public double VisitLikely { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [ForeignKey("Day")]
        public int IdDay { get; set; }
        public virtual ItineraryDay Day { get; set; }

        [ForeignKey("Visit")]
        public int? IdVisit { get; set; }
        public virtual Pick Visit { get; set; }

        public ItineraryDayStep ToDTO()
        {
            return new ItineraryDayStep()
            {
                Id = this.Id,
                IdDay = this.IdDay,
                Index = this.Index,
                Time = this.Time,
                IsPassage = this.IsPassage,
                IsStart = this.IsStart,
                IsEnd = this.IsEnd,
                IsVisit = this.IsVisit,
                IsSuggestion = this.IsSuggestion,
                DistanceToPassage = this.DistanceToPassage,
                VisitLikely = this.VisitLikely,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                IdVisit = this.IdVisit,
                Visit = this.Visit?.ToDTO(),
            };
        }
    }
}
