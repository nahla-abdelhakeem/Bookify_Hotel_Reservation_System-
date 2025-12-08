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
    }
}
