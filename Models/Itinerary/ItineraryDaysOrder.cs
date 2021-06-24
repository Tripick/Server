using System.Collections.Generic;

namespace TripickServer.Models
{
    public class ItineraryDaysOrder
    {
        public List<ItineraryDay> Days { get; set; }
        public double TotalDistance { get; set; }
    }
}
