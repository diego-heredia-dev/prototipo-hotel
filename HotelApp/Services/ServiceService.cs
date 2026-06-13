using HotelApp.Models;
using HotelApp.Repositories;

namespace HotelApp.Services
{
    public class ServiceService
    {
        private readonly ServiceRepository _repository;

        public ServiceService(ServiceRepository repository)
        {
            _repository = repository;
        }

        public List<Service> GetAll()
        {
            return _repository.GetAll();
        }
    }
}