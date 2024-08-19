using BikeRouteTracker.Interfaces;
using BikeRouteTracker.Models;

namespace BikeRouteTracker.Services
{
    internal sealed class LocationWritingListener : ILocationListener
    {
        private readonly ILocationRepository _LocationRepository;
        private readonly ILocationService _LocationService;

        internal LocationWritingListener(
            ILocationService locationService,
            ILocationRepository locationRepository)
        {
            _LocationService = locationService;
            _LocationRepository = locationRepository;

            locationService.RegisterForUpdates(this);
        }

        public void LocationChanged(Location location)
        {
            _LocationRepository.Append(location);
        }

        public void OnProviderDisabled()
        {
            //_LocationService.UnregisterFromUpdates(this);
        }

        public void OnProviderEnabled()
        {
            //_LocationService.RegisterForUpdates(this);
        }
    }
}
