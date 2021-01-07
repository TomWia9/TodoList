using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoList.Server.Migrations
{
    public partial class AddUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ListsOfTodos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListsOfTodos_UserId",
                table: "ListsOfTodos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListsOfTodos_Users_UserId",
                table: "ListsOfTodos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListsOfTodos_Users_UserId",
                table: "ListsOfTodos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ListsOfTodos_UserId",
                table: "ListsOfTodos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ListsOfTodos");
        }
    }
}
