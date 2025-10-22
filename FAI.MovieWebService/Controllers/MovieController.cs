using FAI.Core.Application.DTOs.Movies;
using FAI.Core.Application.Services;
using FAI.Core.Repositories.Movies;
using Microsoft.AspNetCore.Mvc;

namespace FAI.MovieWebService.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class MovieController : Controller
    {
        private IMovieService movieService;
        protected const string ID_PARAMETER_NAME = "/{Id}";

        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpGet(nameof(MovieDto))]
        public async Task<IEnumerable<MovieDto>> GetMovieDtos([FromQuery] string? SearchText,
                                                                          int? GenreId,
                                                                          string? MediumTypeCd,
                                                                          int Take = 10,
                                                                          int Skip = 0,
                                                                          CancellationToken cancellationToken = default)
        {
            return await this.movieService.GetMovieDtos(SearchText, GenreId, MediumTypeCd, Take, Skip, cancellationToken);
        }

        [HttpGet(nameof(MovieDto) + ID_PARAMETER_NAME)]
        public async Task<MovieDto> GetMovieDto([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            return await this.movieService.GetMovieDtoById(Id, cancellationToken);
        }

        [HttpPost(nameof(MovieDto))]
        [ProducesResponseType(typeof(MovieDto), StatusCodes.Status201Created)]
        public async Task<MovieDto> CreateMovieDto(CancellationToken cancellationToken)
        {
            var result = await this.movieService.CreateMovieDto(cancellationToken);
            return result;
        }
    }
}
