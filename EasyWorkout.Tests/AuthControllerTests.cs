using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api.Controllers;
using EasyWorkout.Identity.Api.Data;
using EasyWorkout.Identity.Api.Model;
using EasyWorkout.Identity.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace EasyWorkout.Tests
{
    public class AuthControllerTests
    {
        private readonly IUserStore<User> _userStore;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthControllerTests()
        {
            _userStore = Substitute.For<IUserStore<User>>();

            _userManager = Substitute.For<UserManager<User>>(
                _userStore,
                null, null, null, null, null, null, null, null
            );
            _userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>())
                       .Returns(IdentityResult.Success);

            _userManager.AddClaimAsync(Arg.Any<User>(), Arg.Any<Claim>())
                       .Returns(IdentityResult.Success);
            _logger = Substitute.For<ILogger<AuthController>>();
            _tokenService = Substitute.For<ITokenService>();
            _userService = Substitute.For<IUserService>();
        }

        [Fact]
        public async Task Register_ValidRequest_ShouldReturnCreatedAtAction()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new AppDbContext(options))
            {
                AuthController controller = new(_userManager, _logger, context, _tokenService, _userService);

                RegistrationRequest request = new()
                {
                    UserName = "test",
                    Email = "valid@email.com",
                    Password = "abc123DEF?"
                };

                var response = await controller.Register(request);
                Assert.NotNull(response);
                Assert.IsType<CreatedAtActionResult>(response);
            }
        }
    }
}
