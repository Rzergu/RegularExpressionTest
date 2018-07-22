using System.Windows.Documents;

namespace RegularExpressionData
{
    public interface IMatch
    {
        IRegex Regex { get; }
        TextPointer StartTextPointer { get; }
        TextPointer EndTextPointer { get; }
        int StartOffset { get; }
        int EndOffset { get; }
    }
}
