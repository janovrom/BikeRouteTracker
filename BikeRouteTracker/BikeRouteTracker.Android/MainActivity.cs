using Android.App;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using BikeRouteTracker.Interfaces;
using Splat;
using System;

namespace BikeRouteTracker.Android
{
    [Activity(
        Label = "BikeRouteTracker.Android",
        Theme = "@style/MyTheme.NoActionBar",
        Icon = "@drawable/icon",
        MainLauncher = true,
        Permission = global::Android.Manifest.Permission.AccessFineLocation,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
    public class MainActivity : AvaloniaMainActivity<App>, global::Android.Locations.ILocationListener
    {
        private ILocationService? _locationService;

        public void OnLocationChanged(Location location)
        {
            _locationService?.LocationChanged(location.Latitude, location.Longitude);
        }

        public void OnProviderDisabled(string provider)
        {
            if (provider == LocationManager.GpsProvider)
                _locationService?.LocationProviderDisabled();
        }

        public void OnProviderEnabled(string provider)
        {
            if (provider == LocationManager.GpsProvider)
                _locationService?.LocationProviderEnabled();
        }

        public void OnStatusChanged(string? provider, [GeneratedEnum] Availability status, Bundle? extras)
        {
            if (provider != LocationManager.GpsProvider)
                return;

            switch (status)
            {
                case Availability.Available:
                    _locationService?.LocationProviderEnabled();
                    break;
                case Availability.OutOfService:
                    _locationService?.LocationProviderDisabled();
                    break;
                case Availability.TemporarilyUnavailable:
                    _locationService?.LocationProviderDisabled();
                    break;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window?.AddFlags(WindowManagerFlags.KeepScreenOn | WindowManagerFlags.Fullscreen);

            _locationService = Locator.Current.GetService<ILocationService>();
            
            InitializeLocationManager();
        }

        protected override void OnResume()
        {
            base.OnResume();
            _locationService?.LocationProviderEnabled();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationService?.LocationProviderDisabled();
        }

        protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
        {
            return base.CustomizeAppBuilder(builder)
                .WithInterFont()
                .UseReactiveUI();
        }

        private void InitializeLocationManager()
        {
            RequestPermissionsResult = (requestCodes, permissions, grantResults) =>
            {
                if (grantResults.Length > 0 && grantResults[0] == (int)Permission.Granted)
                {
                    LocationManager? locationManager = GetSystemService(LocationService) as LocationManager;

                    if (locationManager is null)
                        throw new InvalidOperationException("Cannot get location manager");

                    string locationProvider = LocationManager.GpsProvider;

                    if (locationManager.IsProviderEnabled(locationProvider))
                    {
                        locationManager.RequestLocationUpdates(locationProvider, 1000, 1, this);
                    }
                }
            };

            RequestPermissions([global::Android.Manifest.Permission.AccessFineLocation], 42);
        }
    }
}
