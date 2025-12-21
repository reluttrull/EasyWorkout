using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class UpdateWorkoutRequest
    {
        public required string Name { get; set; }
        public string? Notes { get; set; }
    }
}
