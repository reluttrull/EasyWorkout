using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class ChangePasswordRequest
    {
        [Required]
        [MaxLength(40)]
        public required string Password { get; set; }
    }
}
