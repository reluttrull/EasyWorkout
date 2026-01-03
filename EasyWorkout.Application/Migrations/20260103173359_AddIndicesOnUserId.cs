using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyWorkout.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddIndicesOnUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Workouts_AddedByUserId",
                table: "Workouts",
                column: "AddedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_AddedByUserId",
                table: "Exercises",
                column: "AddedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedWorkouts_CompletedByUserId",
                table: "CompletedWorkouts",
                column: "CompletedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workouts_AddedByUserId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_AddedByUserId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_CompletedWorkouts_CompletedByUserId",
                table: "CompletedWorkouts");
        }
    }
}
