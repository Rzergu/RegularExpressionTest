using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RegularExpressionData;

namespace RegularExpressionBL
{
    public class RegexBuilder
    {
        public List<IRegex> Regexes { get; set; }
        public static Regex CombineRegex(IEnumerable<IRegex> regexes)
        {
            var combinedRegexParts =
                $"^({string.Join("|", regexes.Select(kvp => $"(?<{kvp.Name}>{kvp.Regex})"))})";
            return new Regex(combinedRegexParts,
                RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);
        }

        public Regex Recognizer { get;}
        public IEnumerable<string> Names { get; }

        public RegexBuilder(List<IRegex> regexes)
        {
            Regexes = regexes ?? throw new ArgumentNullException(nameof(regexes));
            Recognizer = CombineRegex(regexes);
            Names = regexes.Select(x => x.Name);

        }
    }
}
