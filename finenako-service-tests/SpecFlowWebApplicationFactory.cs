using System.Linq;
using fonenako.DatabaseContexts;
using fonenako_service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace fonenako_service_tests
{
    public class SpecFlowWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private const string InMemoryDbName = "InMemoryTestDB";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<FonenakoDbContext>));

                services.Remove(descriptor);
                services.AddFonenakoDbContext(options =>
                {
                    options.UseInMemoryDatabase(InMemoryDbName);
                });
            });
        }
    }
}
