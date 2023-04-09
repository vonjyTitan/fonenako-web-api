
using fonenako_service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace fonenako_service_tests
{
    public class SpecFlowWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private const string InMemoryDbName = "InMemoryTestDB";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContext>();
                services.AddFonenakoDbContext(options =>
                {
                    options.UseInMemoryDatabase(InMemoryDbName);
                });
            });
        }
    }
}
