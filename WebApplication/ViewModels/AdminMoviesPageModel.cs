using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class AdminMoviesPageModel
    {
        public AdminMoviesPageModel() { }

        public AdminMoviesPageModel(IEnumerable<Movie> movies) : this()
        {
            Movies = movies;
        }
        
        public IEnumerable<Movie> Movies { get; }
    }
}