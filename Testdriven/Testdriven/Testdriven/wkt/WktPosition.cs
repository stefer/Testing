using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testdriven.wkt
{
    public struct WktPosition
    {
        public ICollection<WktGeometry> Geometries;
        public WktCoordinate? SimplePosition;
    }

    public enum GeometryType
    {
        Point,
        Line,
        Polygon
    }

    public struct WktGeometry
    {
        public GeometryType Type;
        public ICollection<WktCoordinate> Coordinates;
    }

    public struct WktCoordinate
    {
        public double X;
        public double Y;
        public double Z;
    }
}
