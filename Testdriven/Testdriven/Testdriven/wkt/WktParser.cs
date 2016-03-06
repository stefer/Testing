using SharpMap.Converters.WellKnownText;

namespace Testdriven.wkt
{
    public class WktParser
    {
        static ToWktConverter toWkt = new ToWktConverter();
        static FromWktConverter fromWkt = new FromWktConverter();

        public static WktPosition Parse(string wktString)
        {
            return toWkt.ToPosition(GeometryFromWKT.Parse(wktString));
        }

        public static string ToString(WktPosition position)
        {
            return fromWkt.FromPosition(position).ToString();
        }
    }
}
