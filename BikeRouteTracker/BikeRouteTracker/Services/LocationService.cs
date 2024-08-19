using BikeRouteTracker.Extensions;
using BikeRouteTracker.Interfaces;
using BikeRouteTracker.Models;
using System;
using System.Collections.Generic;

namespace BikeRouteTracker.Services
{
    internal sealed class LocationService : ILocationService, ISpeedService, IElapsedTimeService
    {
        private const double _HoursToSeconds = 3600;

        private readonly HashSet<ILocationListener> _Listeners = [];
        private readonly ILocationProvider _LocationProvider;
        private Location? _previousLocation;
        private Location? _currentLocation;

        public double ElapsedTimeSeconds
        {
            get
            {
                if (_currentLocation is null || _previousLocation is null)
                    return 0;

                return _previousLocation.DeltaHours(_currentLocation) * _HoursToSeconds;
            }
        }

        public double CurrentSpeed
        {
            get
            {
                if (_currentLocation is null || _previousLocation is null)
                    return 0;

                return _previousLocation.SpeedKph(_currentLocation);
            }
        }

        public LocationService(ILocationProvider locationProvider)
        {
            _LocationProvider = locationProvider;
        }

        public void RegisterForUpdates(ILocationListener locationListener)
        {
            if (_Listeners.Count == 0)
            {
                _LocationProvider.RequestLocationUpdates();
            }

            _Listeners.Add(locationListener);
        }

        public void UnregisterFromUpdates(ILocationListener locationListener)
        {
            _Listeners.Remove(locationListener);

            if (_Listeners.Count == 0)
            {
                _LocationProvider.CancelLocationUpdates();
            }
        }

        public void LocationChanged(double latitude, double longitude)
        {
            Location location = new(latitude, longitude, DateTime.UtcNow);

            _previousLocation = _currentLocation;
            _currentLocation = location;

            _Listeners.ForEach((listener, location) => listener.LocationChanged(location), location);
        }

        public void LocationProviderDisabled()
        {
            _Listeners.ForEach(listener => listener.OnProviderDisabled());
        }

        public void LocationProviderEnabled()
        {
            _Listeners.ForEach(listener => listener.OnProviderEnabled());
        }
    }
}
