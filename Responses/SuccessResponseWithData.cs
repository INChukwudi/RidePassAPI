namespace RidePassAPI.Responses
{
    public class SuccessResponseWithData<T> : SuccessResponse
    {
        public SuccessResponseWithData(int statusCode, string message, T data)
            : base(statusCode, message)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
