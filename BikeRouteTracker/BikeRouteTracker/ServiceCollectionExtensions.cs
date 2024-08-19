using BikeRouteTracker.Interfaces;
using BikeRouteTracker.Services;
using BikeRouteTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BikeRouteTracker
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ILocationService, LocationService>();
            services.AddSingleton<ISpeedService>(services => (ISpeedService)services.GetRequiredService<ILocationService>());
            services.AddSingleton<IElapsedTimeService>(services => (IElapsedTimeService)services.GetRequiredService<ILocationService>());
            services.AddSingleton<ILocationRepository, LocationRepository>();

            services.AddTransient<MainViewModel>();

            return services;
        }
    }
}
