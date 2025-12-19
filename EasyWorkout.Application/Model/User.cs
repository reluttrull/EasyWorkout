using System.ComponentModel.DataAnnotations;

namespace EasyWorkout.Application.Model
{
    public class User
    {
        [Key]
        public required Guid Id { get; init; }
        [Required]
        [MaxLength(50)]
        public required string UserName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        public DateOnly JoinedDate { get; init; }
    }
}
