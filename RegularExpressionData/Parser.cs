using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace RegularExpressionData
{
    public static class Parser
    {
        public static IEnumerable<Paragraph> ExtractParagraphs(BlockCollection blocks)
        {
            foreach (var block in blocks)
            {
                if (block is Paragraph para)
                    yield return para;
                else
                    foreach (var siblingBlock in ExtractParagraphs(block.SiblingBlocks))
                    {
                        yield return siblingBlock;
                    }
            }
        }
        public static IEnumerable<ExtendText> ExtractText(IEnumerable<Inline> inlines)
        {
            return inlines.SelectMany(ExtractText);
        }

        public static IEnumerable<ExtendText> ExtractText(Inline inline)
        {
            return ExtractTextEmb((dynamic)inline);
        }

        private static IEnumerable<ExtendText> ExtractTextEmb(Run run)
        {
            return new[] { new ExtendText() { Text = run.Text, StartText = run.ContentStart } };
        }

        private static IEnumerable<ExtendText> ExtractTextEmb(LineBreak br)
        {
            return new[] { new ExtendText() { Text = "\n", StartText = br.ContentStart } };
        }

        private static IEnumerable<ExtendText> ExtractTextEmb(Span span)
        {
            return ExtractText(span.Inlines);
        }

        // ReSharper disable once UnusedParameter.Local
        private static IEnumerable<ExtendText> ExtractTextEmb(Inline inline)
        {
            return Enumerable.Empty<ExtendText>();
        }
    }
}
