using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumAppJWTAzure.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowedForum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowedPosts",
                columns: table => new
                {
                    ForumId = table.Column<int>(type: "int", nullable: false),
                    FollowerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowedPosts", x => new { x.FollowerId, x.ForumId });
                    table.ForeignKey(
                        name: "FK_FollowedPosts_AspNetUsers_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowedPosts_Forums_ForumId",
                        column: x => x.ForumId,
                        principalTable: "Forums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "30a24107-d279-4e37-96fd-01af5b38cb27",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f281061d-3f44-413e-a00f-204a86f224f8", "AQAAAAIAAYagAAAAEGenVQ5+IEW3wmRlSzRjooIxHzCIFj8q7P5cl1g5AeDTlrDZgos3ogPWlGG6a+y09A==", "82a10273-d089-4dd8-b068-b7f5034a9f2a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e448afa-f008-446e-a52f-13c449803c2e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7cac324e-4a2e-4510-9280-e1a564e26e80", "AQAAAAIAAYagAAAAED7yyD08XgoSBOBn8IJKF/Ko2NNyu39AFhBDbIn4NaBiM/3SZTVBnlBki8CxG+EJgA==", "738aaed7-b433-4f1c-8956-5f7c5a481382" });

            migrationBuilder.CreateIndex(
                name: "IX_FollowedPosts_ForumId",
                table: "FollowedPosts",
                column: "ForumId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowedPosts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "30a24107-d279-4e37-96fd-01af5b38cb27",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "554d2ee7-49a3-4a41-ba62-987dcb27273f", "AQAAAAIAAYagAAAAEJ+uk53vmDkBILLsQlDvX84n7ArRnkyM1sQN4rF62UL7WAb4bfSUWoTl3qPliXlWpg==", "2f52f8c6-50cd-44fe-8ace-470dc2bd2d31" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e448afa-f008-446e-a52f-13c449803c2e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ec266fcd-73f6-413b-8936-66673541481a", "AQAAAAIAAYagAAAAEDtJ2Io+twJJBwoYYXSaujIez2Dg48tmqPIthfGXO7gNORfS6ojCG84vmzzhoUzF5w==", "afde0640-3eb8-4d1d-b841-90e182c82f27" });
        }
    }
}
