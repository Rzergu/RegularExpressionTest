using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using RegularExpressionData;

namespace RegularExpressionBL.Test
{
    [TestFixture]
    public class RegexBuilderTest
    {
        [Test]
        public void CombineRegex_Test()
        {
            //Arrange
            var a = new Mock<IRegex>();
            a.Setup(x => x.Regex).Returns(@"\d");
            a.Setup(x => x.Name).Returns("Test");

            //Act
            var b = RegexBuilder.CombineRegex(new List<IRegex>(){a.Object,a.Object});

            //Assert
            Assert.AreEqual(b.ToString(), @"^((?<Test>\d)|(?<Test>\d))");
        }
        [Test]
        public void CombineRegexEmpty_Test()
        {
            //Act
            var b = RegexBuilder.CombineRegex(new List<IRegex>());

            //Assert
            Assert.AreEqual(b.ToString(),"^()");
        }
        [Test]
        public void CombineRegexRecognizerRequired_Test()
        {
            //Arrange
            var a = new Mock<IRegex>();
            a.Setup(x => x.Regex).Returns(@"\d");
            a.Setup(x => x.Name).Returns("Test");



            //Act
            var b = new RegexBuilder(new List<IRegex>() { a.Object, a.Object });

            //Assert
            Assert.AreEqual(b.Recognizer.ToString(), @"^((?<Test>\d)|(?<Test>\d))");
        }
        [Test]
        public void CombineRegexNamesRequired_Test()
        {
            //Arrange
            var a = new Mock<IRegex>();
            a.Setup(x => x.Regex).Returns(@"\d");
            a.Setup(x => x.Name).Returns("Test");


            //Act
            var b = new RegexBuilder(new List<IRegex>() { a.Object, a.Object, a.Object });

            //Assert
            Assert.IsTrue(b.Names.All(x => x.ToString() == "Test"));
        }
    }
}
