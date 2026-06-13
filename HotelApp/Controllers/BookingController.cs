using HotelApp.DTOs;
using HotelApp.Models;
using HotelApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // POST: api/booking
        [HttpPost]
        public IActionResult Create([FromBody] CreateBookingDto dto)
        {
            try
            {
                _bookingService.CreateBooking(dto);
                return Ok("Reserva creada exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/booking
        [HttpGet]
        public IActionResult GetAll()
        {
            var bookings = _bookingService.GetAll();
            return Ok(bookings);
        }

        // GET: api/booking/search?query=...
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string query)
        {
            var result = _bookingService.SearchBookings(query);
            return Ok(result);
        }

        [HttpPost("{id}/checkin")]
        public IActionResult CheckIn(int id)
        {
            try
            {
                _bookingService.CheckIn(id);
                return Ok("Check-in exitoso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("active")]
        public IActionResult GetActiveAndFuture()
        {
            var bookings = _bookingService.GetActiveAndFutureBookings();

            return Ok(bookings);
        }
    }
}