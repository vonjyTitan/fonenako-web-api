using fonenako_service.Daos;
using fonenako_service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace fonenako_service
{
    public static class ServicesConfig
    {
        public static IServiceCollection AddFonenakoServices(this IServiceCollection service)
        {
            service.AddScoped<ILeaseOfferService, LeaseOfferService>()
                .AddScoped<ILeaseOfferDao, LeaseOfferDao>()
                .AddSingleton<IFilterParser, FilterParser>();
            return service;
        }
    }
}
