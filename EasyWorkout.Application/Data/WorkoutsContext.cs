using EasyWorkout.Application.Model;
using EasyWorkout.Identity.Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Data
{
    public class WorkoutsContext : DbContext
    {
        public WorkoutsContext(DbContextOptions<WorkoutsContext> options) : base(options)
        {
        }

        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<ExerciseSet> ExerciseSets { get; set; }

        public DbSet<CompletedWorkout> CompletedWorkouts { get; set; }
        public DbSet<CompletedExercise> CompletedExercises { get; set; }
        public DbSet<CompletedExerciseSet> CompletedExerciseSets { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // save enums as strings
            configurationBuilder.Properties<Enum>().HaveConversion<string>();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompletedWorkout>()
                .HasOne(cw => cw.Workout)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CompletedExercise>()
                .HasOne(ce => ce.Exercise)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
