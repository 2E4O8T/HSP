using BookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Data
{
    public class BookingApiDbContext : DbContext
    {
        public BookingApiDbContext(DbContextOptions<BookingApiDbContext> options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SimpleBooking>();
        }

        public DbSet<SimpleBooking> SimpleBooking { get; set; }
    }
}
