using FAI.Core.Entities.Movies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Persistence.Repositories.DBContext
{
    public class MovieDbContext : DbContext
    {
        // Die Core-Entitäten definieren die Beziehungen (nur) im Code,
        // der DbContext legt fest, wie diese Beziehungen in der Datenbank aussehen und funktionieren.
        public MovieDbContext() { }
        // Konstruktor mit DbContextOptions, um Konfigurationsoptionen zu übergeben
        // damit der Datenbankkontext richtig initialisiert wird 
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) 
        { 
            Database.SetCommandTimeout(90);
        }
       
        // Stellt die Verbindung zu den Entitäten und den Tabelllen in der Datenbank her
        // (ermöglicht CRUD-Operationen Create/Read/Update/Delete)
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<MediumType> MediumTypes { get; set; }

        // hier wird das Datenbank-Modell konfiguriert, um die Entitäten und deren Beziehungen zu definieren
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable(nameof(Movie)); /* Aktuell keinen Sinn, da der Tabellenname 
                                                * dem Klassennamen entspricht */
                entity.Property(p => p.Title)
                      .IsRequired()
                      .HasMaxLength(128);

                /* Index für Title und Fremdschlüsselfelder, damit die Suche schneller geht. 
                   Es muss nicht alles durchsucht werden, sondern ähnlich einem Wörterbuch */
                // p => p.Title ist ein Lambda-Ausdruck
                entity.HasIndex(p => p.Title)
                      .IsUnique(false)
                      .HasDatabaseName("IX_" + nameof(Movie) + "_" +nameof(Movie.Title));

                entity.HasIndex(p => p.GenreId)
                      .IsUnique(false)
                      .HasDatabaseName("IX_" + nameof(Movie) + "_" + nameof(Movie.GenreId));

                entity.HasIndex(p => p.MediumTypeCode)
                      .IsUnique(false)
                      .HasDatabaseName("IX_" + nameof(Movie) + "_" + nameof(Movie.MediumTypeCode));

                entity.Property(p => p.Price)
                      .HasPrecision(19, 2)
                      .HasDefaultValue(0M);

                entity.Property(p => p.ReleaseDate)
                      .HasColumnType("date");
            });

            // 1: n Beziehung zwischen Movie und MediumType
            // Die Beziehnung muss nur auf einer Seite konfiguriert werden
            modelBuilder.Entity<Movie>().HasOne(m => m.MediumType)
                                        .WithMany(m => m.Movies)
                                        .HasForeignKey(m => m.MediumTypeCode)
                                        .OnDelete(DeleteBehavior.SetNull);

            // Primärschlüssel für MediumType ist das Code-Attribut
            modelBuilder.Entity<MediumType>(entity =>
            {
                entity.HasKey(e => e.Code);
            });

            // 1: n Beziehung zwischen Genre und Movie
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasMany(g => g.Movies)
                      .WithOne(m => m.Genre)
                      .HasForeignKey(m => m.GenreId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seeder um die Datenbank mit Default-Daten zu befüllen
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Comedy" },
                new Genre { Id = 3, Name = "Drama" },
                new Genre { Id = 4, Name = "Horror" },
                new Genre { Id = 5, Name = "Science Fiction" }
            );

            modelBuilder.Entity<MediumType>().HasData(
                new MediumType { Code = "DVD", Name = "Digital Versatile Disc" },
                new MediumType { Code = "BD", Name = "Blu-Ray Disc" },
                new MediumType { Code = "4K", Name = "4K Ultra HD Blu Ray" },
                new MediumType { Code = "DIGI", Name = "Digital Copy" },
                new MediumType { Code = "VHS", Name = "Video Home System" },
                new MediumType { Code = "STR", Name = "Streaming" }
            );

            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = new Guid("32c4be85-e8b5-4d9e-a7c8-2ac1c57e3d31"),
                    Title = "Inception",
                    ReleaseDate = new DateTime(2001, 1, 2),
                    GenreId = 3,
                    MediumTypeCode = "DVD",
                    Price = 19.90M
                },
                new Movie
                {
                    Id = new Guid("f5a8fc25-0b6e-405f-983e-a7a9da20f045"),
                    Title = "The Shining",
                    ReleaseDate = new DateTime(1998, 4, 24),
                    GenreId = 4,
                    MediumTypeCode = "VHS",
                    Price = 12.45M
                },
                new Movie
                {
                  Id = new Guid("ce19a45d-5016-49bc-8ef0-572b6f4447af"),
                  Title = "The Hangover",
                  ReleaseDate = new DateTime(2022, 8, 4),
                  GenreId = 2,
                  MediumTypeCode = "STR",
                  Price = 24.90M
                },
                new Movie
                {
                    Id = new Guid("d3fdd970-8909-40f3-afe2-4c10439b810f"),
                    Title = "Get Out",
                    ReleaseDate = new DateTime(2018, 3, 12),
                    GenreId = 1,
                    MediumTypeCode = "BD",
                    Price = 21.33M
                },
                new Movie
                {
                    Id = new Guid("16ecd956-b1e5-466e-9a02-4521390ae1f4"),
                    Title = "Zurück in die Zukunft",
                    ReleaseDate = new DateTime(1984, 12, 19),
                    GenreId = 5,
                    MediumTypeCode = "VHS",
                    Price = 18.79M
                }
            );
        }

        // Nuget installieren: Microsoft.EntityFrameworkCore.Tool
        // Nuget installieren: Microsoft.Extensions.configuration.Json
        // Verbindung zur Datenbank herstellen, Konfigurationsdatei (appsettings.json) auslesen
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

#if DEBUG
            // Verzeichnis ermitteln
            if(currentDirectory.IndexOf("bin") > -1)
            {
                // Verzeichnis bis zum bin-Ordner extrahieren
                currentDirectory = currentDirectory.Substring(0, currentDirectory.IndexOf("bin"));
            }
#endif  
            // Konfigurations-Builder erstellen, der die appsettings.json einliest
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(currentDirectory)
                                                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Konfiguration erstellen
            var configuration = configurationBuilder.Build();
            // ConnectionString aus der Konfiguration auslesen
            var connectionString = configuration.GetConnectionString("MovieDbContextProduction");
            // Verbindung zur SQL-Server-Datenbank herstellen
            optionsBuilder.UseSqlServer(connectionString, opt => opt.CommandTimeout(60));
        }

        /* Command für Initialisierung der EF-Migration 
            * 1. PM Console öffnen (unter Tools -> Nuget Package Manager)
            * 2. add-migration Initial -startupProject FAI.Persistence
            * 3. update-database -startupProject FAI.Persistence
            */
    }
}
