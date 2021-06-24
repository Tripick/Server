namespace TripickServer.Models
{
    public class ClosestPassage
    {
        public Passage Passage { get; set; }
        public Pick Visit { get; set; }
        public double Distance { get; set; }
        public double VisitLikely { get; set; }
    }
}
