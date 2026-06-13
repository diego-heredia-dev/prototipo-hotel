using Microsoft.AspNetCore.Mvc;
using HotelApp.Services;
using HotelApp.DTOs;

namespace HotelApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestController : ControllerBase
    {
        private readonly GuestService _guestService;

        public GuestController(GuestService guestService)
        {
            _guestService = guestService;
        }

        [HttpPost]
        public IActionResult Create(CreateGuestDto dto)
        {
            try
            {
                _guestService.CreateGuest(dto);
                return Ok("Guest created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}