namespace RidePassAPI.Responses
{
    public abstract class ApiResponse
    {
        public ApiResponse(string status, int statusCode, string message)
        {
            Status = status;
            StatusCode = statusCode;
            Message = message;
        }

        public string Status { get; set; }
        
        public string? StatusText { get; set; }

        private int statusCode;

        public int StatusCode {
            get { return statusCode; }

            set {
                statusCode = value;
                StatusText = GetStatusCodeText(value);
            } 
        }
        
        public string Message { get; set; }

        private static string GetStatusCodeText(int statusCode)
        {
            return statusCode switch
            {
                200 => "Ok",
                201 => "Created",
                202 => "Accepted",
                204 => "No Content",
                300 => "Multiple Choices",
                301 => "Moved Permanently",
                400 => "Bad Request",
                401 => "Unauthorized",
                402 => "Payment Required",
                403 => "Forbidden",
                404 => "Not Found",
                405 => "Method Not Allowed",
                415 => "Unsupported Media Type",
                500 => "Internal Server Error",
                502 => "Bad Gateway",
                503 => "Service Unavailable",
                504 => "Gateway Timeout",
                _ => "Ok",
            };
        }
    }
}
