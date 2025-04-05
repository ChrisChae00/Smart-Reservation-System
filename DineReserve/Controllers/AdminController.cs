using Microsoft.AspNetCore.Mvc;
using DineReserve.Data;
using DineReserve.Models;
using Microsoft.EntityFrameworkCore;

namespace DineReserve.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            // Only allow access if logged in as admin
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Home");

            var reservations = await _context.Reservations
                .OrderBy(r => r.ReservationTime)
                .ToListAsync();

            return View(reservations);
        }

        // POST: /Admin/UpdateStatus
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                reservation.Status = status;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Dashboard");
        }
    }
}
