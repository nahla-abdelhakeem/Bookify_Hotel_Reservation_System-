using BookifyHotelSystem.DatabaseServices.BaseRepository;
using BookifyHotelSystem.Models;

namespace BookifyHotelSystem.DatabaseServices.Repositories
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        List<Booking> GetBookingsByUserName(string userName);
        public bool IsRoomAvailable(int roomId, DateTime startDate, DateTime endDate);
    }
}
