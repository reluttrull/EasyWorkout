using EasyWorkout.Application.Data;
using EasyWorkout.Identity.Api.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWorkout.Tests
{
    public class EfWarmupFixture
    {
        public EfWarmupFixture()
        {
            var options = new DbContextOptionsBuilder<WorkoutsContext>()
                .UseInMemoryDatabase("Warmup")
                .Options;

            using var context = new WorkoutsContext(options);
            context.Database.EnsureCreated();

            var authOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Warmup")
                .Options;

            using var authcontext = new AppDbContext(authOptions);
            authcontext.Database.EnsureCreated();
        }
    }

    [CollectionDefinition("EF")]
    public class EfCollection : ICollectionFixture<EfWarmupFixture> { }
}
