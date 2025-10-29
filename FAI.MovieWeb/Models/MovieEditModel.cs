using FAI.Core.Application.DTOs.Movies;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FAI.MovieWeb.Models
{
    public class MovieEditModel
    {
        public MovieDto MovieDto { get; set; }
        public SelectList Genres { get; set; }
        public SelectList MediumTypes { get; set; }
    }
}
