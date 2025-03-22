using Microsoft.AspNetCore.Mvc;
using DineReserve.Models;

namespace DineReserve.Controllers
{
    public class ReservationsController : Controller
    {
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
                // Save to database (future)
                return RedirectToAction("Confirmation", reservation);
            }
            return View(reservation);
        }

        public IActionResult Confirmation(Reservation reservation)
        {
            return View(reservation);
        }
    }
}
