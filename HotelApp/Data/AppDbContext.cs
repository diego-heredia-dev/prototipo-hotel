using Microsoft.EntityFrameworkCore;
using HotelApp.Models;

namespace HotelApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
                .Property(r => r.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BookingGuest>()
                .HasKey(bg => new { bg.BookingId, bg.GuestId });

            modelBuilder.Entity<BookingGuest>()
                .HasOne(bg => bg.Booking)
                .WithMany(b => b.BookingGuests)
                .HasForeignKey(bg => bg.BookingId);

            modelBuilder.Entity<BookingGuest>()
                .HasOne(bg => bg.Guest)
                .WithMany(g => g.BookingGuests)
                .HasForeignKey(bg => bg.GuestId);
            
            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasConversion<string>();
        }

        public DbSet<Guest> Guests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Service> Services { get; set; }
    }
}