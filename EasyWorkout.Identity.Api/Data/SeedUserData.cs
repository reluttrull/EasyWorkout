using EasyWorkout.Identity.Api.Model;

namespace EasyWorkout.Identity.Api.Data
{
    public class SeedUserData
    {
        public static void Initialize(AppDbContext appDbContext)
        {
            List<User> users =
            [
                new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = "TestUser",
                    Email = "email@email.com",
                    JoinedDate = DateOnly.FromDateTime(DateTime.UtcNow)
                }
            ];


            if (!appDbContext.Users.Any())
            {
                appDbContext.Users.AddRange(users);
                appDbContext.SaveChanges();
            }
        }
    }
}
