namespace HotelApp.DTOs
{
    public class CreateBookingDto
    {
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfGuests { get; set; }
        public List<int> GuestIds { get; set; }
    }
}