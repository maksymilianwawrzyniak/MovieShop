using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class AdminDirectorsPageModel
    {
        public AdminDirectorsPageModel() { }

        public AdminDirectorsPageModel(IEnumerable<Director> directors) : this()
        {
            Directors = directors;
        }
        
        public IEnumerable<Director> Directors { get; }
    }
}