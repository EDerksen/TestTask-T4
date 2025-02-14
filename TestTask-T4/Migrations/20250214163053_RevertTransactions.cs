using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask_T4.Migrations
{
    /// <inheritdoc />
    public partial class RevertTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RevertTransactionId",
                table: "Transactions",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevertTransactionId",
                table: "Transactions");
        }
    }
}
