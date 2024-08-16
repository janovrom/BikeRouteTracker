using BikeRouteTracker.Interfaces;
using BikeRouteTracker.Models;
using System.Collections.Generic;

namespace BikeRouteTracker.Services
{
    internal sealed class LocationRepository : ILocationRepository
    {
        #region Data members

        private readonly IList<Location> _Locations = [];

        #endregion //Data members

        #region Public methods

        public IEnumerable<Location> GetAll()
        {
            return _Locations;
        }

        public void Append(Location location)
        {
            _Locations.Add(location);
        }

        #endregion //Public methods
    }
}
