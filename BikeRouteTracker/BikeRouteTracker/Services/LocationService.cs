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

        public void RegisterForUpdates(ILocationListener locationListener)
        {
            _Listeners.Add(locationListener);
        }

        public void UnregisterFromUpdates(ILocationListener locationListener)
        {
            _Listeners.Remove(locationListener);
        }

        public void LocationChanged(double latitude, double longitude)
        {
            Location location = new(latitude, longitude, DateTime.UtcNow);

            _previousLocation = _currentLocation;
            _currentLocation = location;

            _Listeners.ForEach((listener, location) => listener.OnLocationChanged(location), location);
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
