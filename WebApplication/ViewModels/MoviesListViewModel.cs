using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class MoviesListViewModel
    {
        public MoviesListViewModel()
        {
        }

        public MoviesListViewModel(IEnumerable<Movie> movies) : this()
        {
            Movies = movies;
        }
        
        public IEnumerable<Movie> Movies { get; }

    }
}