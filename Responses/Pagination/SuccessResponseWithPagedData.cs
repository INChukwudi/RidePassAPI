namespace RidePassAPI.Responses.Pagination
{
    public class SuccessResponseWithPagedData<T> : SuccessResponse where T : class
    {
        public SuccessResponseWithPagedData(int statusCode, string message, Pagination<T> data)
            : base(statusCode, message)
        {
            Data = data;
        }

        public Pagination<T>? Data { get; set; }
    }
}
