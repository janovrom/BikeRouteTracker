using BikeRouteTracker.Extensions;
using BikeRouteTracker.Interfaces;
using BikeRouteTracker.Models;
using System;
using System.Collections.Generic;

namespace BikeRouteTracker.Services
{
    internal sealed class LocationService : ILocationService, ISpeedService, IElapsedTimeService
    {
        private readonly HashSet<ILocationListener> _Listeners = [];

        private Location? _PreviousLocation;
        private Location? _CurrentLocation;

        public double ElapsedTimeSeconds => 0;

        public double CurrentSpeed
        {
            get
            {
                if (_CurrentLocation is null || _PreviousLocation is null)
                    return 0;

                return _PreviousLocation.Distance(_CurrentLocation);
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
