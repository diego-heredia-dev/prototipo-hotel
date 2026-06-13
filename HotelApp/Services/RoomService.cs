using HotelApp.Models;
using HotelApp.Repositories;

namespace HotelApp.Services
{
    public class RoomService
    {
        private readonly RoomRepository _roomRepository;

        public RoomService(RoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public List<Room> GetRooms()
        {
            return _roomRepository.GetAll();
        }
    }
}