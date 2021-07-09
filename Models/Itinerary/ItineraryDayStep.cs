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
        public Pick Visit { get; set; }

        public ItineraryDayStep ToDTO()
        {
            return new ItineraryDayStep()
            {
                Id = this.Id,
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
                Visit = this.Visit?.ToDTO(),
            };
        }
    }
}
