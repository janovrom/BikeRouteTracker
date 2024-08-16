using BikeRouteTracker.Interfaces;
using BikeRouteTracker.Models;
using BikeRouteTracker.Writers;
using ReactiveUI;
using System;
using System.IO;
using System.Windows.Input;

namespace BikeRouteTracker.ViewModels
{
    public class MainViewModel : ViewModelBase, ILocationListener
    {
        private readonly ILocationService _LocationService;
        private readonly ILocationRepository _LocationRepository;
        private readonly ISpeedService _SpeedService;
        private readonly IElapsedTimeService _ElapsedTimeService;

        private int _speedKph = 0;
        public int SpeedKph
        {
            get => _speedKph;
            set => this.RaiseAndSetIfChanged(ref _speedKph, value);
        }

        private ICommand StopCommand { get; init; }

        public MainViewModel()
        {
        }

        public MainViewModel(
            ILocationService locationService,
            ILocationRepository locationRepository,
            ISpeedService speedService,
            IElapsedTimeService elapsedTimeService)
        {
            _LocationService = locationService;
            _LocationRepository = locationRepository;
            _SpeedService = speedService;
            _ElapsedTimeService = elapsedTimeService;

            _LocationService.RegisterForUpdates(this);

            StopCommand = ReactiveCommand.Create(() =>
            {
                _LocationService.UnregisterFromUpdates(this);

                string fileName = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "route.gpx");

                using FileStream stream = File.OpenWrite(fileName);
                GpxWriter.Create()
                    .Write(_LocationRepository.GetAll())
                    .Close()
                    .WriteTo(stream);
            });
        }

        public void OnLocationChanged(Location location)
        {
            SpeedKph = (int)_SpeedService.CurrentSpeed;
        }

        public void OnProviderDisabled()
        {
            SpeedKph = 0;

            _LocationService.UnregisterFromUpdates(this);
        }

        public void OnProviderEnabled()
        {
            SpeedKph = (int)_SpeedService.CurrentSpeed;

            _LocationService.RegisterForUpdates(this);
        }
    }
}
