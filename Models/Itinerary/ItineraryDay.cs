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
        public double DistanceToStart { get; set; }
        public double DistanceToEnd { get; set; }
        public List<ItineraryDayStep> Steps { get; set; }

        public ItineraryDay ToDTO()
        {
            return new ItineraryDay()
            {
                Id = this.Id,
                Index = this.Index,
                DistanceToStart = this.DistanceToStart,
                DistanceToEnd = this.DistanceToEnd,
                Steps = this.Steps.ToDTO(),
            };
        }
    }
}
