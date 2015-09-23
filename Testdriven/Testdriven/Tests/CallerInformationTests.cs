using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testdriven;

namespace Tests
{
    [TestClass]
    public class CallerInformationTests
    {

        CallerInformation ci = new CallerInformation();

        [TestMethod]
        public void CallerMemberName_ShouldReturnCallingMethodName()
        {
            Assert.AreEqual(ci.GetCallerMemberName(), "CallerMemberName_ShouldReturnCallingMethodName");
        }

        [TestMethod]
        public void CallerFilePath_ShouldReturnPathToCallingMethodSourceFile()
        {
            Assert.IsTrue(ci.GetCallerFillePath().Contains("CallerInformationTests.cs"));
        }

        [TestMethod]
        public void CallerLineNumber_ShouldReturnCallingMethodLineNumber()
        {
            int line = ci.GetCallerLineNumber();
            Assert.IsTrue(line > 31 && line < 35);
        }
    }
}
