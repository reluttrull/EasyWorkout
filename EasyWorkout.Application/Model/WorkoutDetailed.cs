using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Model
{
    public class WorkoutDetailed
    {
        public Workout Workout { get; set; }
        public DateTime? LastCompletedDate { get; set; }

        public WorkoutDetailed(Workout workout, DateTime? lastCompletedDate)
        {
            Workout = workout;
            LastCompletedDate = lastCompletedDate;
        }
    }
}
