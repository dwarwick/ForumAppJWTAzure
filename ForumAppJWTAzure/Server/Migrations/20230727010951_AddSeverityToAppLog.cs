using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumAppJWTAzure.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSeverityToAppLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Severity",
                table: "AppLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "30a24107-d279-4e37-96fd-01af5b38cb27",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "ModifiedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "42272744-9b32-4401-b5b2-7e0a3b7f3c66", null, null, "AQAAAAIAAYagAAAAEFpLHO3AKAhzrbGRNSZNiw2fOYoTtV4JxcA6ey0C13GZITi+PaDsPoxb/gnBk+qN/A==", "466b0995-1d9b-4799-8d4c-faaab76ea6cb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e448afa-f008-446e-a52f-13c449803c2e",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "ModifiedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "adff69bc-ef38-4527-814d-e04f9dfc93c2", null, null, "AQAAAAIAAYagAAAAEG7X7fD/SZNkPxMRmSUYpzJG2B+iKpk4ZiSxc0z1X344PivYHJvCNgVO/uqux53zkw==", "9d0883c2-dab4-437d-8f32-5f29614185e6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Severity",
                table: "AppLogs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "30a24107-d279-4e37-96fd-01af5b38cb27",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "ModifiedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d242fa19-6109-4507-820f-8bb75ad2e060", new DateTime(2023, 7, 26, 2, 21, 19, 73, DateTimeKind.Utc).AddTicks(6295), new DateTime(2023, 7, 26, 2, 21, 18, 971, DateTimeKind.Utc).AddTicks(63), "AQAAAAIAAYagAAAAEO4kVy53xYAV3JYeQnLWtU8NqLOxfp4MlyqcYp64Jt83pv3ExhJA4F1Q7TF9t46xdw==", "2322cca0-15c7-446c-902f-bc2ad0d23393" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e448afa-f008-446e-a52f-13c449803c2e",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "ModifiedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "109de897-597c-4f10-8d15-f73072db89f0", new DateTime(2023, 7, 26, 2, 21, 18, 971, DateTimeKind.Utc).AddTicks(39), new DateTime(2023, 7, 26, 2, 21, 18, 866, DateTimeKind.Utc).AddTicks(4129), "AQAAAAIAAYagAAAAEEaWwykNRfcxfsq1CGMp/XdF8Q7VFnTRptdx52oW2vN9srn3oR3ElpuLb5stuXMK3A==", "425e1974-bed2-4ccc-aa43-8566aa46ba27" });
        }
    }
}
