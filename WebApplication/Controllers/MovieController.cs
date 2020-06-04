using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Database;
using WebApplication.Models;
using WebApplication.Utils;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class MovieController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<MovieController> _logger;
        private readonly Connection _connection;

        public MovieController(IHttpContextAccessor httpContextAccessor, ILogger<MovieController> logger,
            Connection connection)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _connection = connection;
        }

        [Route("/buy", Name = "BuyMovie")]
        public async Task<IActionResult> BuyMovie(string id)
        {
            var user = SessionUtil.GetUser(_httpContextAccessor);
            var movie = await _connection.Find<Movie>((Constants.Id, id));
            var relationships = await _connection.GetRelationshipsBetween(user, movie, Constants.BoughtLabel);
            if (relationships == null || !relationships.GetEnumerator().MoveNext())
                await _connection.CreateDirectedRelationship(user, movie, Constants.BoughtLabel);
            else
                _logger.Log(LogLevel.Information, $"User {user.Email} already has bought movie {movie.Title}!");

            return Redirect("/");
        }
        
        [Route("/movie/{id}", Name = "ShowMovie")]
        public async Task<IActionResult> ShowMovie(string id)
        {
            var movie = await _connection.Find<Movie>((Constants.Id, id));
            var actors = await _connection.FindAllByRelationship<Movie, Actor>(movie, Constants.StarredLabel);
            var director = (await _connection.FindAllByRelationship<Movie, Director>(movie, Constants.DirectedLabel))
                .FirstOrDefault();
            var movieViewModel = new MovieViewModel(movie, actors, director);
            return View(movieViewModel);
        }

    }
}