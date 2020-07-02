using TripickServer.Models;

namespace TripickServer.Requests
{
    public abstract class IRequest
    {
        public AuthenticationKeys AuthenticationKeys { get; set; }
    }
}
