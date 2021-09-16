using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DatabaseBenchmark
{
    static class ServiceRegistrationExtensions
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services
                .AddLogging()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton(configuration)
                ;

            services.AddDbContext<SqlTestDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("SqlServer");
                if (connectionString.Contains("|DataDirectory|", StringComparison.OrdinalIgnoreCase))
                {
                    var dir = Path.Combine(AppContext.BaseDirectory, "Data");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    if (!dir.EndsWith("\\"))
                        dir += "\\";
                    connectionString = connectionString.Replace("|DataDirectory|", dir, StringComparison.OrdinalIgnoreCase);
                }
                options.UseSqlServer(connectionString);
            });
            services.AddDbContext<PostgreSqlTestDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgreSql"));
            });
            services.AddDbContext<MySqlTestDbContext>(options =>
            {
                var cs = configuration.GetConnectionString("MySql");
                options.UseMySql(cs, ServerVersion.AutoDetect(cs));
            });
            services.AddDbContext<MariaDbTestDbContext>(options =>
            {
                var cs = configuration.GetConnectionString("MariaDb");
                options.UseMySql(cs, ServerVersion.AutoDetect(cs));
            });
            return services;
        }
    }
}
