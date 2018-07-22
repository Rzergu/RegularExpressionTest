using System.Windows.Media;

namespace RegularExpressionData
{
    class SystemRegex:IRegex
    {
        public string Name { get; }
        public string Regex { get; set; }
        public Brush Color { get; set; }

        public SystemRegex(string name, string regex, Brush color)
        {
            Name = name;
            Regex = regex;
            Color = color;
        }
    }
}
