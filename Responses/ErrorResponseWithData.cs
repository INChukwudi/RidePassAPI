namespace RidePassAPI.Responses
{
    public class ErrorResponseWithData<T> : ErrorResponse
    {
        public ErrorResponseWithData(int statusCode, string message, T data)
            : base(statusCode, message)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
