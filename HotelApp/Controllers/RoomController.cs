using Microsoft.AspNetCore.Mvc;
using HotelApp.Services;

namespace HotelApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_roomService.GetRooms());
        }
    }
}