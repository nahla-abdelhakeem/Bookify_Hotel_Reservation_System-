using BookifyHotelSystem.DatabaseServices.Unit_Of_Work;
using BookifyHotelSystem.Models;
using BookifyHotelSystem.View_Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookifyHotelSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.unitOfWork = unitOfWork;
        }
        //get
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await userManager.FindByNameAsync(loginVm.UserName);

                if (applicationUser is not null)
                {
                    bool isFound = await userManager.CheckPasswordAsync(applicationUser, loginVm.Password);

                    if (isFound)
                    {
                        if (applicationUser.LockoutEnd != null)
                        {
                            return View("LockedPage");
                        }

                        await signInManager.SignInAsync(applicationUser, loginVm.RememberMe);

                        // Check if user is Admin
                        var isAdmin = await userManager.IsInRoleAsync(applicationUser, "Admin");
                        if (isAdmin)
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid user name or password");

            }
            return View(loginVm);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegiterViewModel registerVm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    UserName = registerVm.UserName,
                    Email = registerVm.Email,
                    PasswordHash = registerVm.Password,
                    FullName = registerVm.FullName,
                };

                IdentityResult result = await userManager.CreateAsync(applicationUser, registerVm.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(applicationUser, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(registerVm);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }




        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            var userName = User.Identity.Name;
            var user = userManager.Users.FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var bookings = unitOfWork.BookingRepo.GetBookingsByUserName(userName);

            if (bookings == null)
            {
                bookings = new List<Models.Booking>();
            }

            var bookingHistory = bookings.Select(b => new BookingHistoryViewModel
            {
                BookingNo = b.BookingNo,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                TotalNights = (b.EndDate - b.StartDate).Days,
                TotalPrice = b.TotalPrice,
                RoomNames = b.RoomBookings.Select(rb => rb.Room.Name).ToList()
            }).ToList();

            var profileVm = new ProfileViewModel
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                BookingHistory = bookingHistory
            };

            return View(profileVm);
        }
    }
}
