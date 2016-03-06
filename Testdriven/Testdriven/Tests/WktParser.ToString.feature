Feature: WktParser
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Simple Position
	Given I have a simple position with coordinates 40, 50
	When I call ToString
	Then the result should be 'POINT (40 50)'

Scenario: Simple Line
	Given I have a position 
	And has geometry Line with coordinates 40, 50, 100, 150
	When I call ToString
	Then the result should be 'LINESTRING (40 50, 100 150)'

Scenario: Simple Polygon
	Given I have a position 
	And has geometry Polygon with coordinates 0, 0, 100, 0, 100, 100, 0, 100, 0, 0 
	When I call ToString
	Then the result should be 'POLYGON ((0 0, 100 0, 100 100, 0 100, 0 0))'

Scenario: Multi Line
	Given I have a position 
	And has geometry Line with coordinates 40, 50, 100, 150
	And has geometry Line with coordinates 140, 150, 200, 250
	When I call ToString
	Then the result should be 'MULTILINESTRING ((40 50, 100 150), (140 150, 200 250))'

Scenario: Multi Polygon
	Given I have a position 
	And has geometry Polygon with coordinates 40, 50, 100, 150, 24, 56, 40, 50
	And has geometry Polygon with coordinates 140, 150, 10, 15, 240, 560, 140, 150
	When I call ToString
	Then the result should be 'MULTIPOLYGON (((40 50, 100 150, 24 56, 40 50)), ((140 150, 10 15, 240 560, 140 150)))'
