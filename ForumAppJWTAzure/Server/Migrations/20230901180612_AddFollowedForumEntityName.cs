using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumAppJWTAzure.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowedForumEntityName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowedPosts_AspNetUsers_FollowerId",
                table: "FollowedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowedPosts_Forums_ForumId",
                table: "FollowedPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowedPosts",
                table: "FollowedPosts");

            migrationBuilder.RenameTable(
                name: "FollowedPosts",
                newName: "FollowedForums");

            migrationBuilder.RenameIndex(
                name: "IX_FollowedPosts_ForumId",
                table: "FollowedForums",
                newName: "IX_FollowedForums_ForumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowedForums",
                table: "FollowedForums",
                columns: new[] { "FollowerId", "ForumId" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "30a24107-d279-4e37-96fd-01af5b38cb27",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b946ffe1-dd7b-4d62-ba1b-5ba0dd4f0b97", "AQAAAAIAAYagAAAAEB9XFkuG2jiJHQyD3dp5pVURFkletFNcGBTyrUf+YA9wGxuCVgwwP4EM7/L8520v9A==", "f90d4c62-41bc-44cd-a872-4fbd0dee5e94" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e448afa-f008-446e-a52f-13c449803c2e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "33d8e5b2-e4b0-44b6-bb4c-39e55772c175", "AQAAAAIAAYagAAAAEADWNRZ3rL7eJsPfeC8yNeYWcdK5nXgKp+X/Lp/iyXcaOfi6WkUgAocLxMM9mwrxBQ==", "83014d35-eb4a-432f-94e3-6bfb73c388fe" });

            migrationBuilder.AddForeignKey(
                name: "FK_FollowedForums_AspNetUsers_FollowerId",
                table: "FollowedForums",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowedForums_Forums_ForumId",
                table: "FollowedForums",
                column: "ForumId",
                principalTable: "Forums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowedForums_AspNetUsers_FollowerId",
                table: "FollowedForums");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowedForums_Forums_ForumId",
                table: "FollowedForums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowedForums",
                table: "FollowedForums");

            migrationBuilder.RenameTable(
                name: "FollowedForums",
                newName: "FollowedPosts");

            migrationBuilder.RenameIndex(
                name: "IX_FollowedForums_ForumId",
                table: "FollowedPosts",
                newName: "IX_FollowedPosts_ForumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowedPosts",
                table: "FollowedPosts",
                columns: new[] { "FollowerId", "ForumId" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_FollowedPosts_AspNetUsers_FollowerId",
                table: "FollowedPosts",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowedPosts_Forums_ForumId",
                table: "FollowedPosts",
                column: "ForumId",
                principalTable: "Forums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
