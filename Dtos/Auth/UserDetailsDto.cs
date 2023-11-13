namespace RidePassAPI.Dtos.Auth
{
    public class UserDetailsDto
    {
        public string? UserId { get; set; }
        
        public int? UserType { get; set; }
        
        public string? Email { get; set; }
        
        public string? Token { get; set; }
    }
}
