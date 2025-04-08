using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DineReserve.Models;
using DineReserve.Data;
using Microsoft.EntityFrameworkCore;

namespace DineReserve.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    // GET: Home/Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // POST: Home/Login
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        // Look for a user in the database
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

        if (user != null)
        {
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("LastName", user.LastName);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Role", user.Role);


            // Redirect based on role
            if (user.Role == "Admin")
                return RedirectToAction("Dashboard", "Admin");

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Invalid credentials";
        return View();
    }




    // GET: Home/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: Home/Register
    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        if (!ModelState.IsValid)
            return View(user);

        user.Role = "Customer";
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
        HttpContext.Session.SetString("FirstName", user.FirstName);
        HttpContext.Session.SetString("LastName", user.LastName);
        HttpContext.Session.SetString("Email", user.Email);
        HttpContext.Session.SetString("Role", user.Role);


        return RedirectToAction("Index");
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
