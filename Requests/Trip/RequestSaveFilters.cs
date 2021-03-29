using System.Collections.Generic;
using TripickServer.Models;

namespace TripickServer.Requests.Trip
{
    public class RequestSaveFilters
    {
        public int IdTrip { get; set; }
        public List<Filter> Filters { get; set; }
    }
}
