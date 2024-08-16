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
            LocationService locationService = new();
            services.AddSingleton<ILocationService>(locationService);
            services.AddSingleton<ISpeedService>(locationService);
            services.AddSingleton<IElapsedTimeService>(locationService);
            services.AddSingleton<ILocationRepository, LocationRepository>();

            services.AddTransient<MainViewModel>();

            return services;
        }
    }
}
