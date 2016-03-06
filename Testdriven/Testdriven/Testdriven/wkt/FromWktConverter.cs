using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Testdriven.wkt
{
    public class FromWktConverter
    {
        IGeometryFactory factory = GeometryFactory.Default;

        public IGeometry FromPosition(WktPosition from)
        {
            if (from.SimplePosition != null)
            {
                return factory.CreatePoint(FromCoordinate(from.SimplePosition.Value));
            }
            else if (from.Geometries == null || from.Geometries.Count == 0)
            {
                throw new InvalidOperationException("Cannot convert a position with no geometries");
            }
            else if (from.Geometries.Count == 1)
            {
                var geometry = from.Geometries.First();
                return FromGeometry(geometry);
            }
            else
            {
                return CreateGeometryCollection(from.Geometries);
            }
        }

        private IGeometry CreateGeometryCollection(ICollection<WktGeometry> geometries)
        {
            if (geometries.All(x => x.Type == GeometryType.Line))
            {
                return factory.CreateMultiLineString(geometries.Select(x => FromGeometry(x)).Cast<ILineString>().ToArray());
            }
            else if (geometries.All(x => x.Type == GeometryType.Polygon))
            {
                return factory.CreateMultiPolygon(geometries.Select(x => FromGeometry(x)).Cast<IPolygon>().ToArray());
            }
            else
            {
                return factory.CreateGeometryCollection(geometries.Select(x => FromGeometry(x)).ToArray());
            }
        }

        private IGeometry FromGeometry(WktGeometry geometry)
        {
            switch(geometry.Type)
            {
                case GeometryType.Line:
                    return factory.CreateLineString(GetCoordinateArray(geometry.Coordinates));

                case GeometryType.Polygon:
                    return factory.CreatePolygon(GetCoordinateArray(geometry.Coordinates));

                case GeometryType.Point:
                    if (geometry.Coordinates.Count == 1)
                    {
                        return factory.CreatePoint(FromCoordinate(geometry.Coordinates.First()));
                    }
                    else
                    {
                        return factory.CreateMultiPoint(GetCoordinateArray(geometry.Coordinates));
                    }

                default:
                    throw new InvalidOperationException($"Unknown geometry type {geometry.Type}");
            }
        }

        private Coordinate[] GetCoordinateArray(ICollection<WktCoordinate> coordinates)
        {
            return coordinates.Select(x => FromCoordinate(x)).ToArray();
        }

        private Coordinate FromCoordinate(WktCoordinate coordinate)
        {
            return new Coordinate(coordinate.X, coordinate.Y, coordinate.Z ?? double.NaN);
        }
    }
}
