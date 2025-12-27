using EasyWorkout.Application.Data;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api.Data;
using EasyWorkout.Identity.Api.Model;
using Microsoft.EntityFrameworkCore;
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
            return await _usersContext.Users.SingleOrDefaultAsync<User>(e => e.Id == id);
        }

        public async Task<User?> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken token = default)
        {
            var userToChange = await GetByIdAsync(id, token);
            if (userToChange is null) return null;

            userToChange.FirstName = request.FirstName;
            userToChange.LastName = request.LastName;

            await _usersContext.SaveChangesAsync(token);

            return userToChange;
        }
    }
}
