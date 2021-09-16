using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DatabaseBenchmark
{
    static class TestFactories
    {
        public static Func<IServiceProvider, string, Task> GetCreate<T>(int count = 10000, int pageSize = 100) where T: TestDbContext
        {
            return async (sp, t) =>
            {
                using var scope = sp.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<T>();
                await TestRunner.TestCreate(context, count, pageSize).ConfigureAwait(false);
            };
        }

        public static Func<IServiceProvider, string, Task> GetUpdate<T>(string taskName, int readPageSize = 100) where T : TestDbContext
        {
            return async (sp, t) =>
            {
                using var scope = sp.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<T>();
                await TestRunner.TestUpdate(context, taskName, readPageSize).ConfigureAwait(false);
            };
        }

        public static Func<IServiceProvider, string, Task> GetRead<T>(int readCount = 10000) where T : TestDbContext
        {
            return async (sp, t) =>
            {
                using var scope = sp.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<T>();
                await TestRunner.TestRead(context, readCount).ConfigureAwait(false);
            };
        }

        public static Func<IServiceProvider, string, Task> GetDelete<T>(int readPageSize = 100) where T : TestDbContext
        {
            return async (sp, t) =>
            {
                using var scope = sp.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<T>();
                await TestRunner.TestDelete(context, readPageSize).ConfigureAwait(false);
            };
        }

        public static Func<IServiceProvider, string, Task> GetPrepare<T>() where T : TestDbContext
        {
            return async (sp, t) =>
            {
                using var scope = sp.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<T>();
                await TestRunner.PrepareForTests(context).ConfigureAwait(false);
            };
        }
    }
}
