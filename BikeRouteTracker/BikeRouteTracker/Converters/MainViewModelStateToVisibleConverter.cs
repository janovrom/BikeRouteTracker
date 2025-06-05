using System;
using System.Globalization;
using Avalonia.Data.Converters;
using BikeRouteTracker.ViewModels;

namespace BikeRouteTracker.Converters
{
    public class MainViewModelStateToVisibleConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not MainViewModelState state)
            {
                return false;
            }

            if (parameter is string param)
            {
                return param switch
                {
                    "Stopped" => state == MainViewModelState.Stopped,
                    "Starting" => state == MainViewModelState.Starting,
                    "Running" => state == MainViewModelState.Running,
                    "Paused" => state == MainViewModelState.Paused,
                    _ => false
                };
            }

            return false;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}