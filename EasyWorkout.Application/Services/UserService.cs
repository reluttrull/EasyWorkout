using EasyWorkout.Application.Data;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api.Data;
using EasyWorkout.Identity.Api.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _usersContext;

        public UserService(AppDbContext userContext)
        {
            _usersContext = userContext; 
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _usersContext.Users
                .FirstOrDefault(e => e.Id == id);
        }

        public async Task<User?> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken token = default)
        {
            var userToChange = _usersContext.Users.First(e => e.Id == id);
            if (userToChange is null) return null;

            userToChange.FirstName = request.FirstName;
            userToChange.LastName = request.LastName;

            await _usersContext.SaveChangesAsync(token);

            return userToChange;
        }
    }
}
