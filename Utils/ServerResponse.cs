namespace TripickServer.Utils
{
    public class ServerResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }

        public ServerResponse()
        {
            this.IsSuccess = true;
            this.Message = string.Empty;
        }
        public ServerResponse(T t)
        {
            this.IsSuccess = true;
            this.Message = string.Empty;
            this.Result = t;
        }
    }
}
