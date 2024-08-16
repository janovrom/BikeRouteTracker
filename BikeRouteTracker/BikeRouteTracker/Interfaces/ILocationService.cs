namespace BikeRouteTracker.Interfaces
{
    public interface ILocationService
    {
        void LocationChanged(double latitude, double longitude);
        void LocationProviderDisabled();
        void LocationProviderEnabled();
        void RegisterForUpdates(ILocationListener mainViewModel);
        void UnregisterFromUpdates(ILocationListener locationListener);
    }
}
