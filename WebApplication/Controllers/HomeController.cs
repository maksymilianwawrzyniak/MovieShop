﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceStack;
using WebApplication.Database;
using WebApplication.Models;
using WebApplication.Utils;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HomeController> _logger;
        private readonly Connection _connection;

        public HomeController(IHttpContextAccessor httpContextAccessor, ILogger<HomeController> logger,
            Connection connection)
        {
            _logger = logger;
            _connection = connection;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(string genre = null)
        {
            var movies = genre.IsNullOrEmpty()
                ? await _connection.GetAllOfLabel<Movie>()
                : await _connection.FindAll<Movie>(("Genre", genre));
            var genres = new HashSet<string>((await _connection.GetAllOfLabel<Movie>()).Select(x => x.Genre));
            var indexPageViewModel = new IndexPageViewModel
                {Movies = movies, Genres = genres, UserViewModel = new UserViewModel()};
            return View(indexPageViewModel);
        }

        public IActionResult RegistrationPage()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.Log(LogLevel.Information, "Privacy page");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier};
            _logger.Log(LogLevel.Error, errorViewModel.ToString());
            return View(errorViewModel);
        }

        [Route("/movie/{id}", Name = "ShowMovie")]
        public async Task<IActionResult> ShowMovie(string id)
        {
            var user = SessionUtil.GetUser(_httpContextAccessor);
            var movie = await _connection.Find<Movie>(("Id", id));
            return RedirectToAction(nameof(Index));
        }
    }
}