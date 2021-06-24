using System.Collections.Generic;

namespace TripickServer.Models
{
    public class ItineraryDay
    {
        public int Index { get; set; }
        public double DistanceToStart { get; set; }
        public double DistanceToEnd { get; set; }
        public List<ItineraryDayStep> Steps { get; set; }
    }
}
