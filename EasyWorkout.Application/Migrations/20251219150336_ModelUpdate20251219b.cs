using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyWorkout.Application.Migrations
{
    /// <inheritdoc />
    public partial class ModelUpdate20251219b : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseWorkout_Exercises_ExercisesExerciseId",
                table: "ExerciseWorkout");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseWorkout_Workouts_WorkoutsWorkoutId",
                table: "ExerciseWorkout");

            migrationBuilder.RenameColumn(
                name: "WorkoutId",
                table: "Workouts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "WorkoutsWorkoutId",
                table: "ExerciseWorkout",
                newName: "WorkoutsId");

            migrationBuilder.RenameColumn(
                name: "ExercisesExerciseId",
                table: "ExerciseWorkout",
                newName: "ExercisesId");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseWorkout_WorkoutsWorkoutId",
                table: "ExerciseWorkout",
                newName: "IX_ExerciseWorkout_WorkoutsId");

            migrationBuilder.RenameColumn(
                name: "ExerciseSetId",
                table: "ExerciseSets",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ExerciseId",
                table: "Exercises",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CompletedWorkoutId",
                table: "CompletedWorkouts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CompletedExerciseSetId",
                table: "CompletedExerciseSets",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CompletedExerciseId",
                table: "CompletedExercises",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseWorkout_Exercises_ExercisesId",
                table: "ExerciseWorkout",
                column: "ExercisesId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseWorkout_Workouts_WorkoutsId",
                table: "ExerciseWorkout",
                column: "WorkoutsId",
                principalTable: "Workouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseWorkout_Exercises_ExercisesId",
                table: "ExerciseWorkout");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseWorkout_Workouts_WorkoutsId",
                table: "ExerciseWorkout");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Workouts",
                newName: "WorkoutId");

            migrationBuilder.RenameColumn(
                name: "WorkoutsId",
                table: "ExerciseWorkout",
                newName: "WorkoutsWorkoutId");

            migrationBuilder.RenameColumn(
                name: "ExercisesId",
                table: "ExerciseWorkout",
                newName: "ExercisesExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseWorkout_WorkoutsId",
                table: "ExerciseWorkout",
                newName: "IX_ExerciseWorkout_WorkoutsWorkoutId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ExerciseSets",
                newName: "ExerciseSetId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Exercises",
                newName: "ExerciseId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CompletedWorkouts",
                newName: "CompletedWorkoutId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CompletedExerciseSets",
                newName: "CompletedExerciseSetId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CompletedExercises",
                newName: "CompletedExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseWorkout_Exercises_ExercisesExerciseId",
                table: "ExerciseWorkout",
                column: "ExercisesExerciseId",
                principalTable: "Exercises",
                principalColumn: "ExerciseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseWorkout_Workouts_WorkoutsWorkoutId",
                table: "ExerciseWorkout",
                column: "WorkoutsWorkoutId",
                principalTable: "Workouts",
                principalColumn: "WorkoutId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
