using BikeRouteTracker.Models;

namespace BikeRouteTracker.Interfaces
{
    public interface ILocationListener
    {
        void LocationChanged(Location location);

        void OnProviderDisabled();

        void OnProviderEnabled();
    }
}
