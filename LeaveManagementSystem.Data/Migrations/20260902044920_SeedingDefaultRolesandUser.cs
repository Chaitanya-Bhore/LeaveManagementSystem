using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultRolesandUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "418e3361-c56a-4d22-b825-8953e615762a", "88a2a3a2-842d-4552-a48c-fba06715158b", "Supervisor", "SUPERVISOR" },
                    { "8edd273b-c9dd-4849-8892-240fd61a9dca", "172ad0f4-02c2-422a-9896-94490cd1a0c2", "Employee", "EMPLOYEE" },
                    { "b84229b8-ca2e-4423-9610-52c3c7e177bf", "9fc2c7e6-2bb4-4974-8f5e-cf90b16f09b4", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f8e1c3b0-5d4e-4f9a-9c6b-2e1f3c4d5e6f", 0, "AQAAAAEAACcQAAAAEJ2a8b9c0d1e2f3g4h5i6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEIxW2R98V3x5AWuqNCMJhOm+/COLTAQSPrQLMy43XLclF00+E0827xHmWLTH2E9LeQ==", null, false, "AQAAAAEAACcQAAAAEJ2a8b9c0d1e2f3g4h5i6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3", false, "admin@example.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b84229b8-ca2e-4423-9610-52c3c7e177bf", "f8e1c3b0-5d4e-4f9a-9c6b-2e1f3c4d5e6f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "418e3361-c56a-4d22-b825-8953e615762a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8edd273b-c9dd-4849-8892-240fd61a9dca");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b84229b8-ca2e-4423-9610-52c3c7e177bf", "f8e1c3b0-5d4e-4f9a-9c6b-2e1f3c4d5e6f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b84229b8-ca2e-4423-9610-52c3c7e177bf");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f8e1c3b0-5d4e-4f9a-9c6b-2e1f3c4d5e6f");
        }
    }
}
