using System.Windows.Documents;

namespace RegularExpressionData
{
    public class GeneralMatch: IMatch
    {
        public IRegex Regex { get; }
        public TextPointer StartTextPointer { get; }
        public TextPointer EndTextPointer { get; }
        public int StartOffset { get; }
        public int EndOffset { get; }

        public GeneralMatch(IRegex regex, TextPointer startPoint, TextPointer endPoint, int startOffset, int endOffset)
        {
            Regex = regex;
            StartTextPointer = startPoint;
            EndTextPointer = endPoint;
            StartOffset = startOffset;
            EndOffset = endOffset;
        }

    }
}
