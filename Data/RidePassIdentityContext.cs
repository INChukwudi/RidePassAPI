using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RidePassAPI.Models.IdentityModels;

namespace RidePassAPI.Data
{
    public class RidePassIdentityContext : IdentityDbContext<AppUser>
    {
        public RidePassIdentityContext(DbContextOptions<RidePassIdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
