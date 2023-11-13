using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RidePassAPI.Models;

namespace RidePassAPI.Data
{
    public class RidePassAPIContext : DbContext
    {
        public RidePassAPIContext (DbContextOptions<RidePassAPIContext> options)
            : base(options)
        {
        }

        public DbSet<RidePassAPI.Models.Driver> Driver { get; set; } = default!;

        public DbSet<RidePassAPI.Models.Passenger> Passenger { get; set; } = default!;

        public DbSet<RidePassAPI.Models.Transaction> Transaction { get; set; } = default!;

        public DbSet<RidePassAPI.Models.Wallet> Wallet { get; set; } = default!;
    }
}
