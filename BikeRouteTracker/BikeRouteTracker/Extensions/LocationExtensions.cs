using BikeRouteTracker.Models;
using System;

namespace BikeRouteTracker.Extensions
{
    internal static class LocationExtensions
    {
        internal static double Distance(this Location location1, Location location2)
        {
            const double earthRadius = 6371; // in kilometers

            double latitudeDifference = DegreesToRadians(location2.Latitude - location1.Latitude);
            double longitudeDifference = DegreesToRadians(location2.Longitude - location1.Longitude);

            double a = Math.Sin(latitudeDifference / 2) * Math.Sin(latitudeDifference / 2) +
                       Math.Cos(DegreesToRadians(location1.Latitude)) * Math.Cos(DegreesToRadians(location2.Latitude)) *
                       Math.Sin(longitudeDifference / 2) * Math.Sin(longitudeDifference / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = earthRadius * c;

            return distance;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
