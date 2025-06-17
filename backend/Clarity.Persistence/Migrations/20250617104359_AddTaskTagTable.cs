using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clarity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskTagTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTagEntity_AppTasks_AppTaskId",
                table: "TaskTagEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTagEntity_Tags_TagId",
                table: "TaskTagEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskTagEntity",
                table: "TaskTagEntity");

            migrationBuilder.RenameTable(
                name: "TaskTagEntity",
                newName: "TaskTags");

            migrationBuilder.RenameIndex(
                name: "IX_TaskTagEntity_TagId",
                table: "TaskTags",
                newName: "IX_TaskTags_TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskTags",
                table: "TaskTags",
                columns: new[] { "AppTaskId", "TagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTags_AppTasks_AppTaskId",
                table: "TaskTags",
                column: "AppTaskId",
                principalTable: "AppTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTags_Tags_TagId",
                table: "TaskTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTags_AppTasks_AppTaskId",
                table: "TaskTags");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskTags_Tags_TagId",
                table: "TaskTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskTags",
                table: "TaskTags");

            migrationBuilder.RenameTable(
                name: "TaskTags",
                newName: "TaskTagEntity");

            migrationBuilder.RenameIndex(
                name: "IX_TaskTags_TagId",
                table: "TaskTagEntity",
                newName: "IX_TaskTagEntity_TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskTagEntity",
                table: "TaskTagEntity",
                columns: new[] { "AppTaskId", "TagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTagEntity_AppTasks_AppTaskId",
                table: "TaskTagEntity",
                column: "AppTaskId",
                principalTable: "AppTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTagEntity_Tags_TagId",
                table: "TaskTagEntity",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
