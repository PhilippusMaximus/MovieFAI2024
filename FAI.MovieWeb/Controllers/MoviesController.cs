using FAI.Application.Services;
using FAI.Core.Application.DTOs.Movies;
using FAI.Core.Application.Services;
using FAI.Core.Entities.Movies;
using FAI.MovieWeb.Models;
using FAI.Persistence.Repositories.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FAI.MovieWeb.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieDbContext _context;
        private readonly IMovieService movieService;

        public MoviesController(MovieDbContext context, IMovieService movieService)
        {
            _context = context;
            this.movieService = movieService;
        }



        // GET: Movies
        public async Task<IActionResult> Index([FromQuery] string? searchText,
                                               int? genreId,
                                               string? mediumTypeCode,
                                               int take = 10,
                                               int skip = 0,
                                               CancellationToken cancellationToken = default)
        {
            var movieDtos = await movieService.GetMovieDtos(searchText: searchText,
                                                            genreId: genreId,
                                                            mediumTypeCd: mediumTypeCode,
                                                            take: take,
                                                            skip: skip,
                                                            cancellationToken);
           
            return View(movieDtos);
        }

        // GET: Movies/Details
        public async Task<IActionResult> Details([FromRoute]Guid id, CancellationToken cancellation)
        {
            var movieDto = await this.movieService.GetMovieDtoById(id, cancellation);

            if (movieDto == null)
            {
                return NotFound();
            }

            return View(movieDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var movieModel = new MovieEditModel();

            var movieDto = await this.movieService.CreateMovieDto(cancellationToken);
            movieModel.MovieDto = movieDto;

            await this.InitMovieEditModel(movieModel, cancellationToken);
            
            /* Version ohne eigenes Model für Genre- und MediumType-Listen
            var genres = await this.movieService.GetGenres(cancellationToken);
            var mediumTypes = await this.movieService.GetMediumTypes(cancellationToken);

            movieModel.Genres = new SelectList(genres, "Id", "Name");
            movieModel.MediumType = new SelectList(genres, "Code", "Name");
            

            ViewData["Genres"] = new SelectList(genres, "Id", "Name");
            ViewData["MediumTypes"] = new SelectList(mediumTypes, "Code", "Name");

            return View(movieDto);
            */

            return View(movieModel);
        }

        private async Task InitMovieEditModel(MovieEditModel movieModel, CancellationToken cancellationToken)
        {
            var genres = await this.movieService.GetGenres(cancellationToken);
            var mediumTypes = await this.movieService.GetMediumTypes(cancellationToken);

            movieModel.Genres = new SelectList(genres, "Id", "Name");
            movieModel.MediumTypes = new SelectList(mediumTypes, "Code", "Name");
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Price,ReleaseDate,GenreId,MediumTypeCode")] Movie movie)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        movie.Id = Guid.NewGuid();
        //        _context.Add(movie);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
        //    ViewData["MediumTypeCode"] = new SelectList(_context.MediumTypes, "Code", "Code", movie.MediumTypeCode);
        //    return View(movie);
        //}

        // GET: Movies/Edit
        public async Task<IActionResult> Edit([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var movieDto = await this.movieService.GetMovieDtoById(id, CancellationToken.None);

            if (movieDto == null)
            {
                return NotFound();
            }

            var movieModel = new MovieEditModel();
            movieModel.MovieDto = movieDto;

            await this.InitMovieEditModel(movieModel, cancellationToken);

            return View(movieModel);
        }

        // POST: Movies/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] Guid id, MovieDto movieDto, CancellationToken cancellationToken)
        {
            movieDto.Id = id;

            if (movieDto == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var updMovieDto = await this.movieService.UpdateMovieDto(movieDto, CancellationToken.None);

                }
                catch
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var movieModel = new MovieEditModel();
            movieModel.MovieDto = movieDto;

            await this.InitMovieEditModel(movieModel, cancellationToken);

            return View(movieModel);
        }

        // GET: Movies/Delete
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellation)
        {
            var movieDto = await this.movieService.GetMovieDtoById(id, cancellation);

            if (movieDto == null)
            {
                return NotFound();
            }

            return View(movieDto);
        }

        // POST: Movies/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            await this.movieService.DeleteMovie(id, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        //private bool MovieExists(Guid id)
        //{
        //    return _context.Movies.Any(e => e.Id == id);
        //}
    }
}
