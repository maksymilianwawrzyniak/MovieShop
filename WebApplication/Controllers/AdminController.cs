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

        #region Pages

        public async Task<IActionResult> MoviesPage()
        {
            _logger.Log(LogLevel.Information, "Movies page");
            var movies = await _connection.GetAllOfLabel<Movie>();
            return View(new AdminMoviesPageModel(movies));
        }
        
        public async Task<IActionResult> DirectorsPage()
        {
            _logger.Log(LogLevel.Information, "Directors page");
            var directors = await _connection.GetAllOfLabel<Director>();
            return View(new AdminDirectorsPageModel(directors));
        }
        
        public async Task<IActionResult> ActorsPage()
        {
            _logger.Log(LogLevel.Information, "Actors page");
            var actors = await _connection.GetAllOfLabel<Actor>();
            return View(new AdminActorsPageModel(actors));
        }
        
        public async Task<IActionResult> UsersPage()
        {
            _logger.Log(LogLevel.Information, "Users page");
            var users = await _connection.GetAllOfLabel<User>();
            return View(new AdminUsersPageModel(users));
        }

        public IActionResult AddMoviePage()
        {
            return View();
        }
        public async Task<IActionResult> EditMoviePage(string id)
        {
            var movie = await _connection.Find<Movie>(("Id", id));
            return View(movie);
        }
        
        public IActionResult AddDirectorPage()
        {
            return View();
        }
        public async Task<IActionResult> EditDirectorPage(string id)
        {
            var director = await _connection.Find<Director>(("Id", id));
            return View(director);
        }
        
        public IActionResult AddActorPage()
        {
            return View();
        }
        public async Task<IActionResult> EditActorPage(string id)
        {
            var actor = await _connection.Find<Actor>(("Id", id));
            return View(actor);
        }

        #endregion

        #region Operations

        [Route("admin/movie/add", Name = "AddMovie")]
        public async Task<IActionResult> AddMovie(Movie movie)
        {
            await _connection.Create(movie);
            return RedirectToAction(nameof(MoviesPage));
        }

        [Route("admin/movie/edit", Name = "EditMovie")]
        public async Task<IActionResult> EditMovie(Movie movie)
        {
            await _connection.Update(movie.Id, movie);
            return RedirectToAction(nameof(MoviesPage));
        }

        [Route("admin/movie/remove", Name = "RemoveMovie")]
        public async Task<IActionResult> RemoveMovie(string id)
        {
            var movie = await _connection.Find<Movie>(("Id", id));
            await _connection.Delete(movie);
            return RedirectToAction(nameof(MoviesPage));
        }
        
        [Route("admin/director/add", Name = "AddDirector")]
        public async Task<IActionResult> AddDirector(Director director)
        {
            await _connection.Create(director);
            return RedirectToAction(nameof(DirectorsPage));
        }
        
        [Route("admin/director/edit", Name = "EditDirector")]
        public async Task<IActionResult> EditDirector(Director director)
        {
            await _connection.Update(director.Id, director);
            return RedirectToAction(nameof(DirectorsPage));
        }
        
        [Route("admin/director/remove", Name = "RemoveDirector")]
        public async Task<IActionResult> RemoveDirector(string id)
        {
            var director = await _connection.Find<Director>(("Id", id));
            await _connection.Delete(director);
            return RedirectToAction(nameof(DirectorsPage));
        }

        [Route("admin/actor/add", Name = "AddActor")]
        public async Task<IActionResult> AddActor(Actor actor)
        {
            await _connection.Create(actor);
            return RedirectToAction(nameof(ActorsPage));
        }
        
        [Route("admin/actor/edit", Name = "EditActor")]
        public async Task<IActionResult> EditActor(Actor actor)
        {
            await _connection.Update(actor.Id, actor);
            return RedirectToAction(nameof(ActorsPage));
        }
        
        [Route("admin/actor/remove", Name = "RemoveActor")]
        public async Task<IActionResult> RemoveActor(string id)
        {
            var actor = await _connection.Find<Actor>(("Id", id));
            await _connection.Delete(actor);
            return RedirectToAction(nameof(ActorsPage));
        }
        
        [Route("admin/user/remove", Name = "RemoveUser")]
        public async Task<IActionResult> RemoveUser(string id)
        {
            var user = await _connection.Find<User>(("Id", id));
            await _connection.Delete(user);
            return RedirectToAction(nameof(UsersPage));
        }
        #endregion
    }
}