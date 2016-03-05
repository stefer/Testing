Feature: Wkt string parser
	As a goegrapher
	I want to parse and build wkt strings in multiple formats
	So that I can use positions freely
	
@mytag
Scenario: Simple position
	Given I have a string with value 'POINT (40 50)' 
	When I call parse method
	Then the result should be a Simple Point with values 40, 50

Scenario: Linestring
	Given I have a string with value 'LINESTRING (40 50, 100 120)' 
	When I call parse method
	Then the result should have 1 geometry
	And geometry 0 should be a Line
	And geometry 0 should have 2 coordinates starting with 40, 50, 100, 120

Scenario: Polygon
	Given I have a string with value 'Polygon ((0 0, 100 0, 100 100, 0 100, 0 0 ))' 
	When I call parse method
	Then the result should have 1 geometry
	And geometry 0 should be a Polygon
	And geometry 0 should have 5 coordinates starting with 0, 0, 100, 0

Scenario: Polygon ignores holes
	Given I have a string with value 'Polygon ((0 0, 100 0, 100 100, 0 100, 0 0 ), (10 10, 90 10, 90 90, 10 90, 10 10 ))' 
	When I call parse method
	Then the result should have 1 geometry
	And geometry 0 should be a Polygon
	And geometry 0 should have 5 coordinates starting with 0, 0, 100, 0


Scenario: MultiPoint
	Given I have a string with value 'MULTIPOINT (40 50, 100 120)' 
	When I call parse method
	Then the result should have 2 geometry
	And geometry 0 should be a Point
	And geometry 0 should have 1 coordinates starting with 40, 50
	And geometry 1 should be a Point
	And geometry 1 should have 1 coordinates starting with 100, 120

Scenario: MultiLinestring
	Given I have a string with value 'MULTILINESTRING ((10 10, 20 20, 10 40), (40 40, 30 30, 40 20, 30 10))' 
	When I call parse method
	Then the result should have 2 geometry
	And geometry 0 should be a Line
	And geometry 0 should have 3 coordinates starting with 10, 10, 20, 20
	And geometry 1 should be a Line
	And geometry 1 should have 4 coordinates starting with 40, 40, 30, 30	

Scenario: Multipolygon
	Given I have a string with value 'MULTIPOLYGON (((40 40, 20 45, 45 30, 40 40)), ((20 35, 10 30, 10 10, 30 5, 45 20, 20 35), (30 20, 20 15, 20 25, 30 20)))' 
	When I call parse method
	Then the result should have 2 geometry
	And geometry 0 should be a Polygon
	And geometry 0 should have 4 coordinates starting with 40, 40, 20, 45
	And geometry 1 should be a Polygon
	And geometry 1 should have 6 coordinates starting with 20, 35, 10, 30

Scenario: GeometryCollection
	Given I have a string with value 'GEOMETRYCOLLECTION (POINT (10 20), LINESTRING (40 50, 100 120), POLYGON ((0 0, 100 0, 100 100, 0 100, 0 0 )))' 
	When I call parse method
	Then the result should have 3 geometry
	And geometry 0 should be a Point
	And geometry 0 should have 1 coordinates starting with 10, 20
	And geometry 1 should be a Line
	And geometry 1 should have 2 coordinates starting with 40, 50, 100, 120
	And geometry 2 should be a Polygon
	And geometry 2 should have 5 coordinates starting with 0, 0, 100, 0
