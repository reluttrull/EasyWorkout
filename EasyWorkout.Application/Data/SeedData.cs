using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Model;
using EasyWorkout.Identity.Api.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Data
{
    public static class SeedData
    {
        public static void Initialize(WorkoutsContext workoutsContext, List<User> testUsers)
        {
            if (testUsers.Count == 0) return;

            List<Workout> workouts =
            [
                new Workout()
                {
                    Id = Guid.NewGuid(),
                    AddedByUserId = testUsers[0].Id,
                    AddedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Name = "Workout #1",
                    Exercises = [],
                    Notes = "sdfjlk sd fjlksd fjklsd flks jlakfjsdlkjfldsj lj kl sajkl l jsdjk sdlkj dsjkls jlksk kfsdlj lksdf kljsdf lkdjkjlksfjkl dfsj fsdksd"
                }
            ];

            List<Exercise> exercises =
            [
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    AddedByUserId = testUsers[0].Id,
                    AddedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Name = "Exercise #1",
                    Workouts = [workouts[0]]
                }
            ];
            workouts[0].Exercises.Add(exercises[0]);

            List<ExerciseSet> exerciseSets =
            [
                new ExerciseSet()
                {
                    Id = Guid.NewGuid(),
                    ExerciseId = exercises[0].Id,
                    SetNumber = 1,
                    Reps = 5,
                    Weight = 20,
                    WeightUnit = Enums.WeightUnit.Kilograms
                }
            ];


            if (!workoutsContext.Workouts.Any())
            {
                workoutsContext.Workouts.AddRange(workouts);
                workoutsContext.Exercises.AddRange(exercises);
                workoutsContext.ExerciseSets.AddRange(exerciseSets);
                workoutsContext.SaveChanges();
            }
        }
    }
}
