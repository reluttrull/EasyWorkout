using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyWorkout.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddDistanceToSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Distance",
                table: "ExerciseSets",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistanceUnit",
                table: "ExerciseSets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Distance",
                table: "CompletedExerciseSets",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistanceUnit",
                table: "CompletedExerciseSets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GoalDistance",
                table: "CompletedExerciseSets",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "ExerciseSets");

            migrationBuilder.DropColumn(
                name: "DistanceUnit",
                table: "ExerciseSets");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "CompletedExerciseSets");

            migrationBuilder.DropColumn(
                name: "DistanceUnit",
                table: "CompletedExerciseSets");

            migrationBuilder.DropColumn(
                name: "GoalDistance",
                table: "CompletedExerciseSets");
        }
    }
}
