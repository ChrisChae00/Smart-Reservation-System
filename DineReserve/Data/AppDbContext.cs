using Microsoft.EntityFrameworkCore;
using DineReserve.Models;

namespace DineReserve.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
