using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class MovieViewModel
    {
        public MovieViewModel(Movie movie, int boughtCount)
        {
            Id = movie.Id;
            Title = movie.Title;
            Genre = movie.Genre;
            Published = movie.Published;
            Price = movie.Price;
            ThumbnailPath = movie.ThumbnailPath;
            BoughtCount = boughtCount;
        }
        
        public string Id { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public string Published { get; set; }

        public double Price { get; set; }

        public string ThumbnailPath { get; set; }

        public int BoughtCount { get; set; }
    }
}