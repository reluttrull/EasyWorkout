using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class ChangeEmailRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
