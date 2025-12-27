using System.ComponentModel.DataAnnotations;

namespace EasyWorkout.Contracts.Requests
{
    public class LoginRequest
    {
        public required string UserName { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
