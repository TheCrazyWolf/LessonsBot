using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LessonsBot_DB.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bots",
                columns: table => new
                {
                    IdBot = table.Column<Guid>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    IdValueService = table.Column<long>(type: "INTEGER", nullable: true),
                    TimeOutResponce = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bots", x => x.IdBot);
                });

            migrationBuilder.CreateTable(
                name: "PeerProps",
                columns: table => new
                {
                    IdPeerProp = table.Column<Guid>(type: "TEXT", nullable: false),
                    IdPeer = table.Column<long>(type: "INTEGER", nullable: false),
                    TypeLesson = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    LastResult = table.Column<string>(type: "TEXT", nullable: true),
                    BotIdBot = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeerProps", x => x.IdPeerProp);
                    table.ForeignKey(
                        name: "FK_PeerProps_Bots_BotIdBot",
                        column: x => x.BotIdBot,
                        principalTable: "Bots",
                        principalColumn: "IdBot");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeerProps_BotIdBot",
                table: "PeerProps",
                column: "BotIdBot");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeerProps");

            migrationBuilder.DropTable(
                name: "Bots");
        }
    }
}
