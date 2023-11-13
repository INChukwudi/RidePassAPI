namespace RidePassAPI.Responses
{
    public class SuccessResponse : ApiResponse
    {
        public SuccessResponse(int statusCode,string message)
            : base("Success", statusCode, message)
        {
        }
    }
}
