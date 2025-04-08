using Microsoft.AspNetCore.Mvc;
using DineReserve.Data;
using DineReserve.Models;
using Microsoft.EntityFrameworkCore;

namespace DineReserve.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new Reservation();

            // Auto-fill if user is logged in
            if (HttpContext.Session.GetString("Email") != null)
            {
                model.FirstName = HttpContext.Session.GetString("FirstName");
                model.LastName = HttpContext.Session.GetString("LastName");
                model.Email = HttpContext.Session.GetString("Email");
            }

            ViewBag.BlockedTimes = _context.Reservations
                .Where(r => r.ReservationTime.Date == DateTime.Today &&
                            (r.Status == "Pending" || r.Status == "Confirmed"))
                .Select(r => r.ReservationTime.ToString("HH:mm"))
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return View("Thankyou", reservation);
            }

            return View(reservation);
        }

        [HttpGet]
        public IActionResult GetBlockedTimes(string date)
        {
            if (!DateTime.TryParse(date, out var selectedDate))
                return BadRequest("Invalid date format.");

            var blockedTimes = _context.Reservations
                .Where(r => r.ReservationTime.Date == selectedDate.Date &&
                            (r.Status == "Pending" || r.Status == "Confirmed"))
                .Select(r => r.ReservationTime.ToString("HH:mm"))
                .ToList();

            return Json(blockedTimes);
        }

    }
}
