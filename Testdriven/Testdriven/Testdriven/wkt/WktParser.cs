using SharpMap.Converters.WellKnownText;

namespace Testdriven.wkt
{
    public class WktParser
    {
        public static WktPosition Parse(string wktString)
        {
            WktConverter converter = new WktConverter();

            return converter.Convert(GeometryFromWKT.Parse(wktString));

        }
    }
}
