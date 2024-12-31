using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrightStar.Services.Infrastructure.Migrations
{
    public partial class Updatetokenconfigs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventSubscriptions_Events_EventId",
                table: "EventSubscriptions");

            migrationBuilder.AddForeignKey(
                name: "FK_EventSubscriptions_Events_EventId",
                table: "EventSubscriptions",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventSubscriptions_Events_EventId",
                table: "EventSubscriptions");

            migrationBuilder.AddForeignKey(
                name: "FK_EventSubscriptions_Events_EventId",
                table: "EventSubscriptions",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
