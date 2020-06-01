using WebApplication.Models;

namespace WebApplication.Dtos
{
    public class ActorMovieRelationshipDto
    {
        public Actor Actor { get; set; }
        
        public Movie Movie { get; set; }
    }
}