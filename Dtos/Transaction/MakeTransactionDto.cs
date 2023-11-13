using System.ComponentModel.DataAnnotations;

namespace RidePassAPI.Dtos.Transaction
{
    public class MakeTransactionDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
