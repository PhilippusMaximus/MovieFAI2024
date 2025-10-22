using FAI.Core.Application.DTOs.Movies;
using FAI.Core.Application.Services;
using FAI.Core.Entities.Movies;
using FAI.Core.Repositories.Movies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAI.Common;
using FAI.Core.Attributes;

namespace FAI.Application.Services
{
    [MapServiceDependency(nameof(MovieService))]
    public class MovieService : IMovieService
    {
        // Repositories für den Zugriff auf die Datenbank
        private readonly IMovieRepository movieRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IMediumTypeRepository mediumTypeRepository;

        // Konstruktor mit Dependency Injection der Repositories
        public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository, IMediumTypeRepository mediumTypeRepository)
        {
            this.movieRepository = movieRepository;
            this.genreRepository = genreRepository;
            this.mediumTypeRepository = mediumTypeRepository;
        }   

        #region READ Methods

        public async Task<IEnumerable<MovieDto>> GetMovieDtos(string? searchText,
                                                              int? genreId,
                                                              string? mediumTypeCd,
                                                              int take = 10,
                                                              int skip = 0, CancellationToken cancellationToken = default)
        {
            // Repositories liegen auf dem selben DBContext, daher kann Include verwendet werden
            var query = this.movieRepository.QueryFrom<Movie>()
                                            // Lädt die zugehörige Genre-Entität mit
                                            .Include(i => i.Genre)
                                            // Lädt die zugehörige MediumType-Entität mit
                                            .Include(i => i.MediumType).AsQueryable();
                                            

            // Filter für Suchtext
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(w => w.Title.Contains(searchText));
            }
            // Filter für GenreId
            if (genreId.HasValue)
            {
                query = query.Where(w => w.GenreId == genreId.Value);
            }
            // Filter für MediumTypeCode
            if (!string.IsNullOrWhiteSpace(mediumTypeCd))
            {
                query = query.Where(w => w.MediumTypeCode == mediumTypeCd);
            }

            // Filter sind UND-Verknüpft, sodass nur die Datensätze zurückgegeben werden, die alle Filterkriterien erfüllen
            // Rückgabe der gefilterten und paginierten Liste

            return await query.Skip(skip)                   // Pagination: überspringe die ersten 'skip' Einträge
                        .Take(take)                         // Pagination: nehme die nächsten 'take' Einträge 
                        .Select(s => MovieDto.MapFrom(s))  // Mappt die Movie-Entität zu MovieDto 
                        .ToListAsync(cancellationToken);    // Asynchrone Ausführung der Abfrage und Rückgabe der Liste
        }

        public async Task<MovieDto> GetMovieDtoById(Guid id, CancellationToken cancellationToken = default)
        {
            var query = this.movieRepository.QueryFrom<Movie>(m => m.Id == id) 
                                            .Select(s => MovieDto.MapFrom(s));

            // Rückgabe des einzelnen MovieDto oder null, wenn nicht gefunden
            // SingleOrDefaultAsync gibt genau einen Eintrag zurück oder null, wenn kein Eintrag gefunden wurde
            // Würden zwei oder mehr Einträge gefunden, würden eine Ausnahme ausgelöst
            // Weil wenn nach Primärschlüssel gesucht wird, sollte immer nur ein Eintrag zurückgegeben werden
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Genre>> GetGenres(CancellationToken cancellationToken = default)
        {
            return await this.genreRepository.QueryFrom<Genre>() 
                                             .AsNoTracking()
                                             .OrderBy(o => o.Name) 
                                             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MediumType>> GetMediumTypes(CancellationToken cancellationToken = default)
        {
            // AsNoTracking, da die MediumTypes nur gelesen und nicht verändert werden
             
            return await this.mediumTypeRepository.QueryFrom<MediumType>()
                                            .AsNoTracking()
                                            .OrderBy(o => o.Name) 
                                            .ToListAsync(cancellationToken)
                                            // Umwandlung der Task<List<MediumType>> in Task<IEnumerable<MediumType>>
                                            // Dies ist notwendig, da die Methode IEnumerable<MediumType> zurückgeben soll
                                            .ContinueWith(t => (IEnumerable<MediumType>)t.Result, cancellationToken);
        }
        #endregion

        #region COMMAND Methods (Create, Update, Delete)

        public async Task<MovieDto> CreateMovieDto(CancellationToken cancellationToken = default)
        {
            // Variante Post, Put, Patch

            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "N/A",
                Price = 0M,
                ReleaseDate = DateTime.Now.Date,
                MediumTypeCode = "BD", // Default MediumType "Blu-ray"
            };

            // Variante 1: Neues Movie Dummy Objekt wird in die Db gespeichert
            // SaveImmediately true, damit sofort gespeichert wird. Schreibt den ganzen DbContext in die DB. Mit Bedacht verwenden!
            // Verhindert, dass das Objekt erst später gespeichert wird und zwischenzeitlich ungültige Daten entstehen können
            await this.movieRepository.AddAsync(movie, saveImmediately: true, cancellationToken);

            // Gespeichertes Movie Objekt wird als MovieDto gemappt und zurückgegeben
            return MovieDto.MapFrom(movie);
        }

        public async Task<MovieDto> UpdateMovieDto(MovieDto movieDto, CancellationToken cancellationToken = default)
        {
            // Movie Entität erstellen
            var movie = new Movie();

            // Werte aus dem DTO in die Entität mappen
            Helpers.MapEntityProperties<MovieDto, Movie>(movieDto, movie, null);

            // Entität mit eingefügten Werten speichern. Die Id kann aus movie oder movieDto gelesen werden.
            var updatedMovie = await this.movieRepository.UpdateAsync(movie, movie.Id, saveImmediately: true, cancellationToken);

            // await this.movieRepository.SaveAsync(cancellationToken); <-- Nicht mehr nötig, da saveImmediately true ist

            // Aktualisertes DTO zurückgeben
            return MovieDto.MapFrom(updatedMovie);
        }

        public async Task DeleteMovieAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Movie Objekt anhand der Id löschen
            await this.movieRepository.RemoveByKeyAsync<Movie>(id, saveImmediately: true, cancellationToken);
        }

        #endregion
    }
}
