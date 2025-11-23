using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace family_tree_builder.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonNodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fid = table.Column<int>(type: "INTEGER", nullable: true),
                    Mid = table.Column<int>(type: "INTEGER", nullable: true),
                    Pids = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonNodes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonNodes");
        }
    }
}
