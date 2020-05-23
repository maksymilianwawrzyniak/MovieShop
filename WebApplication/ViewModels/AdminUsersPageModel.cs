using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class AdminUsersPageModel
    {
        public AdminUsersPageModel() { }

        public AdminUsersPageModel(IEnumerable<User> users) : this()
        {
            Users = users;
        }
        
        public IEnumerable<User> Users { get; }
    }
}