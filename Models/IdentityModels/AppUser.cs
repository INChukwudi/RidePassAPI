using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RidePassAPI.Models.IdentityModels
{
    public class AppUser : IdentityUser
    {
        [Required]
        public int? UserId { get; set; }

        [Required]
        public int? UserType { get; set; }
    }

    public enum UserTypes
    {
        Customer,
        Beneficiary,
        MinistryAdmin,
    }
}
