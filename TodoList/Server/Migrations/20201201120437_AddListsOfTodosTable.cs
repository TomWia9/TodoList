using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoList.Server.Migrations
{
    public partial class AddListsOfTodosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListOfTodosId",
                table: "Todos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ListsOfTodos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListsOfTodos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_ListOfTodosId",
                table: "Todos",
                column: "ListOfTodosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_ListsOfTodos_ListOfTodosId",
                table: "Todos",
                column: "ListOfTodosId",
                principalTable: "ListsOfTodos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_ListsOfTodos_ListOfTodosId",
                table: "Todos");

            migrationBuilder.DropTable(
                name: "ListsOfTodos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_ListOfTodosId",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "ListOfTodosId",
                table: "Todos");
        }
    }
}
