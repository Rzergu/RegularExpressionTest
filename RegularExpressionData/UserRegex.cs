using System.Windows.Media;

namespace RegularExpressionData
{
    public class UserRegex:IRegex
    {
        public string Name { get; set; }
        public string Regex { get; set; }
        public Brush Color { get; set; }
    }
}
