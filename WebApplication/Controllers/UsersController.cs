using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApplication.Database;
using WebApplication.Models;
using WebApplication.Utils;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class UsersController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UsersController> _logger;
        private readonly Connection _connection;

        public ISession Session => _httpContextAccessor.HttpContext.Session;

        public UsersController(IHttpContextAccessor httpContextAccessor, ILogger<UsersController> logger,
            Connection connection)
        {
            _logger = logger;
            _connection = connection;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("registration", Name = "RegisterUser")]
        public async Task<IActionResult> RegisterUser(User user)
        {
            user.UserType = "User";
            var dbUser = await _connection.Find<User>((Constants.Email, user.Email));
            if (dbUser != null)
                return Redirect("/");

            await _connection.Create(user);
            return Redirect("/");
        }

        [Route("login", Name = "LoginUser")]
        public async Task<IActionResult> LoginUser(UserViewModel userViewModel)
        {
            var user = await _connection.Find<User>((Constants.Email, userViewModel.Email));
            if (user == default)
                return Redirect("/");

            if (user.Password != userViewModel.Password)
                return Redirect("/");

            Session.SetString("user", JsonConvert.SerializeObject(user));
            await Session.CommitAsync();
            return Redirect("/");
        }

        [Route("logout", Name = "LogoutUser")]
        public async Task<IActionResult> LogoutUser()
        {
            Session.Clear();
            await Session.CommitAsync();
            return Redirect("/");
        }
    }
}