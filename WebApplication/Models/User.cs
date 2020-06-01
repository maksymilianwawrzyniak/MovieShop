using WebApplication.ViewModels;

namespace WebApplication.Models
{
    public class User : BaseModel
    {
        public User()
        {
            
        }

        public User(UserViewModel userViewModel)
        {
            Email = userViewModel.Email;
            Username = userViewModel.Email;
            Password = userViewModel.Password;
        }
        
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        public string UserType { get; set; }
    }
}