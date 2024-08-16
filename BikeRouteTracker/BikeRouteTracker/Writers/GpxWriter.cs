using BikeRouteTracker.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BikeRouteTracker.Writers
{
    internal sealed class GpxWriter
    {
        private const string GpxHeader = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        private const string EmptySpace = "    ";

        private readonly StringBuilder _StringBuilder = new();

        private GpxWriter()
        {
            _StringBuilder.Append(GpxHeader);
        }

        internal static GpxWriter Create()
        {
            return new GpxWriter();
        }

        internal Gpx Close()
        {
            _StringBuilder.Append("</gpx>");

            return new Gpx(_StringBuilder.ToString());
        }

        internal GpxWriter Write(IEnumerable<Location> locations)
        {
            foreach (Location location in locations)
            {
                Write(location);
            }

            return this;
        }

        internal GpxWriter Write(Location location)
        {
            _StringBuilder.AppendLine($"{EmptySpace}<wpt lat=\"{location.Latitude}\" lon=\"{location.Longitude}\">");

            _StringBuilder.AppendLine($"{EmptySpace}{EmptySpace}<time>{location.DateTime}</time>");

            _StringBuilder.AppendLine($"{EmptySpace}</wpt>");

            return this;
        }

        internal sealed class Gpx
        {
            private readonly string _Content;

            internal Gpx(string v)
            {
                _Content = v;
            }

            public void WriteTo(Stream stream)
            {
                stream.Write(Encoding.UTF8.GetBytes(_Content));
            }
        }
    }
}
