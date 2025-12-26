using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Model
{
    public class CompletedWorkoutDetailed
    {
        public CompletedWorkout CompletedWorkout { get; set; }
        public Workout BasedOnWorkout { get; set; }
        public IEnumerable<CompletedExerciseSetDetailed> DetailedCompletedExerciseSets { get; set; }

        public CompletedWorkoutDetailed(
                CompletedWorkout completedWorkout, 
                Workout basedOnWorkout, 
                IEnumerable<CompletedExerciseSetDetailed> detailedCompletedExerciseSets) { 
            CompletedWorkout = completedWorkout;
            BasedOnWorkout = basedOnWorkout;
            DetailedCompletedExerciseSets = detailedCompletedExerciseSets;
        }
    }
}
