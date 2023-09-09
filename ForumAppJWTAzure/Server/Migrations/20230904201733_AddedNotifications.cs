using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumAppJWTAzure.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedById",
                table: "Notifications",
                column: "CreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

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
        }
    }
}
