using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoManager.Migrations
{
    /// <inheritdoc />
    public partial class Dbcontex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "maintenancetype",
                table: "MaintenanceRecords",
                newName: "MaintenanceType");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NextServiceDate",
                table: "MaintenanceRecords",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MaintenanceRecords",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "MaintenanceRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MaintenanceRecords",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MaintenanceRecords");

            migrationBuilder.RenameColumn(
                name: "MaintenanceType",
                table: "MaintenanceRecords",
                newName: "maintenancetype");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NextServiceDate",
                table: "MaintenanceRecords",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MaintenanceRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
