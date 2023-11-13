namespace RidePassAPI.Responses
{
    public class ApiExceptionResponse : ErrorResponse
    {
        public ApiExceptionResponse(int statusCode, string message, string? details = null) :
            base(statusCode, message)
        {
            Details = details;
        }

        public string? Details { get; private set; }
    }
}
