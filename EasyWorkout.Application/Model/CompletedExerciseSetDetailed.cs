using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Model
{
    public class CompletedExerciseSetDetailed
    {
        public CompletedExerciseSet CompletedExerciseSet { get; set; }
        public ExerciseSet BasedOnExerciseSet { get; set; }
        public string ExerciseName { get; set; }

        public CompletedExerciseSetDetailed(CompletedExerciseSet completedExerciseSet, ExerciseSet basedOnExerciseSet, string exerciseName)
        {
            CompletedExerciseSet = completedExerciseSet;
            BasedOnExerciseSet = basedOnExerciseSet;
            ExerciseName = exerciseName;
        }
    }
}
