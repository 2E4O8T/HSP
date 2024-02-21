using CalendarApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarApi.Data
{
    public class CalendarApiDbContext : DbContext
    {
        public CalendarApiDbContext(DbContextOptions<CalendarApiDbContext>options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SimpleCalendar>();
        }

        public DbSet<SimpleCalendar> SimpleCalendar { get; set; }
    }
}
