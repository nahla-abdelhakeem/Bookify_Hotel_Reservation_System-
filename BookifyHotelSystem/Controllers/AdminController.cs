using BookifyHotelSystem.DatabaseServices.Unit_Of_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookifyHotelSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Dashboard()
        {
            // Get statistics
            var totalUsers = unitOfWork.UserRepo.GetAll().Count();
            var totalRoles = unitOfWork.RoleRepo.GetAll().Count();
            var totalRoomTypes = unitOfWork.RoomTypeRepo.GetAll().Count();
            var totalRooms = unitOfWork.RoomRepo.GetAll().Count();
            var totalBookings = unitOfWork.BookingRepo.GetAll().Count();
            var pendingBookings = unitOfWork.BookingRepo.GetAll()
                .Count(b => b.StartDate > DateTime.Now);

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalRoles = totalRoles;
            ViewBag.TotalRoomTypes = totalRoomTypes;
            ViewBag.TotalRooms = totalRooms;
            ViewBag.TotalBookings = totalBookings;
            ViewBag.PendingBookings = pendingBookings;

            return View();
        }
    }
}
