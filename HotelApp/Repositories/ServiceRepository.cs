using HotelApp.Data;
using HotelApp.Models;

namespace HotelApp.Repositories
{
    public class ServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Service> GetAll()
        {
            return _context.Services.ToList();
        }
    }
}