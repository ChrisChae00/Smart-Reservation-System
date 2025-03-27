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
            return View();
        }

        [HttpPost]
        public IActionResult Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Reservations.Add(reservation);
                _context.SaveChanges();

                return RedirectToAction("Confirmation", new { id = reservation.Id });
            }
            return View(reservation);
        }

        public IActionResult Confirmation(int id)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null)
                return NotFound();

            return View(reservation);
        }
    }
}
