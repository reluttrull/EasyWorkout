using EasyWorkout.Application.Data;
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

            using var ctx = new WorkoutsContext(options);
            ctx.Database.EnsureCreated();
        }
    }

    [CollectionDefinition("EF")]
    public class EfCollection : ICollectionFixture<EfWarmupFixture> { }
}
