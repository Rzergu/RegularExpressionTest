using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using NUnit.Framework;
using RegularExpressionData;


namespace RegularExpressionDataTest
{
    [TestFixture]
    public class ParserTest
    {
        [Test]
        public void ExtractTextRun_Test()
        {
            //Arrange
            var a = new Run("Test");
            
            //Act
            var b = Parser.ExtractText(a);

            //Assert
            Assert.IsTrue(b.Any(x => x.Text == "Test"));

        }
        [Test]
        public void ExtractTextLineBreak_Test()
        {
            //Arrange
            var a = new LineBreak();

            //Act
            var b = Parser.ExtractText(a);

            //Assert
            Assert.IsTrue(b.Any(x => x.Text == "\n"));

        }
        [Test]
        public void ExtractTextSpanLineBreak_Test()
        {
            //Arrange
            var a = new Span(new LineBreak());

            //Act
            var b = Parser.ExtractText(a).ToList();

            //Assert
            Assert.IsTrue(b.Any(x => x.Text == "\n"));

        }
        [Test]
        public void ExtractTextSpanInline_Test()
        {
            //Arrange
            var a = new Span(new InlineUIContainer());

            //Act
            var b = Parser.ExtractText(a).ToList();

            //Assert
            Assert.IsTrue(b.Count == 0);

        }
        [Test]
        public void ExtractTextInline_Test()
        {
            //Arrange
            var a = new InlineUIContainer();

            //Act
            var b = Parser.ExtractText(a).ToList();

            //Assert
            Assert.IsTrue(b.Count == 0);

        }
        [Test]
        public void ExtractTextInlinesCount_Test()
        {
            //Arrange
            var a = new List<Inline>() { new Span(new Run("Test")),new LineBreak(),new Run("Test")};

            //Act
            var b = Parser.ExtractText(a).ToList();

            //Assert
            Assert.IsTrue(b.Count == 3);

        }
        [Test]
        public void ExtractTextInlines_Test()
        {
            //Arrange
            var a = new List<Inline>() { new Span(new Run("Test")), new LineBreak(), new Run("Test") };

            //Act
            var b = Parser.ExtractText(a).ToList();

            //Assert
            Assert.IsTrue(b.Count(x => x.Text == "Test") == 2);

        }
        [Test]
        public void ExtractTextInlinesUi_Test()
        {
            //Arrange
            var a = new List<Inline>() { new Span(new Run("Test")), new InlineUIContainer(), new InlineUIContainer() };

            //Act
            var b = Parser.ExtractText(a).ToList();

            //Assert
            Assert.IsTrue(b.Count == 1);

        }
    }
}
