using EasyWorkout.Application.Data;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api.Data;
using EasyWorkout.Identity.Api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _usersContext;
        private readonly UserManager<User> _userManager;

        public UserService(AppDbContext userContext, UserManager<User> userManager)
        {
            _usersContext = userContext;
            _userManager = userManager;
        }

        public async Task<bool> ChangeEmailAsync(Guid id, ChangeEmailRequest request, CancellationToken token = default)
        {
            var userToChange = await GetByIdAsync(id, token);
            if (userToChange is null) return false;

            userToChange.Email = request.Email;

            await _usersContext.SaveChangesAsync(token);

            return true;
        }

        public async Task<bool> ChangePasswordAsync(Guid id, ChangePasswordRequest request, CancellationToken token = default)
        {
            var userToChange = await GetByIdAsync(id, token);
            if (userToChange is null) return false;

            var changePasswordResult = await _userManager.ChangePasswordAsync(userToChange, request.CurrentPassword, request.NewPassword);
            if (!changePasswordResult.Succeeded) return false;

            return true;
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
