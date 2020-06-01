using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class IndexPageViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        
        public UserViewModel UserViewModel { get; set; }
        
        public IEnumerable<string> Genres { get; set; }
    }
}