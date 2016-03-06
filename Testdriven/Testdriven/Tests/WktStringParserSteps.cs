using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TechTalk.SpecFlow;
using Testdriven.wkt;
using System.Data.Spatial;
using System.Linq;
using System.Collections.Generic;

namespace Tests
{
    [Binding]
    public class WktStringParserSteps
    {
        [StepArgumentTransformation(@"((?:.+,\s+)*(?:.+))")]
        public IList<double> DoubleCollectionTransform(string doubleList)
        {
            var splits = doubleList.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return splits.Select(x => double.Parse(x)).ToList();
        }

        [Given(@"I have a string with value '(.*)'")]
        public void GivenIHaveAStringWithValue(string p0)
        {
            ScenarioContext.Current.Add("wktString", p0);
        }
        
        [When(@"I call parse method")]
        public void WhenICallParseMethod()
        {
            var wktString = ScenarioContext.Current.Get<string>("wktString");

            var geometry = WktParser.Parse(wktString);
            ScenarioContext.Current.Add("result", geometry);
        }

        [Then(@"the result should be a Simple Point with values (.*)")]
        public void ThenTheResultShouldBeAPointWithValues(IList<double> values)
        {
            var geometry = GetPosition();

            Assert.AreEqual(values[0], geometry.SimplePosition.Value.X, 0.001);
            Assert.AreEqual(values[1], geometry.SimplePosition.Value.Y, 0.001);
        }

        [Then(@"the result should have (.*) geometry")]
        public void ThenTheResultShouldHaveGeometry(int numGeometries)
        {
            var position = GetPosition();

            Assert.AreEqual(numGeometries, position.Geometries.Count);
        }

        private WktPosition GetPosition()
        {
            return ScenarioContext.Current.Get<WktPosition>("result");
        }

        [Then(@"geometry (.*) should be a (.*)")]
        public void ThenGeometryShouldBeALine(int geometryIndex, string type)
        {
            var position = GetPosition();
            var geom = position.Geometries.ElementAt(geometryIndex);
            var expectedType = Enum.Parse(typeof (GeometryType), type);

            Assert.AreEqual(expectedType, geom.Type);
        }

        [Then(@"geometry (.*) should have (.*) coordinates starting with (.*)")]
        public void ThenGeometryShouldHaveCoordinates(int geometryIndex, int numCoordinates, IList<double> values)
        {
            var position = GetPosition();
            var geometry = position.Geometries.ElementAt(geometryIndex);
            Assert.IsNull(position.SimplePosition);

            var coordinates = geometry.Coordinates.ToList();

            Assert.AreEqual(numCoordinates, coordinates.Count);

            for (int i = 0; i < values.Count / 2; i++)
            {
                Assert.AreEqual(values[i * 2], coordinates[i].X);
                Assert.AreEqual(values[i * 2 + 1], coordinates[i].Y);
            }
        }


        [Then(@"the result should be a Line with values (.*)")]
        public void ThenTheResultShouldBeALineWithValues(IList<double> values)
        {
            var geometry = ScenarioContext.Current.Get<WktPosition>("result");

            Assert.AreEqual(1, geometry.Geometries.Count);
            Assert.AreEqual(GeometryType.Line, geometry.Geometries.First().Type);
            Assert.IsNull(geometry.SimplePosition);

            var coordinates = geometry.Geometries.First().Coordinates.ToList();

            for (int i = 0; i < values.Count / 2; i++)
            {
                Assert.AreEqual(values[i*2], coordinates[i].X);
                Assert.AreEqual(values[i * 2 + 1], coordinates[i].Y);
            }
        }

        [Then(@"the result should be a Polygon with values (.*)")]
        public void ThenTheResultShouldBeAPolygonWithValues(IList<double> values)
        {
            var geometry = ScenarioContext.Current.Get<WktPosition>("result");

            Assert.AreEqual(1, geometry.Geometries.Count);
            Assert.AreEqual(GeometryType.Polygon, geometry.Geometries.First().Type);
            Assert.IsNull(geometry.SimplePosition);

            var coordinates = geometry.Geometries.First().Coordinates.ToList();

            for (int i = 0; i < values.Count / 2; i++)
            {
                Assert.AreEqual(values[i * 2], coordinates[i].X);
                Assert.AreEqual(values[i * 2 + 1], coordinates[i].Y);
            }
        }


    }
}
