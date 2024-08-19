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
using BikeRouteTracker.Services;
using Splat;
using System;
using System.Collections.Generic;

namespace BikeRouteTracker.Android
{
    [Activity(
        Label = "BikeRouteTracker.Android",
        Theme = "@style/MyTheme.NoActionBar",
        Icon = "@drawable/icon",
        MainLauncher = true,
        Exported = true,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
    public class MainActivity : AvaloniaMainActivity<App>, global::Android.Locations.ILocationListener, ILocationProvider
    {
        private ILocationService? _locationService;
        private LocationManager? _locationManager;
        private readonly List<Interfaces.ILocationListener> _Listeners = [];

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
            RegisterDependencies();
            base.OnCreate(savedInstanceState);

            Window?.AddFlags(WindowManagerFlags.KeepScreenOn | WindowManagerFlags.Fullscreen);

            _locationService = Locator.Current.GetService<ILocationService>();
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

        private void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant<ILocationProvider>(this);
        }

        public void RequestLocationUpdates()
        {
            if (_locationManager is not null)
            {
                return;
            }

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
                        locationManager.RequestLocationUpdates(locationProvider, 1000, 0, this);
                    }
                }
            };

            RequestPermissions([global::Android.Manifest.Permission.AccessFineLocation], 42);
        }

        public void CancelLocationUpdates()
        {
            if (_locationManager is null)
                return;

            _locationManager.RemoveUpdates(this);
            _locationManager = null;
        }
    }
}
