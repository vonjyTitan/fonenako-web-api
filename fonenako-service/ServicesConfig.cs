
using fonenako_service.Controllers.Validator;
using fonenako_service.Daos;
using fonenako_service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace fonenako_service
{
    public static class ServicesConfig
    {
        public static IServiceCollection AddFonenakoServices(this IServiceCollection service)
        {
            service
                .AddScoped<ILeaseOfferService, LeaseOfferService>()
                .AddScoped<ILeaseOfferDao, LeaseOfferDao>()
                .AddScoped<ILocalisationService, LocalisationService>()
                .AddScoped<ILocalisationDao, LocalisationDao>()
                .AddSingleton<IEndPointInputValidatorFactory, EndPointInputValidatorFactory>();
            return service;
        }
    }
}
