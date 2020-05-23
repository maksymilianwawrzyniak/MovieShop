using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class AdminActorsPageModel
    {
        public AdminActorsPageModel() { }

        public AdminActorsPageModel(IEnumerable<Actor> actors) : this()
        {
            Actors = actors;
        }
        
        public IEnumerable<Actor> Actors { get; }
    }
}