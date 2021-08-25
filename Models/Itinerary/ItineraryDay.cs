using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class ItineraryDay : ModelBase<ItineraryDay>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double DistanceToStart { get; set; }
        public double DistanceToEnd { get; set; }
        public List<ItineraryDayStep> Steps { get; set; }

        [ForeignKey("Itinerary")]
        public int IdItinerary { get; set; }
        public virtual Itinerary Itinerary { get; set; }

        public ItineraryDay ToDTO()
        {
            return new ItineraryDay()
            {
                Id = this.Id,
                Index = this.Index,
                Name = this.Name,
                Date = this.Date,
                DistanceToStart = this.DistanceToStart,
                DistanceToEnd = this.DistanceToEnd,
                Steps = this.Steps.ToDTO(),
            };
        }
    }
}
