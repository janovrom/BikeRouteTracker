using BikeRouteTracker.Models;
using System;

namespace BikeRouteTracker.Extensions
{
    internal static class LocationExtensions
    {

        /// <summary>
        /// Calculates the distance between two locations using the Haversine formula.
        /// </summary>
        /// <param name="location1">The first location.</param>
        /// <param name="location2">The second location.</param>
        /// <returns>The distance between the two locations in kilometers.</returns>
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

        /// <summary>
        /// Calculates the time difference in hours between two locations.
        /// </summary>
        /// <param name="location1">The first location.</param>
        /// <param name="location2">The second location.</param>
        /// <returns>The time difference in hours.</returns>
        internal static double DeltaHours(this Location location1, Location location2)
        {
            return (location2.DateTime - location1.DateTime).TotalHours;
        }

        /// <summary>
        /// Calculates the speed in kilometers per hour between two locations.
        /// </summary>
        /// <param name="location1">The first location.</param>
        /// <param name="location2">The second location.</param>
        /// <returns>The speed in kilometers per hour.</returns>
        internal static double SpeedKph(this Location location1, Location location2)
        {
            double distance = location1.Distance(location2);
            double time = location1.DeltaHours(location2);

            if (Math.Abs(time) < double.Epsilon)
                return 0;

            return distance / time;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
