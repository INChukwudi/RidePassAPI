namespace RidePassAPI.Models
{
    public class Transaction : BaseModel
    {
        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public double Amount { get; set; }

        public string Description { get; set; }
    }
}
