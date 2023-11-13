namespace RidePassAPI.Responses
{
    public class ErrorResponse : ApiResponse
    {
        public ErrorResponse(int statusCode, string message)
            : base("Error", statusCode, message)
        {
        }
    }
}
