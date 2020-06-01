using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class IndexPageViewModel
    {
        public IndexPageViewModel()
        {
            
        }

        public IndexPageViewModel(IEnumerable<Movie> movies, IEnumerable<string> genres)
        {
            Movies = movies;
            UserViewModel = new UserViewModel();
            Genres = genres;
        }
        
        public IndexPageViewModel(IEnumerable<Movie> movies, UserViewModel userViewModel, IEnumerable<string> genres)
        {
            Movies = movies;
            UserViewModel = userViewModel;
            Genres = genres;
        }
        
        public IEnumerable<Movie> Movies { get; set; }
        
        public UserViewModel UserViewModel { get; set; }
        
        public IEnumerable<string> Genres { get; set; }
    }
}