using SharpMap.Converters.WellKnownText;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Testdriven.wkt
{
    public class WktParser
    {
        public struct WktPosition
        {
            public ICollection<WktGeometry> Geometries;
            public WktCoordinate? SimplePosition;
        }

        public struct WktGeometry
        {
            public enum GeometryType
            {
                Point,
                Line,
                Polygon
            }

            public GeometryType Type;
            public ICollection<WktCoordinate> Coordinates;
        }

        public struct WktCoordinate
        {
            public double X;
            public double Y;
            public double Z;
        }

        public class WktConverter
        {
            public IGeometry Convert(WktPosition from)
            {
                throw new NotImplementedException();
            }

            public WktPosition Convert(IGeometry from)
            {
                WktPosition pos = new WktPosition();
                if (from is IPoint)
                {
                    pos.SimplePosition = Convert(from.Coordinate);
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
                foreach(var geometry in from.Geometries)
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

            public WktCoordinate Convert(Coordinate c)
            {
                return new WktCoordinate { X = c.X, Y = c.Y, Z = c.Z };
            }

            public WktGeometry ToGeometry(IPoint from)
            {
                return ToGeometry(WktParser.WktGeometry.GeometryType.Point, from);
            }

            public WktGeometry ToGeometry(ILineString from)
            {
                return ToGeometry(WktParser.WktGeometry.GeometryType.Line, from);
            }

            public WktGeometry ToGeometry(IPolygon from)
            {
                return new WktGeometry { Type = WktParser.WktGeometry.GeometryType.Polygon, Coordinates = from.ExteriorRing.Coordinates.Select(x => Convert(x)).ToList() };
            }

            private WktGeometry ToGeometry(WktParser.WktGeometry.GeometryType type, IGeometry from)
            {
                return new WktGeometry { Type = type, Coordinates = from.Coordinates.Select(x => Convert(x)).ToList() };
            }
        }

        public static WktPosition Parse(string wktString)
        {
            WktConverter converter = new WktConverter();

            return converter.Convert(GeometryFromWKT.Parse(wktString));

        }
    }
}
