using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Database;
using WebApplication.Models;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Connection _connection;

        public AdminController(ILogger<HomeController> logger, Connection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        public async Task<IActionResult> Index()
        {
            _logger.Log(LogLevel.Information, "Home page");
            var movies = await _connection.GetAllOfLabel<Movie>();
            var users = await _connection.GetAllOfLabel<User>();
            return View(new AdminPageModel(movies, users));
        }

        public IActionResult AddMoviePage()
        {
            return View();
        }

        public async Task<IActionResult> EditMoviePage(string id)
        {
            var movie = await _connection.Find<Movie>(new Tuple<string, string>("Id", id));
            return View(movie);
        }

        [Route("admin/movie/add", Name = "AddMovie")]
        public async Task<IActionResult> AddMovie(Movie movie)
        {
            await _connection.Create(movie);
            return RedirectToAction(nameof(Index));
        }

        [Route("admin/movie/edit", Name = "EditMovie")]
        public async Task<IActionResult> EditMovie(Movie movie)
        {
            await _connection.Update(movie.Id, movie);
            return RedirectToAction(nameof(Index));
        }

        [Route("admin/movie/remove", Name = "RemoveMovie")]
        public async Task<IActionResult> RemoveMovie(Movie movie)
        {
            await _connection.Delete(movie);
            return RedirectToAction(nameof(Index));
        }
    }
}