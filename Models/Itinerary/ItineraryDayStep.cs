namespace TripickServer.Models
{
    public class ItineraryDayStep
    {
        public bool IsPassage { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsVisit { get; set; }
        public bool IsSuggestion { get; set; }
        public int Index { get; set; }
        public Pick Visit { get; set; }
        public double DistanceToPassage { get; set; }
        public double VisitLikely { get; set; }
    }
}
