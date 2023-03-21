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
                options.UseInMemoryDatabase(Configuration.GetConnectionString("InMemoryDbName"));
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
