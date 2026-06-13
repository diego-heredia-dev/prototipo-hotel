namespace HotelApp.Models
{
    public enum BookingStatus
    {
        RESERVED,
        IN_PROGRESS,
        COMPLETED,
        CANCELLED
    }

    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public List<BookingGuest> BookingGuests { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfGuests { get; set; }
        public BookingStatus Status { get; set; }
    }
}
