using System;

namespace Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Data.Spatial;

    /// <summary>
    /// System.Data.Spatial.DbGeometry has methods to parse and conveert to WKT strings.
    /// It is very neat, but does not have any implementation itself, so extra libraries are required.
    /// In this case Microsoft.SqlServer.Types was loaded with NuGet, which added native dll:s that need 
    /// to be loaded at runtime.
    /// </summary>
    [TestClass]
    public class WktFromDbGeometry
    {

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
        }

        [TestMethod]
        public void DbGeometry_ShallParseWktPoint()
        {
            var geometry = DbGeometry.FromText("POINT (40 50)");

            Assert.AreEqual(true, geometry.IsSimple);
            Assert.AreEqual(0, geometry.Dimension);
            Assert.AreEqual(1, geometry.PointCount);
            Assert.AreEqual(1, geometry.ElementCount);
            Assert.AreEqual(40, geometry.XCoordinate.Value, 0.001);
            Assert.AreEqual(50, geometry.YCoordinate.Value, 0.001);
        }

        [TestMethod]
        public void DbGeometry_ShallParseWktLine()
        {
            var geometry = DbGeometry.FromText("LINESTRING (40 50, 100.6 150.0)");

            Assert.AreEqual(true, geometry.IsSimple);
            Assert.AreEqual(1, geometry.Dimension);
            Assert.AreEqual(1, geometry.ElementCount);
            Assert.AreEqual(2, geometry.PointCount);
            Assert.IsNull(geometry.XCoordinate);
            var p1 = geometry.PointAt(1);
            var p2 = geometry.PointAt(2);
            Assert.AreEqual(40, p1.XCoordinate.Value, 0.001);
            Assert.AreEqual(50, p1.YCoordinate.Value, 0.001);
            Assert.AreEqual(100.6, p2.XCoordinate.Value, 0.001);
            Assert.AreEqual(150.0, p2.YCoordinate.Value, 0.001);
        }

        [TestMethod]
        public void DbGeometry_ShallParseWktPolygon()
        {
            var geometry = DbGeometry.FromText("POLYGON ((40 50, 40 150.0, 100.4 150, 40 50))");

            Assert.AreEqual(true, geometry.IsSimple);
            Assert.AreEqual(2, geometry.Dimension);
            Assert.AreEqual(4, geometry.PointCount);
            Assert.AreEqual(1, geometry.ElementCount);
            Assert.IsNull(geometry.XCoordinate);
            var p1 = geometry.PointAt(1);
            var p2 = geometry.PointAt(3);
            Assert.AreEqual(40, p1.XCoordinate.Value, 0.001);
            Assert.AreEqual(50, p1.YCoordinate.Value, 0.001);
            Assert.AreEqual(100.4, p2.XCoordinate.Value, 0.001);
            Assert.AreEqual(150.0, p2.YCoordinate.Value, 0.001);

            Assert.AreEqual(4, geometry.ExteriorRing.PointCount);
            p1 = geometry.ExteriorRing.StartPoint;
            Assert.AreEqual(40, p1.XCoordinate.Value, 0.001);
            Assert.AreEqual(50, p1.YCoordinate.Value, 0.001);
        }

        [TestMethod]
        public void DbGeometry_ShallParseWktPolygonWithHoles()
        {
            var geometry = DbGeometry.FromText("POLYGON ((0 0, 0 100.0, 100 100, 100 0, 0 0), (10 10, 10 90, 90 90, 90 10, 10 10))");

            Assert.AreEqual(true, geometry.IsSimple);
            Assert.AreEqual(2, geometry.Dimension);
            Assert.AreEqual(10, geometry.PointCount, "All points in all rings");
            Assert.IsNull(geometry.XCoordinate, "It is not a point");
            var p1 = geometry.PointAt(1);
            var p2 = geometry.PointAt(3);
            Assert.AreEqual(0, p1.XCoordinate.Value, 0.001);
            Assert.AreEqual(0, p1.YCoordinate.Value, 0.001);
            Assert.AreEqual(100, p2.XCoordinate.Value, 0.001);
            Assert.AreEqual(100, p2.YCoordinate.Value, 0.001);

            Assert.AreEqual(5, geometry.ExteriorRing.PointCount);
            p1 = geometry.ExteriorRing.StartPoint;
            Assert.AreEqual(0, p1.XCoordinate.Value, 0.001);
            Assert.AreEqual(0, p1.YCoordinate.Value, 0.001);

            Assert.AreEqual(1, geometry.InteriorRingCount);
            Assert.AreEqual(5, geometry.InteriorRingAt(1).PointCount);
            p1 = geometry.InteriorRingAt(1).StartPoint;
            Assert.AreEqual(10, p1.XCoordinate.Value, 0.001);
            Assert.AreEqual(10, p1.YCoordinate.Value, 0.001);
        }
    }
}

