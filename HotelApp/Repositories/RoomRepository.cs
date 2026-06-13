using HotelApp.Data;
using HotelApp.Models;

namespace HotelApp.Repositories
{
    public class RoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Room> GetAll()
        {
            return _context.Rooms.ToList();
        }

        public Room GetById(int id)
        {
            return _context.Rooms.FirstOrDefault(r => r.Id == id);
        }
    }
}