using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Testdriven.wkt;

namespace Tests
{
    [Binding]
    public class WktParserToStringSteps
    {        
        private WktPosition GetPosition()
        {
            return ScenarioContext.Current.Get<WktPosition>("position");
        }

        [StepArgumentTransformation(@"((?:.+,\s+)*(?:.+))")]
        public IList<double> DoubleCollectionTransform(string doubleList)
        {
            var splits = doubleList.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return splits.Select(x => double.Parse(x)).ToList();
        }

        [Given(@"I have a simple position with coordinates (.*), (.*)")]
        public void GivenItIsASimplePositionWithCoordinates(double p0, double p1)
        {
            WktPosition pos = new WktPosition { SimplePosition = new WktCoordinate { X = p0, Y = p1 } };
            ScenarioContext.Current.Add("position", pos);
        }

        [Given(@"I have a simple Line position with coordinates (.*)")]
        public void GivenIHaveASimpleLinePositionWithCoordinates(IList<double> coordinates)
        {
            WktPosition pos = new WktPosition
            {
                Geometries = new List<WktGeometry>
                {
                    new WktGeometry { Type = GeometryType.Line, Coordinates = CreateCoordinates(coordinates).ToList() }
                }
            };
            ScenarioContext.Current.Add("position", pos);
        }

        [Given(@"I have a position")]
        public void GivenIHaveAPosition()
        {
            ScenarioContext.Current.Add("position", new WktPosition());
        }

        [Given(@"has geometry (.*) with coordinates (.*)")]
        public void GivenGeometryIsLineWithCoordinates(string type, IList<double> coordinates)
        {
            var pos = GetPosition();
            if (pos.Geometries == null)
            {
                pos.Geometries = new List<WktGeometry>();
            }
            GeometryType geoType = (GeometryType)Enum.Parse(typeof(GeometryType), type);
            pos.Geometries.Add(new WktGeometry { Type = geoType, Coordinates = CreateCoordinates(coordinates).ToList() });

            ScenarioContext.Current["position"] = pos;
        }

        private IEnumerable<WktCoordinate> CreateCoordinates(IList<double> coordinates)
        {
            for (int i = 0; i < coordinates.Count / 2; i++)
            {
                yield return new WktCoordinate { X = coordinates[i * 2], Y = coordinates[i * 2 + 1] };
            }
        }

        [When(@"I call ToString")]
        public void WhenICallToString()
        {
            var pos = GetPosition();
            ScenarioContext.Current.Add("result", WktParser.ToString(pos));
        }
        
        [Then(@"the result should be '(.*)'")]
        public void ThenTheResultShouldBe(string p0)
        {
            var result = ScenarioContext.Current.Get<string>("result");
            Assert.AreEqual(p0, result);
        }
    }
}
