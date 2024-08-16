using BikeRouteTracker.Models;

namespace BikeRouteTracker.Interfaces
{
    public interface ILocationListener
    {
        void OnLocationChanged(Location location);

        void OnProviderDisabled();

        void OnProviderEnabled();
    }
}
