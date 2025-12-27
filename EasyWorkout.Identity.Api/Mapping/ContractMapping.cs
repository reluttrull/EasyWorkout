using EasyWorkout.Contracts.Responses;
using EasyWorkout.Identity.Api.Model;

namespace EasyWorkout.Identity.Api.Mapping
{
    public static class ContractMapping
    {

        public static UserResponse MapToResponse(this User user)
        {
            return new UserResponse()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                JoinedDate = user.JoinedDate
            };
        }
    }
}
