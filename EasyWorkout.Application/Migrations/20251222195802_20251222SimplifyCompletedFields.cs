using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyWorkout.Application.Migrations
{
    /// <inheritdoc />
    public partial class _20251222SimplifyCompletedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedExerciseSets_CompletedExercises_CompletedExerciseId",
                table: "CompletedExerciseSets");

            migrationBuilder.DropTable(
                name: "CompletedExercises");

            migrationBuilder.DropColumn(
                name: "DurationUnit",
                table: "CompletedExerciseSets");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "CompletedExerciseSets");

            migrationBuilder.DropColumn(
                name: "WeightUnit",
                table: "CompletedExerciseSets");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "CompletedWorkouts",
                newName: "CompletedNotes");

            migrationBuilder.RenameColumn(
                name: "CompletedExerciseId",
                table: "CompletedExerciseSets",
                newName: "ExerciseSetId");

            migrationBuilder.RenameIndex(
                name: "IX_CompletedExerciseSets_CompletedExerciseId",
                table: "CompletedExerciseSets",
                newName: "IX_CompletedExerciseSets_ExerciseSetId");

            migrationBuilder.AddColumn<Guid>(
                name: "CompletedWorkoutId",
                table: "CompletedExerciseSets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompletedExerciseSets_CompletedWorkoutId",
                table: "CompletedExerciseSets",
                column: "CompletedWorkoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedExerciseSets_CompletedWorkouts_CompletedWorkoutId",
                table: "CompletedExerciseSets",
                column: "CompletedWorkoutId",
                principalTable: "CompletedWorkouts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedExerciseSets_ExerciseSets_ExerciseSetId",
                table: "CompletedExerciseSets",
                column: "ExerciseSetId",
                principalTable: "ExerciseSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedExerciseSets_CompletedWorkouts_CompletedWorkoutId",
                table: "CompletedExerciseSets");

            migrationBuilder.DropForeignKey(
                name: "FK_CompletedExerciseSets_ExerciseSets_ExerciseSetId",
                table: "CompletedExerciseSets");

            migrationBuilder.DropIndex(
                name: "IX_CompletedExerciseSets_CompletedWorkoutId",
                table: "CompletedExerciseSets");

            migrationBuilder.DropColumn(
                name: "CompletedWorkoutId",
                table: "CompletedExerciseSets");

            migrationBuilder.RenameColumn(
                name: "CompletedNotes",
                table: "CompletedWorkouts",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "ExerciseSetId",
                table: "CompletedExerciseSets",
                newName: "CompletedExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_CompletedExerciseSets_ExerciseSetId",
                table: "CompletedExerciseSets",
                newName: "IX_CompletedExerciseSets_CompletedExerciseId");

            migrationBuilder.AddColumn<string>(
                name: "DurationUnit",
                table: "CompletedExerciseSets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "CompletedExerciseSets",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WeightUnit",
                table: "CompletedExerciseSets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CompletedExercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedWorkoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Notes = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedExercises_CompletedWorkouts_CompletedWorkoutId",
                        column: x => x.CompletedWorkoutId,
                        principalTable: "CompletedWorkouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedExercises_CompletedWorkoutId",
                table: "CompletedExercises",
                column: "CompletedWorkoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedExerciseSets_CompletedExercises_CompletedExerciseId",
                table: "CompletedExerciseSets",
                column: "CompletedExerciseId",
                principalTable: "CompletedExercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
