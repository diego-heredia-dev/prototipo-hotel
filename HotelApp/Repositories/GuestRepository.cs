using HotelApp.Data;
using HotelApp.Models;

namespace HotelApp.Repositories
{
    public class GuestRepository
    {
        private readonly AppDbContext _context;

        public GuestRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Guest guest)
        {
            _context.Guests.Add(guest);
            _context.SaveChanges();
        }

        public Guest GetByDocument(string document)
        {
            return _context.Guests.FirstOrDefault(g => g.DocumentNumber == document);
        }
    }
}