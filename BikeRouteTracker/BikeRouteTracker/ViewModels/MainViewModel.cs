﻿using BikeRouteTracker.Interfaces;
using BikeRouteTracker.Models;
using BikeRouteTracker.Services;
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

        public ICommand StopCommand { get; init; }
        public ICommand StartCommand { get; init; }

        public MainViewModel
        (
            ILocationService locationService,
            ILocationRepository locationRepository,
            ISpeedService speedService,
            IElapsedTimeService elapsedTimeService
        )
        {
            _LocationService = locationService;
            _LocationRepository = locationRepository;
            _SpeedService = speedService;
            _ElapsedTimeService = elapsedTimeService;

            _LocationService.RegisterForUpdates(this);

            StopCommand = ReactiveCommand.Create(() =>
            {
                _LocationService.UnregisterFromUpdates(this);

                //string fileName = Path.Combine(
                //    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                //    "route.gpx");
                DateTime d = DateTime.UtcNow;
                string date = $"{d.Year}-{d.Month}-{d.Day}-{d.Hour}-{d.Minute}-{d.Second}";
                string fileName = $"/storage/emulated/0/Documents/route-{date}.gpx"; // "route.gpx

                using FileStream stream = File.OpenWrite(fileName);
                GpxWriter.Create()
                    .Write(_LocationRepository.GetAll())
                    .Close()
                    .WriteTo(stream);
            });

            StartCommand = ReactiveCommand.Create(() =>
            {
                _LocationService.RegisterForUpdates(this);
            });
        }

        public void LocationChanged(Location location)
        {
            _LocationRepository.Append(location);
            SpeedKph = (int)_SpeedService.CurrentSpeed;
        }

        public void OnProviderDisabled()
        {
            //SpeedKph = 0;

            //_LocationService.UnregisterFromUpdates(this);
        }

        public void OnProviderEnabled()
        {
            //SpeedKph = (int)_SpeedService.CurrentSpeed;

            //_LocationService.RegisterForUpdates(this);
        }
    }
}
