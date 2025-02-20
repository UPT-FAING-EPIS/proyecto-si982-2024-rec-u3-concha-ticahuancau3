using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoCF.Migrations
{
    /// <inheritdoc />
    public partial class AddPreguntasRespuestas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Preguntas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Texto = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EvaluacionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preguntas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preguntas_Evaluaciones_EvaluacionId",
                        column: x => x.EvaluacionId,
                        principalTable: "Evaluaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Respuestas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Texto = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsCorrecta = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PreguntaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuestas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Respuestas_Preguntas_PreguntaId",
                        column: x => x.PreguntaId,
                        principalTable: "Preguntas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_EvaluacionId",
                table: "Preguntas",
                column: "EvaluacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_PreguntaId",
                table: "Respuestas",
                column: "PreguntaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Respuestas");

            migrationBuilder.DropTable(
                name: "Preguntas");
        }
    }
}
