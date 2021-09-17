using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseBenchmark
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json")
                .Build()
                ;

            var container = new ServiceCollection();
            container.ConfigureApplication(configuration);

            using var serviceProvider = container.BuildServiceProvider();

            var options = new DbTestOptions
            {

            };
            var rootCommand = new RootCommand()
            {
                new Option<int>(new[] { "--concurrent", "-c" }, () => 1),
                new Option<int>("--read-pagesize", ()=> options.ReadPageSize),
                new Option<int>("--read-count", () => options.ReadCount),
                new Option<int>("--insert-count", () => options.RecordCount),
                new Option<int>("--insert-pagesize", () => options.InsertPageSize),
                new Option<string[]>(new[] { "--databases", "-d" }, () => Array.Empty<string>())
            };

            rootCommand.Handler = CommandHandler.Create<StartOptions>(async options =>
            {
                await RunWithOptions(options, serviceProvider).ConfigureAwait(false);
            });

            await rootCommand.InvokeAsync(args).ConfigureAwait(false);

            Console.WriteLine("Tests Completed!");
        }

        static async Task RunWithOptions(StartOptions startupOptions, IServiceProvider serviceProvider)
        {
            var options = new DbTestOptions
            {
                ConcurrentCount = Math.Max(startupOptions.Concurrent, 1),
                InsertPageSize = Math.Max(startupOptions.InsertPageSize, 1),
                ReadCount = Math.Max(startupOptions.ReadCount, 1),
                ReadPageSize = Math.Max(startupOptions.ReadPageSize, 1),
                RecordCount = Math.Max(startupOptions.InsertCount, 1)
            };
            var databases = startupOptions.Databases;
            if (startupOptions.Databases == null || startupOptions.Databases.Length == 0 || startupOptions.Databases.Any(s => s == "*"))
            {
                databases = new string[] { "mysql", "mariadb", "postgresql", "sqlserver" };
            }
            foreach (var db in databases)
            {
                switch (db.ToLowerInvariant())
                {
                    case "sqlserver":
                        await TestRunner.TestDatabase<SqlTestDbContext>("SqlServer", options, serviceProvider).ConfigureAwait(false);
                        break;
                    case "mysql":
                        await TestRunner.TestDatabase<MySqlTestDbContext>("MySql", options, serviceProvider).ConfigureAwait(false);
                        break;
                    case "mariadb":
                        await TestRunner.TestDatabase<MariaDbTestDbContext>("MariaDb", options, serviceProvider).ConfigureAwait(false);
                        break;
                    case "postgresql":
                        await TestRunner.TestDatabase<PostgreSqlTestDbContext>("PostgreSql", options, serviceProvider).ConfigureAwait(false);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
