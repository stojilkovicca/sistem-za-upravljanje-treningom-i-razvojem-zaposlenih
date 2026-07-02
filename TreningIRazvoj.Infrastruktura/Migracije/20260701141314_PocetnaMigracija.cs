using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TreningIRazvoj.Infrastruktura.Migracije
{
    /// <inheritdoc />
    public partial class PocetnaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KategorijePrograma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategorijePrograma", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Predavaci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Imejl = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    OblastStrucnosti = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Interni = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predavaci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zaposleni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Imejl = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RadnoMesto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Odeljenje = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DatumZaposlenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aktivan = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zaposleni", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RazvojniProgrami",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Vrsta = table.Column<int>(type: "int", nullable: false),
                    DatumPocetka = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumZavrsetka = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kapacitet = table.Column<int>(type: "int", nullable: false),
                    TrajanjeUSatima = table.Column<int>(type: "int", nullable: false),
                    MinimalanBrojPoena = table.Column<int>(type: "int", nullable: true),
                    Objavljen = table.Column<bool>(type: "bit", nullable: false),
                    KategorijaProgramaId = table.Column<int>(type: "int", nullable: false),
                    PredavacId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RazvojniProgrami", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RazvojniProgrami_KategorijePrograma_KategorijaProgramaId",
                        column: x => x.KategorijaProgramaId,
                        principalTable: "KategorijePrograma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RazvojniProgrami_Predavaci_PredavacId",
                        column: x => x.PredavacId,
                        principalTable: "Predavaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prijave",
                columns: table => new
                {
                    ZaposleniId = table.Column<int>(type: "int", nullable: false),
                    RazvojniProgramId = table.Column<int>(type: "int", nullable: false),
                    DatumPrijave = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProcenatPrisustva = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    BrojPoena = table.Column<int>(type: "int", nullable: true),
                    DatumZavrsetka = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OcenaPrograma = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prijave", x => new { x.ZaposleniId, x.RazvojniProgramId });
                    table.ForeignKey(
                        name: "FK_Prijave_RazvojniProgrami_RazvojniProgramId",
                        column: x => x.RazvojniProgramId,
                        principalTable: "RazvojniProgrami",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prijave_Zaposleni_ZaposleniId",
                        column: x => x.ZaposleniId,
                        principalTable: "Zaposleni",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KategorijePrograma_Naziv",
                table: "KategorijePrograma",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Predavaci_Imejl",
                table: "Predavaci",
                column: "Imejl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prijave_RazvojniProgramId",
                table: "Prijave",
                column: "RazvojniProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_RazvojniProgrami_KategorijaProgramaId",
                table: "RazvojniProgrami",
                column: "KategorijaProgramaId");

            migrationBuilder.CreateIndex(
                name: "IX_RazvojniProgrami_PredavacId",
                table: "RazvojniProgrami",
                column: "PredavacId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_Imejl",
                table: "Zaposleni",
                column: "Imejl",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prijave");

            migrationBuilder.DropTable(
                name: "RazvojniProgrami");

            migrationBuilder.DropTable(
                name: "Zaposleni");

            migrationBuilder.DropTable(
                name: "KategorijePrograma");

            migrationBuilder.DropTable(
                name: "Predavaci");
        }
    }
}
