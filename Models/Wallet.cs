namespace RidePassAPI.Models
{
    public class Wallet : BaseModel
    {
        public int OwnerId { get; set; }
        
        public byte OwnerType { get; set; }

        public double Balance { get; set; }
    }
}
