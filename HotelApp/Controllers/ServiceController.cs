using Microsoft.AspNetCore.Mvc;
using HotelApp.Services;

namespace HotelApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly ServiceService _service;

        public ServiceController(ServiceService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }
    }
}