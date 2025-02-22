using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Glow_Up.Repositories._Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReplyTable010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Posts_PostId1",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_PostId1",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "Reactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId1",
                table: "Reactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_PostId1",
                table: "Reactions",
                column: "PostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Posts_PostId1",
                table: "Reactions",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
