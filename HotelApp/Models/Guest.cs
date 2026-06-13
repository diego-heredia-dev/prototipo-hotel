namespace HotelApp.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string DocumentNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<BookingGuest> BookingGuests { get; set; }
    }
}
