using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class ChangePasswordRequest
    {
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(40)]
        public required string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(40)]
        public required string NewPassword { get; set; }
    }
}
