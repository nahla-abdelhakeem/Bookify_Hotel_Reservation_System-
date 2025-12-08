using BookifyHotelSystem.DatabaseServices.Repositories;
using BookifyHotelSystem.DatabaseServices.Unit_Of_Work;
using BookifyHotelSystem.Models;
using BookifyHotelSystem.Utilities;
using BookifyHotelSystem.View_Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookifyHotelSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public BookingController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var bookings = unitOfWork.BookingRepo.GetAll().ToList();
            return View(bookings);
        }

        [Authorize]
        public IActionResult Checkout()
        {

            List<RoomViewModel> rooms = HttpContext.Session.Get<List<RoomViewModel>>("rooms");

            // Update session dates from query parameters if provided
            if (Request.Query.Count > 0 && rooms != null)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    string startDateKey = $"[{i}].StartDate";
                    string endDateKey = $"[{i}].EndDate";

                    if (Request.Query.ContainsKey(startDateKey) && DateTime.TryParse(Request.Query[startDateKey], out DateTime startDate))
                    {
                        rooms[i].StartDate = startDate;
                    }

                    if (Request.Query.ContainsKey(endDateKey) && DateTime.TryParse(Request.Query[endDateKey], out DateTime endDate))
                    {
                        rooms[i].EndDate = endDate;
                    }
                }

                // Save updated rooms back to session
                HttpContext.Session.Set("rooms", rooms);
            }

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                ViewBag.UserName = User.Identity.Name;
            }
            
            ViewBag.OrderNumber = GetOrderNo();
            return View();
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Checkout(Booking booking)
        //{
        //    List<RoomViewModel> roomsVM =
        //        HttpContext.Session.Get<List<RoomViewModel>>("rooms");

        //    if (roomsVM != null && roomsVM.Any())
        //    {
        //        decimal total = 0;
        //        DateTime minDateTimeStart = DateTime.MaxValue;
        //        DateTime maxDateTimeEnd = DateTime.MinValue;
        //        foreach (var r in roomsVM)
        //        {

        //            RoomBooking roomBooking = new RoomBooking();
        //            roomBooking.RoomId = r.Id;

        //            booking.RoomBookings.Add(roomBooking);
        //            total += (r.PricePerNight * r.Nights);

        //            // get the first item in roomsVM to set booking dates
        //            if (r.StartDate.HasValue && r.StartDate.Value < minDateTimeStart)
        //                minDateTimeStart = r.StartDate.Value;
        //            if (r.EndDate.HasValue && r.EndDate.Value > maxDateTimeEnd)
        //                maxDateTimeEnd = r.EndDate.Value;
        //        }
        //        booking.StartDate = minDateTimeStart;
        //        booking.EndDate = maxDateTimeEnd;
        //        booking.TotalPrice = (int)total;  
        //    }

        //    unitOfWork.BookingRepo.Add(booking);

        //    HttpContext.Session.Set("rooms", new List<RoomViewModel>());
        //    return View();
        //}








        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(Booking booking)
        {
            List<RoomViewModel> roomsVM =
                HttpContext.Session.Get<List<RoomViewModel>>("rooms");

            if (roomsVM == null || !roomsVM.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            decimal total = 0;
            DateTime minDateTimeStart = DateTime.MaxValue;
            DateTime maxDateTimeEnd = DateTime.MinValue;

            foreach (var r in roomsVM)
            {
                RoomBooking roomBooking = new RoomBooking();
                roomBooking.RoomId = r.Id;

                booking.RoomBookings.Add(roomBooking);
                total += (r.PricePerNight * r.Nights);

                if (r.StartDate.HasValue && r.StartDate.Value < minDateTimeStart)
                    minDateTimeStart = r.StartDate.Value;

                if (r.EndDate.HasValue && r.EndDate.Value > maxDateTimeEnd)
                    maxDateTimeEnd = r.EndDate.Value;
            }

            booking.StartDate = minDateTimeStart;
            booking.EndDate = maxDateTimeEnd;
            booking.TotalPrice = (int)total;

            HttpContext.Session.Set("PendingBooking", booking);


            var domain = "http://localhost:25552";

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
        {
            new Stripe.Checkout.SessionLineItemOptions
            {
                Quantity = 1,
                PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(booking.TotalPrice * 100),
                    ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Hotel Room Booking"
                    }
                }
            }
        },
                SuccessUrl = domain + "/Booking/Success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/Booking/Cancel"
            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            return Redirect(session.Url);

        }


        public IActionResult Success(string session_id)
        {
            var booking = HttpContext.Session.Get<Booking>("PendingBooking");

            if (booking == null)
            {
                return RedirectToAction("Index", "Home");
            }


            var sessionService = new Stripe.Checkout.SessionService();
            var session = sessionService.Get(session_id);

            if (session.PaymentStatus == "paid")
            {
                unitOfWork.BookingRepo.Add(booking);

                ViewBag.BookingNumber = booking.BookingNo;
                ViewBag.TotalPrice = booking.TotalPrice;
                ViewBag.PaymentId = session.PaymentIntentId;

                HttpContext.Session.Remove("PendingBooking");
                HttpContext.Session.Set("rooms", new List<RoomViewModel>());

                return View();
            }
            else
            {

                return RedirectToAction("Cancel");
            }
        }

        public IActionResult Cancel()
        {
            var booking = HttpContext.Session.Get<Booking>("PendingBooking");

            if (booking != null)
            {
                ViewBag.Message = "Payment was cancelled. You can try again from your cart.";

                HttpContext.Session.Remove("PendingBooking");
            }

            return View();
        }










        public int GetOrderNo()
        {
            int rowCount = unitOfWork.BookingRepo.GetAll().LastOrDefault()?.BookingNo + 1 ?? 0;
            return rowCount;
        }
    }
}
