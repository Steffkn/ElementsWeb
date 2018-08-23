using Elements.Services.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Elements.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void Truncate_WithStringEmpty_ShouldReturnEmptyString()
        {
            string inputString = string.Empty;
            var result = inputString.Truncate(90000);
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Truncate_WithNullString_ShouldReturnEmptyString()
        {
            string inputString = null;
            var result = inputString.Truncate(90000);
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Truncate_WithNegativeLength_ShouldNotThrow()
        {
            string inputString = string.Empty;
            var result = inputString.Truncate(-1);
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Truncate_WithBigString_ShouldReturnCorrectly()
        {
            string inputString = "12345667890";
            var result = inputString.Truncate(5);
            Assert.IsNotNull(result);
            Assert.AreEqual("12345...", result);
        }


        [TestMethod]
        public void Truncate_WithBigMaxLenght_ShouldReturnSameString()
        {
            string inputString = "12345667890";
            var result = inputString.Truncate(20);
            Assert.IsNotNull(result);
            Assert.AreEqual("12345667890", result);
        }
    }
}
