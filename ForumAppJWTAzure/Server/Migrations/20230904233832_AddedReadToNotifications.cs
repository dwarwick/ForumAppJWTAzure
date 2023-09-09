using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumAppJWTAzure.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedReadToNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Read",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "30a24107-d279-4e37-96fd-01af5b38cb27",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a6dc1a98-f49a-4bde-bde5-5a180813ba07", "AQAAAAIAAYagAAAAEJGO8EujrySeNgsEmXosIjcQ2LkwAvYcu5NCwZj9gjQXqEdB6J/FEm1GX2wFd1LXzw==", "e93206df-49d5-4886-9043-6c3aeb336626" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e448afa-f008-446e-a52f-13c449803c2e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7af41776-2432-4375-be7f-dd02eb2e8cae", "AQAAAAIAAYagAAAAEOPxjDNFH1G8DiOeAc20OBsjOeB6D2Jsz8SozFnfJxJuszJrM8wV3zgMA9RU+vBTSQ==", "43c8c5ad-06b3-48ac-b7c7-efa690c65966" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Read",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "30a24107-d279-4e37-96fd-01af5b38cb27",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fc675e9c-ea1d-4e23-96c7-a0cf4301125e", "AQAAAAIAAYagAAAAEH9n65i6IiMyQKN77HUF+skk9qM+rtO4xsVYth70ULqq6K0BRpzjd3nzOkoM+0P3GQ==", "e8e73b13-18e9-495f-8ea8-58276f8c049a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e448afa-f008-446e-a52f-13c449803c2e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "92c73e60-3422-46c5-adfc-b1cbcfaf25d6", "AQAAAAIAAYagAAAAEBKjKZ+ugTi5n5UnJYPRpeyWkVCoFf4ZTPWp3WHMJQRzRLczqzd1CbC8j87YbMy93g==", "1b765d9a-bfc5-4e17-af98-1d74367bc34c" });
        }
    }
}
