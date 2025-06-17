using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clarity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTaskTagTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskTags");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_AppTaskId",
                table: "Tags",
                column: "AppTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AppTasks_AppTaskId",
                table: "Tags",
                column: "AppTaskId",
                principalTable: "AppTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AppTasks_AppTaskId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_AppTaskId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "TaskTags",
                columns: table => new
                {
                    AppTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTags", x => new { x.AppTaskId, x.TagId });
                    table.ForeignKey(
                        name: "FK_TaskTags_AppTasks_AppTaskId",
                        column: x => x.AppTaskId,
                        principalTable: "AppTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskTags_TagId",
                table: "TaskTags",
                column: "TagId");
        }
    }
}
