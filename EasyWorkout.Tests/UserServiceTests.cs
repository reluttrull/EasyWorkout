using EasyWorkout.Application.Data;
using EasyWorkout.Application.Model;
using EasyWorkout.Application.Services;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api.Data;
using EasyWorkout.Identity.Api.Model;
using EasyWorkout.Identity.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Tests
{
    [Collection("EF")]
    public class UserServiceTests
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        public UserServiceTests()
        {
            _userStore = Substitute.For<IUserStore<User>>();

            _userManager = Substitute.For<UserManager<User>>(
                _userStore,
                null, null, null, null, null, null, null, null
            );
        }

        [Fact]
        public async Task ChangeEmailAsync_WhenValidRequest_ShouldChangeEmailAddress()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new AppDbContext(options))
            {
                var userService = new UserService(context, _userManager);
                var userId = Guid.NewGuid();
                var firstEdited = DateTime.UtcNow;
                var user = new User()
                {
                    Id = userId,
                    LastEditedDate = firstEdited
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                ChangeEmailRequest request = new()
                {
                    Email = "new@email.com"
                };

                var changedUser = await userService.ChangeEmailAsync(userId, request);
                Assert.NotNull(changedUser);
                Assert.Equal("new@email.com", changedUser.Email);
                Assert.NotEqual(firstEdited, changedUser.LastEditedDate);
            }
        }

        [Fact]
        public async Task GetByIdAsync_WhenUserExists_ShouldReturnUser()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new AppDbContext(options))
            {
                var userService = new UserService(context, _userManager);
                var userId = Guid.NewGuid();
                var user = new User()
                {
                    Id = userId,
                    LastEditedDate = DateTime.UtcNow,
                    FirstName = "Ron"
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                var retrievedUser = await userService.GetByIdAsync(userId);
                Assert.NotNull(retrievedUser);
                Assert.IsType<User>(retrievedUser);
                Assert.Equal("Ron", retrievedUser.FirstName);
            }
        }

        [Fact]
        public async Task UpdateAsync_WhenRequestValid_ShouldUpdateUserProperties()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new AppDbContext(options))
            {
                var userService = new UserService(context, _userManager);
                var userId = Guid.NewGuid();
                var firstEdited = DateTime.UtcNow;
                var user = new User()
                {
                    Id = userId,
                    LastEditedDate = firstEdited
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                UpdateUserRequest request = new()
                {
                    FirstName = "Bob",
                    LastName = "The Builder"
                };

                var changedUser = await userService.UpdateAsync(userId, request);
                Assert.NotNull(changedUser);
                Assert.Equal("Bob", changedUser.FirstName);
                Assert.NotEqual(firstEdited, changedUser.LastEditedDate);
            }
        }
    }
}
