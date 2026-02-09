using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Contracts.Requests
{
    public class RevokeRequest
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
}
