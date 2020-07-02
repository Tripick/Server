namespace TripickServer.Requests.Trip
{
    public class RequestGetAll
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
