using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThaLeague.Data;
using ThaLeague.Models;
using ThaLeague.ViewModels;

namespace ThaLeague.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHost;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment web)
        {
            _logger = logger;
            webHost = web;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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