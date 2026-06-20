using HotelApp.DTOs;
using HotelApp.Models;
using HotelApp.Repositories;

namespace HotelApp.Services
{
    public class GuestService
    {
        private readonly GuestRepository _guestRepository;

        public GuestService(GuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

        public void CreateGuest(CreateGuestDto dto)
        {
            var existing = _guestRepository.GetByDocument(dto.DocumentNumber);

            if (existing != null)
                throw new Exception("Guest with this document already exists");

            var guest = new Guest
            {
                FullName = dto.FullName,
                DocumentNumber = dto.DocumentNumber,
                Phone = dto.Phone,
                Email = dto.Email
            };

            _guestRepository.Add(guest);
        }

        public static bool IsGuestDataValid(CreateGuestDto dto)
        {
            return !string.IsNullOrWhiteSpace(dto.FullName)
                && !string.IsNullOrWhiteSpace(dto.DocumentNumber);
        }
    }
}