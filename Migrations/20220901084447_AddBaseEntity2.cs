using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManager.Migrations
{
    public partial class AddBaseEntity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5f0c1d5f-6be0-41bf-8e1b-37d12f7b5419");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedBy", "UpdatedOn", "UserName" },
                values: new object[] { "12a4e7a1-f314-4ad4-99b6-ffc03187fcae", 0, "44dc17ce-275c-4752-9204-cdfe9214df28", null, new DateTime(2022, 9, 1, 11, 44, 46, 986, DateTimeKind.Local).AddTicks(1665), "admin@admin.com", true, false, false, null, "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAEAACcQAAAAECXziwrDGu/P5SkiGFD1cmgtUze+YltDWdpWbMdfhFxUhXjQ2Q5AgZc26jXzhDJJjg==", "+9999999999", true, "b5238b98-9d33-45f4-bffd-79d02bb84d95", false, null, null, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "12a4e7a1-f314-4ad4-99b6-ffc03187fcae");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedBy", "UpdatedOn", "UserName" },
                values: new object[] { "5f0c1d5f-6be0-41bf-8e1b-37d12f7b5419", 0, "2c16861d-d9ef-4cc1-ae3a-79bf759f3712", null, new DateTime(2022, 9, 1, 11, 43, 22, 787, DateTimeKind.Local).AddTicks(5926), "admin@admin.com", true, false, false, null, "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAEAACcQAAAAEEulDNgZlxLPDXdgfMeJdAs+UkZf3/HEGWts37MSxfRQI4qj8i9ewp0AgXjG9exKYQ==", "+9999999999", true, "a57e9a1d-d29d-42f1-b4db-df37ab6125a5", false, null, null, "admin" });
        }
    }
}
