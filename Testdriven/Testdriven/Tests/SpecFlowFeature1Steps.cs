using System;
using TechTalk.SpecFlow;
using Testdriven.Calculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [Binding]
    public class SpecFlowFeature1Steps
    {
        Calculator calc = new Calculator();
        
        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            calc.PushArgument(p0);
        }
        
        [Given(@"I have entered the (.*) and (.*)")]
        public void GivenIHaveEnteredTheAnd(string p0, string p1)
        {
            calc.PushArgument(int.Parse(p0));
            calc.PushArgument(int.Parse(p1));
        }
        
        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            calc.ApplyOperation(Calculator.Operation.Add);
        }
        
        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            Assert.AreEqual(p0, calc.Result);
        }
    }
}
