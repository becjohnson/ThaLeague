using Microsoft.AspNetCore.Mvc;

namespace ThaLeague.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
