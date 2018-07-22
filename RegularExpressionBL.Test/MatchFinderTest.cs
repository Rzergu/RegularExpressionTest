using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Moq;
using NUnit.Framework;
using RegularExpressionData;

namespace RegularExpressionBL.Test
{
    [TestFixture]
    class MatchFinderTest
    {
        private MatchFinder _d;
        [SetUp]
        public void Init()
        {
            var rMock = new Mock<IRegex>();
            var rMock1 = new Mock<IRegex>();

            rMock.Setup(x => x.Name).Returns("Test1");
            rMock.Setup(x => x.Regex).Returns(@"\d{2}");

            rMock1.Setup(x => x.Name).Returns("Test2");
            rMock1.Setup(x => x.Regex).Returns(@"[a-z]");

            var v = new Run("w").ContentStart;

            var a = new RegexBuilder(new List<IRegex>(){rMock.Object,rMock1.Object});
            var b = new ExtendText() {StartText = v,Text = "Rest"};
            var c = new ExtendText() { StartText = v, Text = "123123" };

            _d = new MatchFinder(new List<ExtendText>(){b, c},a);
        }

        [Test]
        public void Text_Required()
        {
            //Assert
            Assert.AreEqual(_d._text,"Rest123123");
        }
        [Test]
        public void TextPointers_Required()
        {
            //Assert
            Assert.AreEqual(_d._textPointers.Count, 2);
        }
        [Test]
        public void Regex_Required()
        {
            //Assert
            Assert.AreEqual(_d._regex.Recognizer.ToString(), @"^((?<Test1>\d{2})|(?<Test2>[a-z]))");
        }
        [Test]
        public void Indices_Required()
        {
            //Assert
            Assert.AreEqual(_d._textIndices.Count,2);
        }
        [Test]
        public void Parse_Test()
        {
            //Act
            var m = _d.Parse().ToList();
            //Assert
            Assert.AreEqual(m.Count,6);
        }
        [Test]
        public void Parse_MatchType_Test()
        {
            //Act
            var m = _d.Parse().ToList();
            //Assert
            Assert.AreEqual(m.Where(x=>x.Regex.Name == "Test1").ToList().Count, 3);
        }
        [Test]
        public void Parse_MatchType2_Test()
        {
            //Act
            var m = _d.Parse().ToList();
            //Assert
            Assert.AreEqual(m.Where(x => x.Regex.Name == "Test2").ToList().Count ,3);
        }
    }
}
