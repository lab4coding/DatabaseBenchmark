using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseBenchmark
{
    class TestRunner
    {

        const string BIOTEXT = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus vehicula, felis pretium suscipit ultrices, mi arcu maximus augue, ac mattis felis purus ut diam. Etiam ullamcorper vitae diam et interdum. Duis consequat venenatis mollis. Curabitur aliquam purus quam, quis ullamcorper elit tempus at. Donec libero elit, ultrices ac dui ac, consequat fringilla diam. Sed posuere vel erat ac ornare. Donec diam dolor, volutpat ac libero ut, congue rhoncus justo. Integer bibendum justo diam, vitae porta nunc fringilla non. Pellentesque pellentesque feugiat vehicula. Maecenas tellus est, iaculis et dignissim sed, rutrum vitae elit. In vitae tempor tellus. In consequat, risus at volutpat suscipit, nisl dolor pretium leo, ut bibendum quam sapien non nisl. Nullam elementum, elit at eleifend congue, sapien tellus pellentesque ligula, nec pellentesque metus leo ac massa. Nam in ligula elementum, consectetur elit nec, tempus odio.

Pellentesque sollicitudin sed risus at ultrices. Nunc nec eros turpis. In non ullamcorper lorem. Praesent non eros malesuada, dictum sapien eget, sollicitudin tellus. Fusce eu malesuada augue, ac molestie felis. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nulla nibh purus, congue in consectetur nec, hendrerit at turpis. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin a mattis ipsum. Duis eu nisl elementum, vestibulum lacus vitae, laoreet dolor. Praesent vel pulvinar enim. Etiam sed erat a lectus molestie aliquam id quis risus. In lectus augue, commodo id cursus sed, sagittis ut leo. In hac habitasse platea dictumst.";

        internal static async Task TestConcurrent(int count, string testName, Func<IServiceProvider, string, Task> taskFactory, IServiceProvider serviceProvider)
        {
            var list = new List<Task>(count);
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                list.Add(taskFactory(serviceProvider, $"{testName} {i + 1}"));
            }
            await Task.WhenAll(list).ConfigureAwait(false);
            sw.Stop();
            Console.WriteLine($"{testName}: with concurrency {count} completed in {sw.Elapsed}");
        }

        internal static async Task TestDatabase<T>(string dbName, DbTestOptions options, IServiceProvider serviceProvider) where T: TestDbContext
        {
            await TestConcurrent(1, $"{dbName} Prepare", TestFactories.GetPrepare<T>(), serviceProvider);
            await TestConcurrent(options.ConcurrentCount,
                $"{dbName} Create",
                TestFactories.GetCreate<T>(options.RecordCount, options.InsertPageSize),
                serviceProvider);
            await TestConcurrent(options.ConcurrentCount,
                $"{dbName} Update",
                TestFactories.GetUpdate<T>($"{dbName} Update", options.ReadPageSize),
                serviceProvider);
            await TestConcurrent(options.ConcurrentCount,
                $"{dbName} Read",
                TestFactories.GetRead<T>(options.ReadCount),
                serviceProvider);
            await TestConcurrent(1,
                $"{dbName} Delete",
                TestFactories.GetDelete<T>(options.ReadPageSize, options.ConcurrentCount),
                serviceProvider);
        }

        internal static async Task TestRead(TestDbContext context, int count)
        {
            var users = context.Users;
            var totalCount = await users.CountAsync().ConfigureAwait(false);

            var knownUser = await users.AsNoTracking().Skip(totalCount / 2).FirstOrDefaultAsync().ConfigureAwait(false);
            var knownId = knownUser.Id;

            for (int i = 0; i < count; i++)
            {
                await users.AsNoTracking().Where(e => e.Id == knownId).FirstOrDefaultAsync().ConfigureAwait(false);
            }
        }

        internal static async Task TestUpdate(TestDbContext context, string taskName, int readPageSize)
        {
            var users = context.Users;
            var totalCount = await users.CountAsync().ConfigureAwait(false);
            var pageCount = totalCount / readPageSize;
            var lastPageItemCount = totalCount % readPageSize;
            if (lastPageItemCount != 0)
                pageCount++;
            else
                lastPageItemCount = readPageSize;

            var modifiedText = $"Modified by {taskName}";
            for (int i = 0; i < pageCount; i++)
            {
                var itemCount = i == (pageCount - 1) ? lastPageItemCount : readPageSize;
                var list = await context.Users.Skip(i * readPageSize).Take(itemCount).ToListAsync().ConfigureAwait(false);
                for (int j = 0; j < list.Count - 1; j++)
                {
                    var user = list[j];
                    user.Category = modifiedText;
                    user.ModifiedAt = DateTimeOffset.Now;
                }
                context.UpdateRange(list);
                await context.SaveChangesAsync().ConfigureAwait(false);
                context.ChangeTracker.Clear();
            }
        }

        internal static async Task TestDelete(TestDbContext context, int readPageSize, int batchSize = 1)
        {
            var users = context.Users;
            var totalCount = await users.CountAsync().ConfigureAwait(false);

            int deletedCount = 0;

            while (deletedCount < totalCount)
            {
                var list = await context.Users.Take(readPageSize).ToListAsync().ConfigureAwait(false);
                var c = 0;
                for (int j = 0; j < list.Count - 1; j++)
                {
                    context.Remove(list[j]);
                    c++;
                    if (c >= batchSize)
                    {
                        await context.SaveChangesAsync().ConfigureAwait(false);
                        c = 0;
                    }
                }
                if (c > 0)
                {
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
                context.ChangeTracker.Clear();
                deletedCount += list.Count;
            }
        }

        internal static async Task PrepareForTests(TestDbContext context)
        {
            await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
            await TestCreate(context, 100, 10).ConfigureAwait(false);
            await TestDelete(context, 100).ConfigureAwait(false);
        }

        internal static async Task TestCreate(TestDbContext context, int count, int pageSize)
        {
            var pageCount = count / pageSize;
            var lastPageItemCount = count % pageSize;
            if (lastPageItemCount != 0)
                pageCount++;
            else
                lastPageItemCount = pageSize;

            var list = new List<User>(pageSize);

            for (int i = 0; i < pageCount; i++)
            {
                list.Clear();
                var startIndex = (i * pageSize) + 1;
                var itemCount = i == (pageCount - 1) ? lastPageItemCount : pageSize;
                for (int j = 0; j < itemCount; j++)
                {
                    list.Add(new User
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTimeOffset.Now,
                        Category = $"Category {startIndex}",
                        Name = $"Name {startIndex}",
                        Bio = BIOTEXT
                    });
                    startIndex++;
                }
                context.AddRange(list);
                await context.SaveChangesAsync().ConfigureAwait(false);
                context.ChangeTracker.Clear();
            }
        }
    }
}
