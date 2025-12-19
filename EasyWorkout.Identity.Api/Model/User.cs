using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EasyWorkout.Identity.Api.Model
{
    public class User : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        public DateOnly JoinedDate { get; init; }
    }
}
