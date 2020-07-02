using TripickServer.Models;

namespace TripickServer.Requests
{
    public class Request<T> : IRequest
    {
        public T Data { get; set; }
    }
}
