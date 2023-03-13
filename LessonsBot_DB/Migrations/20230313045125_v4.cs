using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LessonsBot_DB.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "GroupsCache",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherCaches",
                table: "TeacherCaches",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupsCache",
                table: "GroupsCache",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherCaches",
                table: "TeacherCaches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupsCache",
                table: "GroupsCache");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "GroupsCache",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }
    }
}
