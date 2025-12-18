using EasyWorkout.Application.Model;
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
        public DbSet<ExerciseSet> ExerciseSets { get; set; }
    }
}
