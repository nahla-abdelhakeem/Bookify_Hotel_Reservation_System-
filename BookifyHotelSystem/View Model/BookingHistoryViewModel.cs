namespace BookifyHotelSystem.View_Model
{
    public class BookingHistoryViewModel
    {
        public int BookingNo { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int TotalNights { get; set; }

        public decimal TotalPrice { get; set; }
        public List<string> RoomNames { get; set; }
    }
}
