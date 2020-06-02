using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class IndexPageViewModel
    {
        public IndexPageViewModel()
        {
            
        }

        public IndexPageViewModel(IEnumerable<MovieViewModel> movies, IEnumerable<string> genres)
        {
            Movies = movies;
            UserViewModel = new UserViewModel();
            Genres = genres;
        }
        
        public IndexPageViewModel(IEnumerable<MovieViewModel> movies, UserViewModel userViewModel, IEnumerable<string> genres)
        {
            Movies = movies;
            UserViewModel = userViewModel;
            Genres = genres;
        }
        
        public IEnumerable<MovieViewModel> Movies { get; set; }
        
        public UserViewModel UserViewModel { get; set; }
        
        public IEnumerable<string> Genres { get; set; }
    }
}