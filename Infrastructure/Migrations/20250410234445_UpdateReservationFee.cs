using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FeeId",
                table: "Reservation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_FeeId",
                table: "Reservation",
                column: "FeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Fee_FeeId",
                table: "Reservation",
                column: "FeeId",
                principalTable: "Fee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Fee_FeeId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_FeeId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "FeeId",
                table: "Reservation");
        }
    }
}
