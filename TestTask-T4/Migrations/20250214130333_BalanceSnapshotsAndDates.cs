using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask_T4.Migrations
{
    /// <inheritdoc />
    public partial class BalanceSnapshotsAndDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BalanceSnapshotId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Transactions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Clients",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Clients",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Clients",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "BalanceSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceSnapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BalanceSnapshotId",
                table: "Transactions",
                column: "BalanceSnapshotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BalanceSnapshots_BalanceSnapshotId",
                table: "Transactions",
                column: "BalanceSnapshotId",
                principalTable: "BalanceSnapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BalanceSnapshots_BalanceSnapshotId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "BalanceSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BalanceSnapshotId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BalanceSnapshotId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Clients");
        }
    }
}
