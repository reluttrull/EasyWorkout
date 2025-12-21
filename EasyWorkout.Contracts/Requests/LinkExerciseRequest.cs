using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class LinkExerciseRequest
    {
        public required Guid Id { get; init; }
        public required Guid AddedByUserId { get; init; }
        public required string Name { get; set; }
    }
}
