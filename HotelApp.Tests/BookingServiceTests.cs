using HotelApp.Data;
using HotelApp.DTOs;
using HotelApp.Models;
using HotelApp.Repositories;
using HotelApp.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HotelApp.Tests
{
    [TestFixture]
    public class BookingServiceTests
    {
        private AppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void CreateBooking_EndDateBeforeStartDate_ThrowsException()
        {
            // Arrange
            var bookingRepository = new BookingRepository(_context);
            var roomRepository = new RoomRepository(_context);

            var service = new BookingService(
                bookingRepository,
                roomRepository
            );

            var dto = new CreateBookingDto
            {
                RoomId = 1,
                StartDate = DateTime.UtcNow.AddDays(10),
                EndDate = DateTime.UtcNow.AddDays(15),
                GuestIds = new List<int> { 1 },
                NumberOfGuests = 1
            };

            // Act & Assert
            Assert.Throws<Exception>(() =>
                service.CreateBooking(dto)
            );
        }

        [Test]
        public void CreateBooking_RoomAlreadyReserved_ThrowsException()
        {
            // Arrange
            var room = new Room
            {
                Id = 1,
                Type = "Suite",
                Capacity = 2,
                Price = 100
            };

            _context.Rooms.Add(room);

            _context.Bookings.Add(new Booking
            {
                RoomId = 1,
                StartDate = DateTime.UtcNow.AddDays(10),
                EndDate = DateTime.UtcNow.AddDays(15),
            });

            _context.SaveChanges();

            var bookingRepository = new BookingRepository(_context);
            var roomRepository = new RoomRepository(_context);

            var service = new BookingService(
                bookingRepository,
                roomRepository
            );

            var dto = new CreateBookingDto
            {
                RoomId = 1,
                StartDate = DateTime.UtcNow.AddDays(12),
                EndDate = DateTime.UtcNow.AddDays(14),
                GuestIds = new List<int> { 1 },
                NumberOfGuests = 1
            };

            // Act & Assert
            Assert.Throws<Exception>(() =>
                service.CreateBooking(dto)
            );
        }

        [Test]
        public void CreateBooking_GuestCountExceedsRoomCapacity_ThrowsException()
        {
            // Arrange
            var room = new Room
            {
                Id = 1,
                Type = "Simple",
                Capacity = 1,
                Price = 100
            };

            _context.Rooms.Add(room);
            _context.SaveChanges();

            var bookingRepository = new BookingRepository(_context);
            var roomRepository = new RoomRepository(_context);

            var service = new BookingService(
                bookingRepository,
                roomRepository
            );

            var dto = new CreateBookingDto
            {
                RoomId = 1,
                StartDate = DateTime.UtcNow.AddDays(10),
                EndDate = DateTime.UtcNow.AddDays(15),
                GuestIds = new List<int> { 1, 2 },
                NumberOfGuests = 2
            };

            // Act & Assert
            Assert.Throws<Exception>(() =>
                service.CreateBooking(dto)
            );
        }

        [Test]
        public void CheckIn_AlreadyCheckedIn_ThrowsException()
        {
            // Arrange
            var booking = new Booking
            {
                Id = 1,
                Status = BookingStatus.IN_PROGRESS
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            var bookingRepository = new BookingRepository(_context);
            var roomRepository = new RoomRepository(_context);

            var service = new BookingService(
                bookingRepository,
                roomRepository
            );

            // Act & Assert
            Assert.Throws<Exception>(() =>
                service.CheckIn(1)
            );
        }

        [Test]
        public void CreateBooking_ValidBooking_SavesBookingSuccessfully()
        {
            // Arrange
            var room = new Room
            {
                Id = 1,
                Type = "Suite",
                Capacity = 2,
                Price = 100
            };

            _context.Rooms.Add(room);
            _context.SaveChanges();

            var bookingRepository = new BookingRepository(_context);
            var roomRepository = new RoomRepository(_context);

            var service = new BookingService(
                bookingRepository,
                roomRepository
            );

            var dto = new CreateBookingDto
            {
                RoomId = 1,
                StartDate = DateTime.UtcNow.AddDays(10),
                EndDate = DateTime.UtcNow.AddDays(15),
                GuestIds = new List<int> { 1, 2 },
                NumberOfGuests = 2
            };

            // Act
            service.CreateBooking(dto);

            var booking = _context.Bookings.FirstOrDefault();

            // Assert
            Assert.That(booking, Is.Not.Null);
            Assert.That(booking.Status, Is.EqualTo(BookingStatus.RESERVED));
        }

        [Test]
        public void IsGuestDataValid_MissingFullName_ReturnFalse()
        {
            //Arrange
            var dto = new CreateGuestDto
            {
                FullName = "",
                DocumentNumber = "123456"
            };

            //Act
            var result = GuestService.IsGuestDataValid(dto);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsBookingExpired_PastCheckOutDate_ReturnsTrue()
        {
            //Arrange
            var booking = new Booking
            {
                EndDate = DateTime.UtcNow.AddDays(-1),
            };

            //Act
            var result = BookingService.IsBookingExpired(booking);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsSearchQueryValid_EmptyQuery_ReturnsFalse()
        {
            //Arrange
            string query = "";

            //Act
            var result = BookingService.IsSearchQueryValid(query);

            //Assert
            Assert.That(result, Is.False);
        }
    }
}