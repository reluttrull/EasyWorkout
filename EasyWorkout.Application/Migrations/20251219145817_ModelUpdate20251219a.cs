using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyWorkout.Application.Migrations
{
    /// <inheritdoc />
    public partial class ModelUpdate20251219a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Workouts_WorkoutId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_WorkoutId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "WorkoutId",
                table: "Exercises");

            migrationBuilder.CreateTable(
                name: "CompletedWorkouts",
                columns: table => new
                {
                    CompletedWorkoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedWorkouts", x => x.CompletedWorkoutId);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseWorkout",
                columns: table => new
                {
                    ExercisesExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkoutsWorkoutId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseWorkout", x => new { x.ExercisesExerciseId, x.WorkoutsWorkoutId });
                    table.ForeignKey(
                        name: "FK_ExerciseWorkout_Exercises_ExercisesExerciseId",
                        column: x => x.ExercisesExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseWorkout_Workouts_WorkoutsWorkoutId",
                        column: x => x.WorkoutsWorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "WorkoutId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletedExercises",
                columns: table => new
                {
                    CompletedExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedWorkoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedExercises", x => x.CompletedExerciseId);
                    table.ForeignKey(
                        name: "FK_CompletedExercises_CompletedWorkouts_CompletedWorkoutId",
                        column: x => x.CompletedWorkoutId,
                        principalTable: "CompletedWorkouts",
                        principalColumn: "CompletedWorkoutId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletedExerciseSets",
                columns: table => new
                {
                    CompletedExerciseSetId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    SetNumber = table.Column<int>(type: "integer", nullable: false),
                    Reps = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    WeightUnit = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<double>(type: "double precision", nullable: false),
                    DurationUnit = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedExerciseSets", x => x.CompletedExerciseSetId);
                    table.ForeignKey(
                        name: "FK_CompletedExerciseSets_CompletedExercises_CompletedExerciseId",
                        column: x => x.CompletedExerciseId,
                        principalTable: "CompletedExercises",
                        principalColumn: "CompletedExerciseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedExercises_CompletedWorkoutId",
                table: "CompletedExercises",
                column: "CompletedWorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedExerciseSets_CompletedExerciseId",
                table: "CompletedExerciseSets",
                column: "CompletedExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseWorkout_WorkoutsWorkoutId",
                table: "ExerciseWorkout",
                column: "WorkoutsWorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedExerciseSets");

            migrationBuilder.DropTable(
                name: "ExerciseWorkout");

            migrationBuilder.DropTable(
                name: "CompletedExercises");

            migrationBuilder.DropTable(
                name: "CompletedWorkouts");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkoutId",
                table: "Exercises",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_WorkoutId",
                table: "Exercises",
                column: "WorkoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Workouts_WorkoutId",
                table: "Exercises",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "WorkoutId");
        }
    }
}
