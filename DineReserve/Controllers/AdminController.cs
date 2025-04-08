using Microsoft.AspNetCore.Mvc;
using DineReserve.Data;
using DineReserve.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

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
        public async Task<IActionResult> Dashboard(DateTime? selectedDate)
        {
            var date = selectedDate ?? DateTime.Today;

            var reservations = await _context.Reservations
                .Where(r => r.ReservationTime.Date == date.Date)
                .ToListAsync();

            // Update the counts
            var totalReservations = reservations.Count(r => r.Status != "Rejected");
            var checkedInCount = reservations.Count(r => r.Status == "Checked-in");
            var pendingCount = reservations.Count(r => r.Status == "Pending");
            var cancelledCount = reservations.Count(r => r.Status == "Canceled");

            // Pass counts and selected date to view
            ViewBag.TotalReservations = totalReservations;
            ViewBag.CheckedInCount = checkedInCount;
            ViewBag.PendingCount = pendingCount;
            ViewBag.CancelledCount = cancelledCount;
            ViewBag.SelectedDate = date;

            return View(reservations);
        }



        // POST: /Admin/UpdateStatus
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string actionType)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            switch (actionType)
            {
                case "Confirm":
                    reservation.Status = "Confirmed";
                    await SendConfirmationEmail(reservation.Email, reservation.FirstName, reservation.ReservationTime);
                    break;
                case "Reject":
                    reservation.Status = "Rejected";
                    break;
                case "CheckIn":
                    reservation.Status = "Checked-in";
                    break;
                case "Cancel":
                    reservation.Status = "Canceled";
                    break;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }


        private async Task SendConfirmationEmail(string toEmail, string firstName, DateTime time)
        {
            var subject = "Your Reservation is Confirmed!";
            var body = $"Hello {firstName},\n\nYour reservation on {time:dddd, MMMM d @ h:mm tt} has been confirmed!\n\nWe look forward to serving you.\n\n– DineReserve";

            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("test11010001101001@gmail.com", "fnkd qfnq xxrj nnvf"); // app password here

                var mail = new MailMessage
                {
                    From = new MailAddress("test11010001101001@gmail.com", "DineReserve"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false // true if you're sending HTML
                };

                mail.To.Add(toEmail);

                await client.SendMailAsync(mail);
            }
        }


    }
}
