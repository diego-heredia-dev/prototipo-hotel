namespace HotelApp.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public string RoomType { get; set; }
        public int RoomCapacity { get; set; }
        public decimal RoomPrice { get; set; }
        public List<string> Guests { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string Status { get; set; }
    }
}