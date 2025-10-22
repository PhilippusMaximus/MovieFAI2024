using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediumTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediumTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false, defaultValue: 0m),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    MediumTypeCode = table.Column<string>(type: "nvarchar(8)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movie_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movie_MediumTypes_MediumTypeCode",
                        column: x => x.MediumTypeCode,
                        principalTable: "MediumTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Comedy" },
                    { 3, "Drama" },
                    { 4, "Horror" },
                    { 5, "Science Fiction" }
                });

            migrationBuilder.InsertData(
                table: "MediumTypes",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { "4K", "4K Ultra HD Blu Ray" },
                    { "BD", "Blu-Ray Disc" },
                    { "DIGI", "Digital Copy" },
                    { "DVD", "Digital Versatile Disc" },
                    { "STR", "Streaming" },
                    { "VHS", "Video Home System" }
                });

            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "Id", "GenreId", "MediumTypeCode", "Price", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("16ecd956-b1e5-466e-9a02-4521390ae1f4"), 5, "VHS", 18.79m, new DateTime(1984, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zurück in die Zukunft" },
                    { new Guid("32c4be85-e8b5-4d9e-a7c8-2ac1c57e3d31"), 3, "DVD", 19.90m, new DateTime(2001, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inception" },
                    { new Guid("ce19a45d-5016-49bc-8ef0-572b6f4447af"), 2, "STR", 24.90m, new DateTime(2022, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Hangover" },
                    { new Guid("d3fdd970-8909-40f3-afe2-4c10439b810f"), 1, "BD", 21.33m, new DateTime(2018, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Get Out" },
                    { new Guid("f5a8fc25-0b6e-405f-983e-a7a9da20f045"), 4, "VHS", 12.45m, new DateTime(1998, 4, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Shining" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movie_GenreId",
                table: "Movie",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_MediumTypeCode",
                table: "Movie",
                column: "MediumTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_Title",
                table: "Movie",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "MediumTypes");
        }
    }
}
