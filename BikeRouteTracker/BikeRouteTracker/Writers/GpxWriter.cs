using BikeRouteTracker.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BikeRouteTracker.Writers
{
    internal sealed class GpxWriter
    {
        private const string GpxHeader = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>" +
            "<gpx xmlns=\"http://www.topografix.com/GPX/1/1\" version=\"1.1\" creator=\"Wikipedia\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd\">";
        private const string EmptySpace = "    ";
        private const string Metadata = @"
<metadata>
  <name>Route</name>
  <desc>Route from BikeRouteTracker</desc>
  <author>
   <name>Janovrom</name>
  </author>
 </metadata>
";

        private readonly StringBuilder _StringBuilder = new();

        private GpxWriter()
        {
            _StringBuilder.Append(GpxHeader);
            _StringBuilder.Append(Metadata);
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
