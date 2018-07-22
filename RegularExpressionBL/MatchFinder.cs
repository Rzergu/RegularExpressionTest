using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using RegularExpressionData;

[assembly: InternalsVisibleTo("RegularExpressionBL.Test")]
namespace RegularExpressionBL
{
    public class MatchFinder
    {
        internal readonly string _text;
        internal readonly List<TextPointer> _textPointers = new List<TextPointer>();
        internal readonly List<int> _textIndices = new List<int>();

        internal readonly RegexBuilder _regex;

        public static async Task<List<IMatch>> Parse(IEnumerable<ExtendText> texts, RegexBuilder regex)
        {
            var lexer = new MatchFinder(texts, regex);
            return await Task.Run(() => lexer.Parse().ToList());
        }


        public MatchFinder(IEnumerable<ExtendText> texts, RegexBuilder regex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var text in texts)
            {
                _textIndices.Add(stringBuilder.Length);
                stringBuilder.Append(text.Text);
                _textPointers.Add(text.StartText);
            }

            _text = stringBuilder.ToString();
            _regex = regex;
        }
        internal Tuple<TextPointer, int> GetBasePointerAndOffset(int position)
        {
            var partNo = _textIndices.BinarySearch(position);
            if (partNo < 0)
                partNo = ~partNo - 1;
            var partStart = _textIndices[partNo];
            var delta = position - partStart;
            return Tuple.Create(_textPointers[partNo], delta);
        }
        internal IEnumerable<IMatch> Parse()
        {
            string restLine = _text;
            int currPos = 0;

            while (restLine != "")
            {
                var match = _regex.Recognizer.Match(restLine);
                var nameAndGroup =
                    _regex.Names.Select(name => new {name, group = match.Groups[name]})
                        .SingleOrDefault(ng => ng.group.Success);

                if (nameAndGroup == null)
                {
                    restLine = restLine.Substring(1);
                    currPos++;
                    continue;
                }
                var length = nameAndGroup.group.Length;

                var tokenType = _regex.Regexes.Find(x => x.Name == nameAndGroup.name);

                if (tokenType != null)
                {
                    var start = GetBasePointerAndOffset(currPos);
                    var end = GetBasePointerAndOffset(currPos + length);
                    yield return new GeneralMatch(tokenType, start.Item1, end.Item1, start.Item2, end.Item2);
                }
                currPos += length;
                restLine = restLine.Substring(length);
            }
        }
    }
}
