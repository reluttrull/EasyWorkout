using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Application.Model
{
    public class Notes
    {
        [Key]
        public required Guid Id { get; init; }
        [MaxLength(250)]
        public required string Content { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}
