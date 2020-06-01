using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Database;
using WebApplication.Models;
using WebApplication.Utils;

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
            const string boughtLabel = "bought";
            var user = SessionUtil.GetUser(_httpContextAccessor);
            var movie = await _connection.Find<Movie>(("Id", id));
            var relationships = await _connection.GetRelationshipsBetween(user, movie, boughtLabel);
            if (relationships == null || !relationships.GetEnumerator().MoveNext())
                await _connection.CreateDirectedRelationship(user, movie, boughtLabel);
            else
                _logger.Log(LogLevel.Information, $"User {user.Email} already has bought movie {movie.Title}!");

            return Redirect("/");
        }
    }
}