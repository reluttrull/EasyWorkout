using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class RegistrationRequest
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [MaxLength(40)]
        public required string Password { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
    }
}
