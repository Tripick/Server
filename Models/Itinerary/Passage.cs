namespace TripickServer.Models
{
    public class Passage
    {
        public Pick Pick { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
