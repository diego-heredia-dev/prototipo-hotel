using HotelApp.DTOs;
using HotelApp.Models;
using HotelApp.Repositories;

namespace HotelApp.Services
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly RoomRepository _roomRepository;

        public BookingService(BookingRepository bookingRepository, RoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public void CreateBooking(CreateBookingDto dto)
        {
            ValidateBookingDates(dto);

            ValidateRoomAvailability(dto);

            var booking = BuildBooking(dto);

            _bookingRepository.Add(booking);
        }

        private void ValidateBookingDates(CreateBookingDto dto)
        {
            
            if (dto.EndDate <= dto.StartDate)
                throw new Exception("La fecha de salida debe ser posterior a la fecha de ingreso.");

            if (dto.StartDate < DateTime.UtcNow)
                throw new Exception("No se pueden crear reservas para fechas pasadas");
        }

        private void ValidateRoomAvailability(CreateBookingDto dto)
        {
            if (_bookingRepository.HasConflict(dto.RoomId, dto.StartDate, dto.EndDate))
                throw new Exception("La habitacion ya esta reservada en esas fechas.");

            var room = _roomRepository.GetById(dto.RoomId);

            if (room == null)
                throw new Exception("Habitacion no encontrada.");

            if (dto.NumberOfGuests > room.Capacity)
                throw new Exception("La cantidad de huespedes excede la capacidad.");
        }

        private Booking BuildBooking(CreateBookingDto dto)
        {
            return new Booking
            {
                RoomId = dto.RoomId,
                StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
                Status = BookingStatus.RESERVED,
                BookingGuests = dto.GuestIds.Select(id => new BookingGuest
                {
                    GuestId = id
                }).ToList()
            };
        }

        public List<BookingResponseDto> SearchBookings(string query)
        {
            var bookings = _bookingRepository.SearchByGuest(query);

            return bookings.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                RoomType = b.Room.Type,
                RoomCapacity = b.Room.Capacity,
                RoomPrice = b.Room.Price,
                Guests = b.BookingGuests
                    .Select(bg => bg.Guest.FullName)
                    .ToList(),
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                NumberOfGuests = b.NumberOfGuests,
                Status = b.Status.ToString()
            }).ToList();
        }

        public List<Booking> GetAll()
        {
            return _bookingRepository.GetAll();
        }

        public void CheckIn(int bookingId)
        {
            var booking = _bookingRepository.GetById(bookingId);

            if (booking == null)
                throw new Exception("Reserva no encontrada");

            if (IsBookingExpired(booking))
                throw new Exception("La reserva ha expirado");

            if (booking.Status == BookingStatus.CANCELLED)
                throw new Exception("No se puede hacer check-in a una reserva eliminada");

            if (booking.Status == BookingStatus.IN_PROGRESS)
                throw new Exception("Check-in ya hecho previamente");

            booking.Status = BookingStatus.IN_PROGRESS;

            _bookingRepository.Update(booking);
        }

        public List<BookingResponseDto> GetActiveAndFutureBookings()
        {
            var bookings = _bookingRepository.GetActiveAndFuture();

            return bookings.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                RoomType = b.Room.Type,
                RoomCapacity = b.Room.Capacity,
                RoomPrice = b.Room.Price,
                Guests = b.BookingGuests
                    .Select(bg => bg.Guest.FullName)
                    .ToList(),
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                NumberOfGuests = b.NumberOfGuests,
                Status = b.Status.ToString()
            }).ToList();
        }

        public static bool IsBookingExpired(Booking booking)
        {
            return booking.EndDate < DateTime.UtcNow;
        }
    }
}