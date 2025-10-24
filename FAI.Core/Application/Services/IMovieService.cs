using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAI.Core.Application.DTOs.Movies;
using FAI.Core.Entities.Movies;

namespace FAI.Core.Application.Services
{
    public interface IMovieService
    {
        #region READ Methods
        // get a list of movies with optional filtering and pagination
        // IEnumerable ist eine Schnittstelle für Auflistungen, dass eine Sammlung von Objekten durchlaufen werden kann
        Task<IEnumerable<MovieDto>> GetMovieDtos(string? searchText,
                                                 int? genreId,
                                                 string? mediumTypeCd,
                                                 int take = 10,
                                                 int skip = 0,
                                                 CancellationToken cancellationToken = default);

        // get a single movie by its unique identifier
        Task<MovieDto> GetMovieDtoById(Guid id, CancellationToken cancellationToken = default);

        // get all available genres
        Task<IEnumerable<Genre>> GetGenres(CancellationToken cancellationToken = default);

        // get all available medium types
        Task<IEnumerable<MediumType>> GetMediumTypes(CancellationToken cancellationToken = default);
        #endregion

        #region COMMAND Methods (CUD)
        // create a new movie entry
        Task<MovieDto> CreateMovieDto(CancellationToken cancellationToken);
        
        // update an existing movie entry
        Task<MovieDto> UpdateMovieDto(MovieDto movieDto, CancellationToken cancellationToken);
        
        // delete a movie entry by its unique identifier
        Task DeleteMovie(Guid id, CancellationToken cancellationToken);
        #endregion
    }
}
