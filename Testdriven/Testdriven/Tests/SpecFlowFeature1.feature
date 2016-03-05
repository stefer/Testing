Feature: SpecFlowFeature1
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Add two numbers
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen

Scenario Outline: Add two numbers with table
	Given I have entered the <number1> and <number2>
	When I press add
	Then the result should be <result> on the screen
Examples: 
| number1 | number2 | result |
| 70      | 50      | 120    |
| 120     | -70     | 50     |
| 50      | 70      | 120    |

