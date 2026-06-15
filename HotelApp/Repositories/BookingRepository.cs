using HotelApp.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Repositories
{
    public class BookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public List<Booking> GetAll()
        {
            return _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.BookingGuests)
                    .ThenInclude(bg => bg.Guest)
                .ToList();
        }

        public bool HasConflict(int roomId, DateTime start, DateTime end)
        {
            return _context.Bookings.Any(b =>
                b.RoomId == roomId &&
                !(end <= b.StartDate || start >= b.EndDate)
            );
        }

        public List<Booking> SearchByGuest(string query)
        {
            return _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.BookingGuests)
                    .ThenInclude(bg => bg.Guest)
                .Where(b =>
                    b.BookingGuests.Any(bg =>
                        bg.Guest.FullName.Contains(query) ||
                        bg.Guest.DocumentNumber.Contains(query)
                    )
                )
                .ToList();
        }

        public Booking GetById(int id)
        {
            return _context.Bookings.FirstOrDefault(b => b.Id == id);
        }

        public void Update(Booking booking)
        {
            _context.Bookings.Update(booking);
            _context.SaveChanges();
        }

        public List<Booking> GetActiveAndFuture()
        {
            var today = DateTime.UtcNow;

            return _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.BookingGuests)
                    .ThenInclude(bg => bg.Guest)
                .Where(b => b.EndDate >= today)
                .OrderBy(b => b.StartDate)
                .ToList();
        }
    }
}