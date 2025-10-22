using FAI.Core.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Application.DTOs.Movies
{
    public class MovieDto : MovieBase
    {
        public virtual string GenreName { get; set; } = string.Empty;

        public virtual string? MediumTypeName { get; set; }

        public static MovieDto? MapFrom(Movie movie)
        {
            if (movie != null)
            {
                // Mapping der Eigenschaften von Movie zu MovieDto
                return new MovieDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Price = movie.Price,
                    ReleaseDate = movie.ReleaseDate,
                    GenreId = movie.GenreId,
                    MediumTypeCode = movie.MediumTypeCode,
                    // Null-conditional Operator ? um zu prüfen, ob Genre null ist
                    GenreName = movie.Genre?.Name ?? string.Empty,
                    // Null-conditional Operator ? um zu prüfen, ob MediumType null ist
                    MediumTypeName = movie.MediumType?.Name
                };
            }

            // Rückgabe von null, wenn movie null ist
            return null;
        }

    }
}
