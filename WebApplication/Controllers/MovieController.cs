using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Views
{
    public class MovieController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}