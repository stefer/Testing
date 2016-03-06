using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Testdriven.wkt
{
    public class ToWktConverter
    {
        public WktPosition ToPosition(IGeometry from)
        {
            WktPosition pos = new WktPosition();
            if (from is IPoint)
            {
                pos.SimplePosition = ToCoordinate(from.Coordinate);
            }
            else if (from is ILineString)
            {
                pos.Geometries = new List<WktGeometry>
                    {
                        ToGeometry((ILineString)from)
                    };
            }
            else if (from is IPolygon)
            {
                pos.Geometries = new List<WktGeometry>
                    {
                        ToGeometry((IPolygon)from)
                    };
            }
            else if (from is IMultiLineString)
            {
                var ml = (IMultiLineString)from;
                pos.Geometries = ml.Geometries.Select(x => ToGeometry((ILineString)x)).ToList();
            }
            else if (from is IMultiPoint)
            {
                var ml = (IMultiPoint)from;
                pos.Geometries = ml.Geometries.Select(x => ToGeometry((IPoint)x)).ToList();
            }
            else if (from is IMultiPolygon)
            {
                var ml = (IMultiPolygon)from;
                pos.Geometries = ml.Geometries.Select(x => ToGeometry((IPolygon)x)).ToList();
            }
            else if (from is IGeometryCollection)
            {
                pos.Geometries = ToGeometry((IGeometryCollection)from).ToList();
            }

            return pos;
        }

        private IEnumerable<WktGeometry> ToGeometry(IGeometryCollection from)
        {
            foreach (var geometry in from.Geometries)
            {
                if (geometry is IPoint)
                {
                    yield return ToGeometry((IPoint)geometry);
                }
                else if (geometry is ILineString)
                {
                    yield return ToGeometry((ILineString)geometry);
                }
                else if (geometry is IPolygon)
                {
                    yield return ToGeometry((IPolygon)geometry);
                }
                else
                {
                    throw new ApplicationException($"Converting {geometry.GeometryType} in GeometryCollection not supported");
                }
            }
        }

        public WktCoordinate ToCoordinate(Coordinate c)
        {
            return new WktCoordinate { X = c.X, Y = c.Y, Z = c.Z };
        }

        public WktGeometry ToGeometry(IPoint from)
        {
            return ToGeometry(GeometryType.Point, from);
        }

        public WktGeometry ToGeometry(ILineString from)
        {
            return ToGeometry(GeometryType.Line, from);
        }

        public WktGeometry ToGeometry(IPolygon from)
        {
            return new WktGeometry { Type = GeometryType.Polygon, Coordinates = from.ExteriorRing.Coordinates.Select(x => ToCoordinate(x)).ToList() };
        }

        private WktGeometry ToGeometry(GeometryType type, IGeometry from)
        {
            return new WktGeometry { Type = type, Coordinates = from.Coordinates.Select(x => ToCoordinate(x)).ToList() };
        }
    }
}
