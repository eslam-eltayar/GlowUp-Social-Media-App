using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Glow_Up.Repositories._Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBlackHatModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BHPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BHPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BHPosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BHComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BHPostId = table.Column<int>(type: "int", nullable: false),
                    VoteCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BHComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BHComments_BHPosts_BHPostId",
                        column: x => x.BHPostId,
                        principalTable: "BHPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BHComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BHLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BHPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BHLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BHLikes_BHPosts_BHPostId",
                        column: x => x.BHPostId,
                        principalTable: "BHPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BHLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BHMedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BHPostId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BHMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BHMedias_BHPosts_BHPostId",
                        column: x => x.BHPostId,
                        principalTable: "BHPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BHComments_BHPostId",
                table: "BHComments",
                column: "BHPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BHComments_UserId",
                table: "BHComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BHLikes_BHPostId",
                table: "BHLikes",
                column: "BHPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BHLikes_UserId",
                table: "BHLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BHMedias_BHPostId",
                table: "BHMedias",
                column: "BHPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BHPosts_UserId",
                table: "BHPosts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BHComments");

            migrationBuilder.DropTable(
                name: "BHLikes");

            migrationBuilder.DropTable(
                name: "BHMedias");

            migrationBuilder.DropTable(
                name: "BHPosts");
        }
    }
}
