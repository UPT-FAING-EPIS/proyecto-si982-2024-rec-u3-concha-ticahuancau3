using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoCF.Migrations
{
    /// <inheritdoc />
    public partial class AddDocenteIdToCursos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocenteId",
                table: "Cursos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocenteId",
                table: "Cursos");
        }
    }
}
