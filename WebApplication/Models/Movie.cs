namespace WebApplication.Models
{
    public class Movie : BaseModel
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Genre { get; set; }

        public string Published { get; set; }
        
        public double Price { get; set; }
        
        public string ThumbnailPath { get; set; }
    }
}