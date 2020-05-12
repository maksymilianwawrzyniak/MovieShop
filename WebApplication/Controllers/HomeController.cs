using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Database;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Connection _databaseConnection;

        public HomeController(ILogger<HomeController> logger, Connection connection)
        {
            _logger = logger;
            _databaseConnection = connection;
        }

        public IActionResult Index()
        {
            Task.Run(async () =>
            {
                var resultSummary = await _databaseConnection.CheckDatabaseConnection();
            });
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}