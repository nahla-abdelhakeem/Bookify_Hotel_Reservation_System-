namespace BookifyHotelSystem.View_Model
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; } = "/Images/avatar.png";

        public List<BookingHistoryViewModel> BookingHistory { get; set; }
    }
}
