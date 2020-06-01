using WebApplication.Models;

namespace WebApplication.Dtos
{
    public class DirectorMovieRelationshipDto
    {
        public Director Director { get; set; }
        
        public Movie Movie { get; set; }
    }
}