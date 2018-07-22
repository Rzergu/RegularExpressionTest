using System.Windows.Media;

namespace RegularExpressionData
{
    public interface IRegex
    {
        string Name { get; }
        string Regex { get; set; }

        Brush Color { get; set; }

    }
}
