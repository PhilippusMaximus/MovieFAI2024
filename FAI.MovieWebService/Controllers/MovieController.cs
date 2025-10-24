using FAI.Core.Application.DTOs.Movies;
using FAI.Core.Application.Services;
using FAI.Core.Repositories.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FAI.MovieWebService.Controllers
{
    [Authorize]
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
            return this.SetLocationUrl<MovieDto>(result, result.Id.ToString());
        }

        [HttpPut(nameof(MovieDto) + ID_PARAMETER_NAME)]
        public async Task<MovieDto> UpdateMovie([FromRoute] Guid Id,
                                                [FromBody] MovieDto movieDto,
                                                CancellationToken cancellationToken)
        {
            // ID immer aus URI dazu verwenden, um die ID im MovieDto Objekt zu überschreiben,
            // falls diese manipuliert wurde
            movieDto.Id = Id;
            return await this.movieService.UpdateMovieDto(movieDto, cancellationToken);
        }

        [HttpDelete(nameof(MovieDto) + ID_PARAMETER_NAME)]
        public async Task DeleteMovieDto([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            await this.movieService.DeleteMovie(Id, cancellationToken);
        }

        #region Private helpers
        protected T SetLocationUrl<T>(T result, string id)
        {
            if(result == null || string.IsNullOrWhiteSpace(id) )
            {
                throw new HttpRequestException("Resource not found.");
            }

            // Aktueller URL ermitteln
            var baseUrl = Request.HttpContext.Request.GetEncodedUrl();

            // Base URL bis zum ersten Querystring-Parameter, kürzen, falls vorhanden
            var length = baseUrl.IndexOf('?') > 0 ? baseUrl.IndexOf('?') : baseUrl.Length;
            var uri = baseUrl.Substring(0, length);

            // So könnten die UrIs aussehen:
            // ..../Movie/MovieDto
            // ..../Movie/MovieDto?param=123 => ..../Movie/MovieDto
            // ..../Movie/MovieDto/

            uri = string.Concat(uri, uri.EndsWith('/') ? string.Empty : "/", id);

            // So schaut es dann aus
            // ..../Movie/MovieDto/ecfcsdf-1234-5678-9876-abcdefabcdef

            HttpContext.Response.Headers.Append("Location", uri);
            HttpContext.Response.StatusCode = StatusCodes.Status201Created;

            return result;
        }



        #endregion

    }
}
