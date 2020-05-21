using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}