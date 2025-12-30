using EasyWorkout.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class CreateWorkoutRequest
    {
        [Required]
        [MaxLength(75, ErrorMessage = "{0} must be {1} characters or fewer.")]
        public required string Name { get; set; }
        [MaxLength(250, ErrorMessage = "{0} must be {1} characters or fewer.")]
        public string? Notes { get; set; }
    }
}
