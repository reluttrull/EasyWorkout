using EasyWorkout.Application.Model;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Application.Services
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<User?> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken token = default);

        Task<User?> ChangeEmailAsync(Guid id, ChangeEmailRequest request, CancellationToken token = default);

        Task<User?> ChangePasswordAsync(Guid id, ChangePasswordRequest request, CancellationToken token = default);
    }
}
