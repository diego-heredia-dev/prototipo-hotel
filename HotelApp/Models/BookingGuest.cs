using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models
{
    [Table("BookingGuests")]
    public class BookingGuest
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
    }
}