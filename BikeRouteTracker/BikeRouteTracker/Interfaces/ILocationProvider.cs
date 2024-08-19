namespace BikeRouteTracker.Interfaces
{
    public interface ILocationProvider
    {
        void RequestLocationUpdates();
        void CancelLocationUpdates();
    }
}
