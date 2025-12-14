using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Model
{
    public class Exercise
    {
        public required Guid ExerciseId { get; init; }
        public required Guid AddedByUserId { get; init; }
        public required DateOnly AddedDate { get; init; }
        public required string Name { get; set; }
        public string Notes { get; set; } = string.Empty;
        public List<ExerciseSet> ExerciseSets { get; init; } = [];
    }
}
