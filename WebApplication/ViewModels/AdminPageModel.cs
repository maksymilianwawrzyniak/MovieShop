using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class AdminPageModel
    {
        public AdminPageModel()
        {
            
        }

        public AdminPageModel(IEnumerable<Movie> movies, IEnumerable<User> users) : this()
        {
            Movies = movies;
            Users = users;
        }
        
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}