using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumAppJWTAzure.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedImagesStringToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Images",
                table: "Posts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "30a24107-d279-4e37-96fd-01af5b38cb27",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "42272744-9b32-4401-b5b2-7e0a3b7f3c66", "AQAAAAIAAYagAAAAEFpLHO3AKAhzrbGRNSZNiw2fOYoTtV4JxcA6ey0C13GZITi+PaDsPoxb/gnBk+qN/A==", "466b0995-1d9b-4799-8d4c-faaab76ea6cb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e448afa-f008-446e-a52f-13c449803c2e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "adff69bc-ef38-4527-814d-e04f9dfc93c2", "AQAAAAIAAYagAAAAEG7X7fD/SZNkPxMRmSUYpzJG2B+iKpk4ZiSxc0z1X344PivYHJvCNgVO/uqux53zkw==", "9d0883c2-dab4-437d-8f32-5f29614185e6" });
        }
    }
}
