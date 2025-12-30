using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Responses
{
    public class UserResponse
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public DateOnly JoinedDate { get; init; }
        public DateTime LastEditedDate { get; init; }
    }
}
