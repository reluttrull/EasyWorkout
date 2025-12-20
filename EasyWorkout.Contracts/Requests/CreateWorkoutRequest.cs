using EasyWorkout.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class CreateWorkoutRequest
    {
        public required string Name { get; set; }
        public string? Notes { get; set; }
    }
}
