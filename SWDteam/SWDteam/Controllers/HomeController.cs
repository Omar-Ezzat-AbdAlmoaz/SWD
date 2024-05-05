using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWDteam.Data;
using SWDteam.Models;
using System.Diagnostics;

namespace SWDteam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public HomeController(ILogger<HomeController> logger,AppDbContext context,IWebHostEnvironment environment)
        {
            _logger = logger;
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.categories.ToListAsync());
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}