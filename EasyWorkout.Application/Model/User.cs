using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Model
{
    public class User
    {
        public required Guid UserId { get; init; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly JoinedDate { get; init; }
    }
}
