namespace RidePassAPI.Responses
{
    public class ApiValidationErrorResponse : ErrorResponse
    {
        public ApiValidationErrorResponse(IReadOnlyList<string> errors) :
            base(400, "One or more validation errors occured")
        {
            Errors = errors;
        }

        public IReadOnlyList<string> Errors { get; set; }
    }
}
