using BikeRouteTracker.Models;
using System.Collections.Generic;

namespace BikeRouteTracker.Interfaces
{
    public interface ILocationRepository
    {
        void Append(Location location);
        IEnumerable<Location> GetAll();
    }
}
