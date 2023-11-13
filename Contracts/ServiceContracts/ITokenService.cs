using RidePassAPI.Models.IdentityModels;

namespace RidePassAPI.Contracts.ServiceContracts
{
    public interface ITokenService
    {
        public string CreateToken(AppUser user);
    }
}
