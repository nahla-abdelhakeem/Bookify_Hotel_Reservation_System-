using BookifyHotelSystem.DatabaseServices.BaseRepository;
using BookifyHotelSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookifyHotelSystem.DatabaseServices.Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        public DbSet<Booking> bookingRepo { get; set; }
        public BookingRepository(AppDbContext context) : base(context)
        {
            bookingRepo = context.Bookings;
        }

        public List<Booking> GetBookingsByUserName(string userName)
        {
            var bookings = bookingRepo.Include(b => b.RoomBookings).ThenInclude(rb => rb.Room)
                                      .Where(b => b.UserName == userName)
                                      .OrderByDescending(b => b.StartDate).ToList();
            return bookings;
        }

        public bool IsRoomAvailable(int roomId, DateTime startDate, DateTime endDate)
        {
            bool isConflict = false;
            var bookings = bookingRepo.Include(b => b.RoomBookings)
                                      .Where(b => b.RoomBookings.Any(rb => rb.RoomId == roomId))
                                      .ToList();
            foreach (var booking in bookings)
            {
                if (startDate < booking.EndDate && endDate > booking.StartDate)
                {
                    isConflict = true;
                    break;
                }
            }
            return !isConflict;
        }
    }
}
