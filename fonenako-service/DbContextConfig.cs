using System;
using fonenako.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace fonenako_service
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddFonenakoDbContext(this IServiceCollection service, IConfiguration Configuration)
        {
            service.AddFonenakoDbContext(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("PgsqlConnectionString")).LogTo(Console.WriteLine);
            });

            return service;
        }

        public static IServiceCollection AddFonenakoDbContext(this IServiceCollection service, Action<DbContextOptionsBuilder> optionsAction)
        {
            service.AddDbContextPool<FonenakoDbContext>(optionsAction);

            return service;
        }
    }
}
