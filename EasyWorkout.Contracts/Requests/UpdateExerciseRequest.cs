using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class UpdateExerciseRequest
    {
        public required string Name { get; set; }
        public string? Notes { get; set; }
    }
}
